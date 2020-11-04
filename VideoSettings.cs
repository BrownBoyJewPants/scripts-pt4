using Assets.Scripts.Settings.Options;
using Game.Services;
using System;
using UnityEngine;
using VoxelEngine.TileEngine.Generators;


namespace Assets.Scripts.Settings
{ 
public class VideoSettings : SettingsBase
{ 
public readonly ResolutionSetting resolution;

public readonly SettingsOption antialias;

public readonly ShadowSettings shadows;

public readonly SettingsOption particles;

public readonly SettingsOption terrain;

public readonly SettingsOption decoration;

public bool ambientocclusion;

public bool depthoffield;

public bool vsync;

public bool anisotropic;

public bool pointlights;

public bool lowQualityBlood;

public bool hdr;

public bool shellCasings;

public static bool Dirty
{ 
get;
set;
}

public VideoSettings(ServiceLocator services)
{ 
this.resolution = new ResolutionSetting();
this.antialias = new AntiAliasSettings();
this.shadows = new ShadowSettings();
this.particles = new ParticleSettings(services);
this.terrain = new TerrainSetting();
this.decoration = new DecorationSetting();
this.antialias.SetValue("SMAA");
this.shadows.SetValue("High");
this.particles.SetValue("Billboard Ultra");
this.terrain.SetValue("Ultra");
this.decoration.SetValue("Ultra");
this.resolution.SetValue(ResolutionSetting.ResolutionName(Screen.width, Screen.height));
this.resolution.fullscreen = false;
this.shadows.softshadows = true;
this.ambientocclusion = true;
this.depthoffield = true;
this.vsync = true;
this.anisotropic = true;
this.pointlights = true;
this.lowQualityBlood = true;
this.hdr = true;
this.shellCasings = true;
base.LoadSettings();
}

public override void Apply()
{ 
this.shadows.Apply();
QualitySettings.vSyncCount = ((!this.vsync) ? 0 : 1);
QualitySettings.anisotropicFiltering = ((!this.anisotropic) ? AnisotropicFiltering.Disable : AnisotropicFiltering.Enable);
SSAOEffect component = Camera.main.GetComponent<SSAOEffect>();
if (component != null)
{ 
component.enabled = this.ambientocclusion;
}
DepthOfFieldScatter component2 = Camera.main.GetComponent<DepthOfFieldScatter>();
if (component2 != null)
{ 
component2.enabled = this.depthoffield;
}
GenerationSettings.LowQualitySplatter = this.lowQualityBlood;
GenerationSettings.shellCasings = this.shellCasings;
Camera.main.hdr = this.hdr;
this.antialias.Apply();
this.particles.Apply();
this.terrain.Apply();
this.decoration.Apply();
VideoSettings.Dirty = true;
component
component2
}
}
}
