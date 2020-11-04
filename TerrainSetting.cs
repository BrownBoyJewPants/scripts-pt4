using System;
using System.Collections.Generic;


namespace Assets.Scripts.Settings.Options
{ 
public class TerrainSetting : SettingsOption
{ 
public override void Apply()
{ 
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
