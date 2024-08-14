using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoonCallsMod
{
    public class Configuration
    {
        public int CallStartTime { get; set; } = 1800;//6:00 pm
        public int CallEndTime { get; set; } = 2400;//12:00 am
        public int CallOdds { get; set; } = 3;//1 in CALL_ODDS chance that call will be cued
        public int MinimumCallInterval { get; set; } = 20;//Minimum number of minutes between calls
        public bool EnableDebugConsole { get; set; } = false;
        public string[] RestrictedSeasons { get; set; } = { "winter" };

        public Configuration() {
           
        }
    }
}
