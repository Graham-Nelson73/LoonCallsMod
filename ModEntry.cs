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
            //only host player will track sound queues, will also want to add to menus at end of day
            if (Context.IsMainPlayer) helper.Events.GameLoop.TimeChanged += this.OnTimeChange; 
        }

        private void OnTimeChange(object? sender, TimeChangedEventArgs e)
        {
            SoundCue.CheckforLoonCue(e.NewTime);
        }
    }
}
