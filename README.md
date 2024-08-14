# Loon Calls Mod

### Adds ambient loon call sound effects to Stardew Valley that are triggered randomly. 

## Installation Instructions

SMAPI version 4.0 or higher is required to install and run the Loon Calls Mod.

- **[Download Mod Files Here](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2FGraham-Nelson73%2FLoonCallsMod%2Ftree%2Fmain%2FMod&filename=LoonCallsMod)**

- Once downloaded, extract zip files and copy the LoonCallsMod folder into your SMAPI mods folder at *..\Steam\steamapps\common\Stardew Valley\Mods*

- Mod will be loaded when opening the game via SMAPI.

## Configuration

By default, loon calls will be audible during Spring, Summer, and Fall, between 6pm and midnight, when the player is in the Farm, Mountains, or Forest. These settings can be altered in the config.json file:

`"CallStartTime": 1800`<br/>
`"CallEndTime": 2400`<br/>
24h time (represented as an integer) at which calls may begin/end being triggered. Ex. 1800 = 18:00 or 6:00pm

`"CallOdds": 3`<br/>
This defines the likelihood that a call will be played at each time interval (every 10 in-game minutes). Ex. if CallOdds = 3, 
there is a 1/3 chance that a call will be triggered at each 10 minute interval. A higher number means a lower chance to hear a call.

`"MinimumCallInterval": 20`<br/>
This is the minimum amount of time between calls being triggered. By default, a call cannot be triggered more than once every 20 in-game minutes.

`"RestrictedSeasons": [ "winter" ]`<br/>
String array that defines seasons in which calls are not triggered. Formatted as a comma seperated list of season names, all lowercase. 
Ex. a value of [ "winter" , "spring" ] would restrict calls to only Summer and Fall.

`"EnableDebugConsole": false`<br/>
Enables/disables logging to SMAPI console.
