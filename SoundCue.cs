using StardewModdingAPI;
using StardewValley.Locations;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using StardewModdingAPI.Utilities;

namespace LoonCallsMod
{
    internal class SoundCue
    {
        private Mod Mod;
        private Configuration Configuration;
        private int LastCallTime = 0;
        private int LastCallIndex = -1;
        private int ChecksSinceLastCall = 0;
        private List<string> CueNames;
        private Random rng;

        public SoundCue(Mod mod)
        {
            Mod = mod;
            rng = new Random();
            Configuration = mod.Helper.ReadConfig<Configuration>();
            LogMessage("Config Loaded");
            CueNames = new List<string>(); 
            BuildAudioCues();
        }

        public void CheckforLoonCue(int time) {
            try
            {
                var location = Game1.currentLocation;
                ChecksSinceLastCall++;

                if (IsGameInCueTime(time) &&
                    IsPlayerInCueLocation(location) &&
                    IsPlayerInCueSeason())
                {
                    var rand = rng.Next(0, Configuration.CallOdds);
                    if (rand == 0 && ChecksSinceLastCall >= (Configuration.MinimumCallInterval / 10))
                    {
                        CueLoonCall(location);
                        LastCallTime = time;
                        ChecksSinceLastCall = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Checking for sound cue failed with exception: {ex.Message}");
            }
        }

        //reset last call time at end of day
        public void DayEnded()
        {
            LastCallTime = 0;
            ChecksSinceLastCall = 0;
            LogMessage("Day Ended");
        }

        //Select random call from list of audio clips
        private void CueLoonCall(GameLocation loc)
        {
            try
            {
                var rand = rng.Next(0, CueNames.Count);
                if (rand == LastCallIndex)//Prevent same call from playing twice in a row, iterate index to next cue
                {
                    rand = (rand + 1) % CueNames.Count;
                }

                string cueName = CueNames[rand];
                LastCallIndex = rand;
                LogMessage($"Cued sound {cueName}");
                Game1.playSound(cueName);
            }
            catch (Exception ex)
            {
                LogMessage($"Sound cue playback failed with exception: {ex.Message}");
            }
        }

        //Add sounds from asset folder to game's sound bank
        private void BuildAudioCues()
        {
            try
            {
                string soundsDirectory = Path.Combine(Mod.Helper.DirectoryPath, "Sounds");
                string[] files = Directory.GetFiles(soundsDirectory);

                foreach (string file in files)
                {
                    CueDefinition sound = new CueDefinition();
                    sound.name = Path.GetFileNameWithoutExtension(file);

                    SoundEffect audio;
                    using (var stream = new System.IO.FileStream(file, System.IO.FileMode.Open))
                    {
                        audio = SoundEffect.FromStream(stream);
                    }
                    sound.SetSound(audio, Game1.audioEngine.GetCategoryIndex("Ambient"), false, true);
                    Game1.soundBank.AddCue(sound);
                    CueNames.Add(sound.name);

                    LogMessage($"Sound file {sound.name} loaded to soundbank");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Building sound cues failed with exception: {ex.Message}");
            }
        }

        private bool IsPlayerInCueLocation(GameLocation location)
        {
            return location is Mountain ||
                location is Farm ||
                location is Woods ||
                location is Forest;
        }

        private bool IsPlayerInCueSeason()
        {
            return !Configuration.RestrictedSeasons.Contains(Game1.currentSeason.ToLower());
        }

        private bool IsGameInCueTime(int time)
        {
            return time >= Configuration.CallStartTime && time <= Configuration.CallEndTime;
        }

        private void LogMessage(string mes)
        {
            if(Configuration.EnableDebugConsole)
            {
                Mod.Monitor.Log($"{mes}", LogLevel.Debug);
            }
        }
    }
}
