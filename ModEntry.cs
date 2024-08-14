using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;

namespace LoonCallsMod
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        private SoundCue SoundCue;

        public override void Entry(IModHelper helper)
        {
            SoundCue = new SoundCue(this);
            helper.Events.GameLoop.TimeChanged += this.OnTimeChange; 
            //TODO: add call cues during menu at end of day
        }

        private void OnTimeChange(object? sender, TimeChangedEventArgs e)
        {
            SoundCue.CheckforLoonCue(e.NewTime);
        }
    }
}
