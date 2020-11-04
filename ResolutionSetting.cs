using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Assets.Scripts.Settings.Options
{ 
public class ResolutionSetting : SettingsOption
{ 
public bool fullscreen;

private Dictionary<string, Resolution> resolutions;

public override void Apply()
{ 
if (this.value == null)
{ 
return;
}
Resolution currentResolution = Screen.currentResolution;
if (this.resolutions.TryGetValue(this.value, out currentResolution))
{ 
Screen.SetResolution(currentResolution.width, currentResolution.height, this.fullscreen);
}
currentResolution
}

protected override IEnumerable<string> GetValues()
{ 
this.resolutions = Screen.resolutions.ToDictionary((Resolution r) => ResolutionSetting.ResolutionName(r));
return from k in this.resolutions.Keys
orderby k
select k;
}

public static string ResolutionName(int width, int height)
{ 
return width + " x " + height;
}

public static string ResolutionName(Resolution resolution)
{ 
return ResolutionSetting.ResolutionName(resolution.width, resolution.height);
}
}
}
