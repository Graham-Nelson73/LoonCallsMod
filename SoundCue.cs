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
        Mod Mod;
        int LastCallTime;
        List<string> CueNames;
        Random rng;

        const int CALL_START_TIME = 1800;//6:00 pm
        const int CALL_END_TIME = 2400;//12:00 am
        const int CALL_ODDS = 3;//1 in CALL_ODDS chance that call will be cued
        const int MIN_CALL_INTERVAL = 10;//Minimum number of minutes between calls

        public SoundCue(Mod mod)
        {
            Mod = mod;
            rng = new Random();
            LastCallTime = 600;
            CueNames = new List<string>();
            BuildAudioCues();
        }

        public void CheckforLoonCue(int time) {
            var location = Game1.currentLocation;
            if (time >= CALL_START_TIME && time <= CALL_END_TIME &&
                IsPlayerInCueLocation(location) &&
                IsPlayerInCueSeason())
            {
                var rand = rng.Next(1, CALL_ODDS + 1);
                if (rand == 1 && time - LastCallTime > MIN_CALL_INTERVAL)
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
            string cueName = CueNames[rand];
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
            return Game1.CurrentSeasonDisplayName != "Winter";
        }
    }
}
