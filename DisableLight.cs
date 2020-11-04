using System;
using UnityEngine;


namespace Assets.Scripts.Effects
{ 
public class DisableLight : MonoBehaviour
{ 
public float time = 1f;

private float startTime;

private bool triggered;

private void Start()
{ 
this.triggered = false;
this.startTime = Time.timeSinceLevelLoad;
base.GetComponent<Light>().enabled = true;
}

private void Update()
{ 
if (!this.triggered && Time.timeSinceLevelLoad - this.startTime > this.time)
{ 
this.triggered = true;
base.GetComponent<Light>().enabled = false;
}
}
}
}
