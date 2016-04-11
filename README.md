# Voicepack Combiner
## Introduction
	
Voicepack Combiner is a [mod] for the [Recursion Real Time Stat Tracker] (RTST) which allowes the user to load multiple [voicepacks] at the same time.

![VoicepackCombinerGUI](https://github.com/Frydac/VoicepackCombiner/tree/master/VoicePackCombiner/Resources/VoicepackCombinerGUI.png)

[mod]: <https://recursion.tk/forumdisplay.php?50-Mods>
[voicepacks]: <https://recursion.tk/forumdisplay.php?50-Mods>
[Recursion Real Time Stat Tracker]: <https://recursiontracker.com/>

## Installation

On the main menu screen of the RTST application, press _"Add Mod"_, find the _"Voicepack Combiner"_ mod, and press _"Install"_. 
If the modmanager is not showing, 
move your mouse to the left side of the window and click in the highlighted region with the arrow.

## Usage
When the Voicepack Combiner mod is loaded, a few menu items are added to the main window under _"Mods"_.  

  
Click Mods->VoicePackCombiner->Settings to open the VoicePackCombiner window, here you can add voicepacks to the list.
Click the "Add VoicePacks" button, and select one or more voicepacks. Or drag and drop voicepacks from windows explorer onto the VoicePackCombiner window.  

The combined voicepack can be loaded/unloaded by clicking the _"Use Combined VoicePack"_ checkbox in the Voicepack Combiner window, 
or by clicking the menu item under _Mods_->_Voicepack Combiner_->_Use Combined VoicePack_. Unloading means that the voicepack that was previously loaded
in the RTST application is restored.

All the voicepacks added to the list will be combined to one voicepack. 
For each achievement, all the sounds for that achievement from each voicepack will be grouped. 
When an achievement is triggered, the RTST application will randomly play one of the grouped sounds for that achievment.

When the combined voicepack is in use, it can be edited like a normal voicepack using the voicepack editor menu of the RTST application (Tools->Options->Audio->Configure). 
Note that when you remove a voicepack from the list, the combined voicepack will be recreated from the source voicepack files, and any changes will be overwritten.

The combined voicepack can be exported/published as a new single voicepack by clicking the "Export Combined VoicePack" button on the VoicePackCombiner window.

Have fun!


## Building
The VoicePackCombiner project depends on RTPluginPS2.dll and RTLibrary.dll, both part of the Recursion RTST project.  
The VoicePackCombinerTest project depends in addition on protobuf-net.  
These assemblies can be found in the Recursion RTST install folder. (Currently only the ones from the beta version will work)


