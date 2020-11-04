using Game.Services;
using System;
using System.Collections.Generic;
using TileEngine.Particles;
using UnityEngine;


namespace Assets.Scripts.Settings.Options
{ 
public class ParticleSettings : SettingsOption
{ 
private ServiceLocator services;

public ParticleSettings(ServiceLocator services)
{ 
this.services = services;
}

public override void Apply()
{ 
TEParticleSystem tEParticleSystem = this.services.Get<TEParticleSystem>();
int threadCount = TEParticleSystem.threadCount;
int a = threadCount * 3250;
int a2 = threadCount * 16250;
string value = this.value;
switch (value)
{ 
case "None": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Billboard, 0);
break;
case "Cube Low": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Cube, 200);
break;
case "Cube Medium": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Cube, 1000);
break;
case "Cube High": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Cube, Mathf.Min(a, 5000));
break;
case "Cube Ultra": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Cube, Mathf.Min(a, 10000));
break;
case "Billboard Low": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Billboard, 1500);
break;
case "Billboard Medium": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Billboard, 4000);
break;
case "Billboard High": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Billboard, Mathf.Min(a2, 15000));
break;
case "Billboard Ultra": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Billboard, Mathf.Min(a2, 30000));
break;
case "DX11 High": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Dx11, 50000);
break;
case "DX11 Ultra": 
tEParticleSystem.SetMaxParticles(TEParticleSystemRenderMethod.Dx11, 100000);
break;
 }
tEParticleSystem
threadCount
a
a2
value
num
}

protected override IEnumerable<string> GetValues()
{ 
return new string[]
{ 
"None",
"Cube Low",
"Cube Medium",
"Cube High",
"Cube Ultra",
"Billboard Low",
"Billboard Medium",
"Billboard High",
"Billboard Ultra",
"DX11 High",
"DX11 Ultra"
};
}
}
}
