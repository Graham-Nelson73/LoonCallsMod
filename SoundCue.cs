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
        private int LastCallTime = 600;
        private int? LastCallIndex = null;
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
            var location = Game1.currentLocation;
            if (IsGameInCueTime(time) &&
                IsPlayerInCueLocation(location) &&
                IsPlayerInCueSeason())
            {
                var rand = rng.Next(1, Configuration.CallOdds + 1);
                if (rand == 1 && time - LastCallTime >= Configuration.MinimumCallInterval)
                {
                    CueLoonCall(location);
                    LastCallTime = time;
                }
            }
        }

        //Select random call from list of audio clips
        private void CueLoonCall(GameLocation loc)
        {
            var rand = rng.Next(0, CueNames.Count);
            if (LastCallIndex.HasValue && rand == LastCallIndex.Value)//Prevent same call from playing twice in a row, iterate index to next cue
            {
                rand = (rand + 1) % CueNames.Count;
            }
            
            string cueName = CueNames[rand];
            LastCallIndex = rand;
            LogMessage($"Cued sound {cueName}");
            Game1.playSound(cueName);
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
                Mod.Monitor.Log($"Building sound cues failed with exception: {ex.Message}", LogLevel.Debug);
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
