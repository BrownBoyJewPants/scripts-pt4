using System;
using UnityEngine;


namespace Assets.Scripts
{ 
public class MusicPlayer : MonoBehaviour
{ 
public AudioClip Song;

private AudioSource source;

private float volume = 1f;

private void Start()
{ 
this.source = base.gameObject.AddComponent<AudioSource>();
this.source.clip = this.Song;
this.source.volume = this.volume;
this.source.ignoreListenerVolume = true;
this.source.Play();
}

private void Update()
{ 
if (!this.source.isPlaying)
{ 
this.source.Play();
}
}

internal void SetVolume(float mv)
{ 
this.volume = mv;
if (this.source != null)
{ 
this.source.volume = mv;
}
}
}
}
