using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Settings.Options
{ 
public class SpeakerModeSetting : SettingsOption
{ 
public override void Apply()
{ 
string value = this.value;
switch (value)
{ 
case "Mono": 
AudioSettings.speakerMode = AudioSpeakerMode.Mono;
break;
case "Stereo": 
AudioSettings.speakerMode = AudioSpeakerMode.Stereo;
break;
case "Quad": 
AudioSettings.speakerMode = AudioSpeakerMode.Quad;
break;
case "Surround 5.1": 
AudioSettings.speakerMode = AudioSpeakerMode.Mode5point1;
break;
case "Surround 7.1": 
AudioSettings.speakerMode = AudioSpeakerMode.Mode7point1;
break;
case "Prologic DTS": 
AudioSettings.speakerMode = AudioSpeakerMode.Prologic;
break;
 }
value
num
}

protected override IEnumerable<string> GetValues()
{ 
return new string[]
{ 
"Mono",
"Stereo",
"Quad",
"Surround 5.1",
"Surround 7.1",
"Prologic DTS"
};
}
}
}
