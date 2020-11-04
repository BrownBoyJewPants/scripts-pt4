using System;
using UnityEngine;


public class IncreasePitchBehaviour : MonoBehaviour
{ 
private AudioSource clip;

private float orgPitch;

private void Start()
{ 
this.clip = base.GetComponent<AudioSource>();
this.orgPitch = this.clip.pitch;
}

private void OnEnable()
{ 
if (this.clip != null)
{ 
this.clip.pitch = this.orgPitch;
}
}

private void Update()
{ 
if (this.clip == null)
{ 
return;
}
this.clip.pitch = Mathf.Min(4f, this.clip.pitch + 0.3f * Time.deltaTime);
if (this.clip.pitch >= 3f)
{ 
this.clip.enabled = false;
}
}
}
