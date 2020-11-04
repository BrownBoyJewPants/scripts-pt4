using System;
using UnityEngine;


namespace Game
{ 
[Serializable]
public class AudioGameSettings
{ 
public float mastervolume;

public float musicvolume;

public float soundvolume;

public AudioSpeakerMode speakermode;

public bool disableaudio;

public AudioGameSettings()
{ 
this.mastervolume = 100f;
this.musicvolume = 100f;
this.soundvolume = 100f;
this.speakermode = AudioSpeakerMode.Mode5point1;
this.disableaudio = false;
}
}
}
