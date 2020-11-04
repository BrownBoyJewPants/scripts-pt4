using Assets.Scripts.Settings.Options;
using System;
using UnityEngine;


namespace Assets.Scripts.Settings
{ 
public class AudioSettings : SettingsBase
{ 
public SettingsOption speakermode;

public float mastervolume;

public float musicvolume;

public float soundvolume;

public bool mute;

public AudioSettings()
{ 
this.mastervolume = 0.5f;
this.musicvolume = 1f;
this.soundvolume = 1f;
this.mute = false;
this.speakermode = new SpeakerModeSetting();
this.speakermode.SetValue("Surround 5.1");
base.LoadSettings();
}

public override void Apply()
{ 
float num = this.mastervolume;
if (this.mute)
{ 
num = 0f;
}
float volume = this.soundvolume * num;
float volume2 = this.musicvolume * num;
AudioListener.volume = volume;
MusicPlayer component = Camera.main.GetComponent<MusicPlayer>();
if (component != null)
{ 
component.SetVolume(volume2);
}
num
volume
volume2
component
}
}
}
