using Assets.Scripts.Settings;
using Game.Gui;
using System;
using UnityEngine;


namespace Game.States
{ 
public class OptionsMenuState : MenuState
{ 
private GuiBindings bindings;

private Assets.Scripts.Settings.AudioSettings audio;

private VideoSettings video;

public OptionsMenuState(GameObject root) : base(root)
{ 
}

protected override void Initialize()
{ 
base.RegisterHandler<OptionsMenuButton>(OptionsMenuButton.Back, new Action(base.CloseMenu));
this.audio = this.game.Services.Get<Assets.Scripts.Settings.AudioSettings>();
this.video = this.game.Services.Get<VideoSettings>();
this.bindings = this.game.Services.Get<GuiBindings>();
base.Initialize();
}

public override void Start()
{ 
this.bindings.UpdateValue("video.resolution", this.video.resolution, delegate(string v)
{ 
this.video.resolution.SetValue(v);
this.video.resolution.Apply();
this.video.Apply();
});
this.bindings.UpdateValue("video.antialias", this.video.antialias, delegate(string v)
{ 
this.video.antialias.SetValue(v);
this.video.Apply();
});
this.bindings.UpdateValue("video.shadows", this.video.shadows, delegate(string v)
{ 
this.video.shadows.SetValue(v);
this.video.Apply();
});
this.bindings.UpdateValue("video.particles", this.video.particles, delegate(string v)
{ 
this.video.particles.SetValue(v);
this.video.Apply();
});
this.bindings.UpdateValue("video.terrain", this.video.terrain, delegate(string v)
{ 
this.video.terrain.SetValue(v);
this.video.Apply();
});
this.bindings.UpdateValue("video.decoration", this.video.decoration, delegate(string v)
{ 
this.video.decoration.SetValue(v);
this.video.Apply();
});
this.bindings.UpdateValue("video.fullscreen", this.video.resolution.fullscreen, delegate(bool b)
{ 
this.video.resolution.fullscreen = b;
this.video.resolution.Apply();
this.video.Apply();
});
this.bindings.UpdateValue("video.softshadows", this.video.shadows.softshadows, delegate(bool b)
{ 
this.video.shadows.softshadows = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.anisotropic", this.video.anisotropic, delegate(bool b)
{ 
this.video.anisotropic = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.ambientocclusion", this.video.ambientocclusion, delegate(bool b)
{ 
this.video.ambientocclusion = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.depthoffield", this.video.depthoffield, delegate(bool b)
{ 
this.video.depthoffield = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.vsync", this.video.vsync, delegate(bool b)
{ 
this.video.vsync = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.pointlights", this.video.pointlights, delegate(bool b)
{ 
this.video.pointlights = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.lowqualityblood", this.video.lowQualityBlood, delegate(bool b)
{ 
this.video.lowQualityBlood = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.hdr", this.video.hdr, delegate(bool b)
{ 
this.video.hdr = b;
this.video.Apply();
});
this.bindings.UpdateValue("video.shellcasings", this.video.shellCasings, delegate(bool b)
{ 
this.video.shellCasings = b;
this.video.Apply();
});
this.bindings.UpdateValue("audio.master", this.audio.mastervolume, delegate(float v)
{ 
this.audio.mastervolume = v;
this.audio.Apply();
});
this.bindings.UpdateValue("audio.music", this.audio.musicvolume, delegate(float v)
{ 
this.audio.musicvolume = v;
this.audio.Apply();
});
this.bindings.UpdateValue("audio.sound", this.audio.soundvolume, delegate(float v)
{ 
this.audio.soundvolume = v;
this.audio.Apply();
});
this.bindings.UpdateValue("audio.mute", this.audio.mute, delegate(bool b)
{ 
this.audio.mute = b;
this.audio.Apply();
});
this.bindings.UpdateValue("audio.speakermode", this.audio.speakermode, delegate(string v)
{ 
this.audio.speakermode.SetValue(v);
this.audio.speakermode.Apply();
this.audio.Apply();
});
base.Start();
}

public override void End()
{ 
this.video.SaveSettings();
this.audio.SaveSettings();
}
}
}
