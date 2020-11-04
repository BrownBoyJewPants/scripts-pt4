using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


namespace Game.Gui
{ 
public class AudioGui
{ 
private Dictionary<string, AudioClip> audioclips;

private PooledPrefab sources;

public bool ignoreNew;

public AudioGui()
{ 
this.audioclips = new Dictionary<string, AudioClip>();
this.sources = new PooledPrefab(this.CreatePrefab());
}

public void PlaySound(string name, Vector3 position, float volume = 1f)
{ 
if (this.ignoreNew)
{ 
return;
}
this.PlayClipAt(this.GetSound(name), position, volume);
}

private AudioClip GetSound(string name)
{ 
AudioClip result;
if (!this.audioclips.TryGetValue(name, out result))
{ 
result = (this.audioclips[name] = Resources.Load<AudioClip>("sounds/" + name));
}
return result;
result
}

private GameObject CreatePrefab()
{ 
GameObject gameObject = new GameObject("Audio Source");
gameObject.AddComponent<AudioSource>();
gameObject.AddComponent<DummyBehaviour>();
return gameObject;
gameObject
}

private AudioSource PlayClipAt(AudioClip clip, Vector3 pos, float volume)
{ 
GameObject gameObject = this.sources.Instantiate();
gameObject.transform.position = pos;
AudioSource component = gameObject.GetComponent<AudioSource>();
component.minDistance = 15f;
component.maxDistance = 60f;
component.volume = volume;
component.rolloffMode = AudioRolloffMode.Logarithmic;
component.PlayOneShot(clip);
gameObject.GetComponent<DummyBehaviour>().StartCoroutine(this.DestroyAfter(gameObject, clip.length));
return component;
gameObject
component
}

[DebuggerHidden]
private IEnumerator DestroyAfter(GameObject obj, float time)
{ 
AudioGui.<DestroyAfter>c__Iterator10 <DestroyAfter>c__Iterator = new AudioGui.<DestroyAfter>c__Iterator10();
<DestroyAfter>c__Iterator.time = time;
<DestroyAfter>c__Iterator.obj = obj;
<DestroyAfter>c__Iterator.<$>time = time;
<DestroyAfter>c__Iterator.<$>obj = obj;
<DestroyAfter>c__Iterator.<>f__this = this;
return <DestroyAfter>c__Iterator;
<DestroyAfter>c__Iterator
}
}
}
