# VoicePackCombiner
## Introduction
VoicePackCombiner is a [mod] for the [Recursion Real Time Stat Tracker] which allowes the user to load multiple [voicepacks] at the same time.

[mod]: <http://link_to_mod_forum_post>
[voicepacks]: <https://recursion.tk/forumdisplay.php?50-Mods>
[Recursion Real Time Stat Tracker]: <https://recursiontracker.com/>

## Installation

On the main menu screen, press "Add Mod", find the "VoicePackCombiner" mod, and press "Install". 
If the modmanager is not showing, 
move your mouse to the left side of the window and click in the highlighted region with the arrow.

## Usage
When the VoicePackCombiner mod is loaded, a few menu items are added to the main window under "Mods".  

  
Click Mods->VoicePackCombiner->Settings to open the VoicePackCombiner window, here you can add voicepacks to the list.
Click the "Add VoicePacks" button, and select one or more voicepacks. Or drag and drop voicepacks from windows explorer onto the VoicePackCombiner window.  

The combined voicepack can be loaded/unloaded by clicking the "Use Combined VoicePack" checkbox in the VoicePackCombiner window, 
or by clicking the corresponding menu item under Mods->VoicePackCombiner->Use Combined VoicePack.

All the voicepacks in the list will be combined to one voicepack. 
For each achievement, all the sounds for that achievement from each voicepack will be grouped. 
When an achievement is triggered, Recursion RTST will randomly play one of the grouped sounds for that achievment.

When the combined voicepack is in use, it can be edited like a normal voicepack via the voicepack editor menu of the Recursion RTST client (Tools->Options->Audio->Configure). 
Note that when you add or remove a voicepack, the combined voicepack will be recombined and any changes will be overwritten.
If you which to alter anything, do that right before you publish/export the voicepack. Then newly exported voicepack can then again be used
to combine with more voicepacks.

The combined voicepack can be exported as a new single voicepack by clicking the "Export Combined VoicePack" button on the VoicePackCombiner window.

Have fun!


## Building
The VoicePackCombiner project depends on RTPluginPS2.dll and RTLibrary.dll, both part of the Recursion RTST project.  
The VoicePackCombinerTest project depends in addition on protobuf-net.  
These assemblies can be found in the Recursion RTST install folder. (Currently only the ones from the beta version will work)


