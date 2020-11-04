using System;
using System.Collections.Generic;
using VoxelEngine.TileEngine.Generators;


namespace Assets.Scripts.Settings.Options
{ 
public class DecorationSetting : SettingsOption
{ 
public static float DecorationPercentage;

public override void Apply()
{ 
string value = base.Value;
switch (value)
{ 
case "None": 
GenerationSettings.decoration = 0f;
break;
case "Low": 
GenerationSettings.decoration = 0.2f;
break;
case "Medium": 
GenerationSettings.decoration = 0.4f;
break;
case "High": 
GenerationSettings.decoration = 0.6f;
break;
case "Ultra": 
GenerationSettings.decoration = 1f;
break;
 }
value
num
}

protected override IEnumerable<string> GetValues()
{ 
return new string[]
{ 
"None",
"Low",
"Medium",
"High",
"Ultra"
};
}
}
}
