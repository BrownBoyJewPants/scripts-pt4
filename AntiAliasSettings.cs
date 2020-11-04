using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace Assets.Scripts.Settings.Options
{ 
public class AntiAliasSettings : SettingsOption
{ 
public override void Apply()
{ 
AAMode? aAMode = null;
int antiAliasing = 0;
bool enabled = false;
string value = this.value;
switch (value)
{ 
case "SMAA": 
enabled = true;
break;
case "FXAA2": 
aAMode = new AAMode?(AAMode.FXAA2);
break;
case "FXAA3": 
aAMode = new AAMode?(AAMode.FXAA3Console);
break;
case "NFAA": 
aAMode = new AAMode?(AAMode.NFAA);
break;
case "SSAA": 
aAMode = new AAMode?(AAMode.SSAA);
break;
case "DLAA": 
aAMode = new AAMode?(AAMode.DLAA);
break;
case "MSAAx2": 
antiAliasing = 2;
break;
case "MSAAx4": 
antiAliasing = 4;
break;
case "MSAAx8": 
antiAliasing = 8;
break;
 }
AntialiasingAsPostEffect component = Camera.main.GetComponent<AntialiasingAsPostEffect>();
if (component != null)
{ 
component.enabled = aAMode.HasValue;
if (aAMode.HasValue)
{ 
component.mode = aAMode.Value;
}
}
SMAA component2 = Camera.main.GetComponent<SMAA>();
if (component2 != null)
{ 
component2.enabled = enabled;
}
QualitySettings.antiAliasing = antiAliasing;
aAMode
antiAliasing
enabled
value
num
component
component2
}

[DebuggerHidden]
protected override IEnumerable<string> GetValues()
{ 
AntiAliasSettings.<GetValues>c__Iterator15 <GetValues>c__Iterator = new AntiAliasSettings.<GetValues>c__Iterator15();
AntiAliasSettings.<GetValues>c__Iterator15 expr_07 = <GetValues>c__Iterator;
expr_07.$PC = -2;
return expr_07;
<GetValues>c__Iterator
expr_07
}
}
}
