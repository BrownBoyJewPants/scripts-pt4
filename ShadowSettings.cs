using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Settings.Options
{ 
public class ShadowSettings : SettingsOption
{ 
public bool softshadows;

public override void Apply()
{ 
string text = this.value;
if (this.softshadows)
{ 
text += "Soft";
}
this.SetQualitySetting(text);
QualitySettings.pixelLightCount = 0;
text
}

private void SetQualitySetting(string setting)
{ 
for (int i = 0; i < QualitySettings.names.Length; i++)
{ 
if (QualitySettings.names[i].Equals(setting, StringComparison.InvariantCultureIgnoreCase))
{ 
QualitySettings.SetQualityLevel(i, false);
return;
}
}
Debug.LogError("Can't find quality setting " + setting);
i
}

protected override IEnumerable<string> GetValues()
{ 
return new string[]
{ 
"None",
"Low",
"Medium",
"High"
};
}
}
}
