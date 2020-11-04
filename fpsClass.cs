using System;
using UnityEngine;


[Serializable]
public class fpsClass : MonoBehaviour
{ 
public float updateInterval;

private float accum;

private int frames;

private float timeleft;

public fpsClass()
{ 
this.updateInterval = 0.5f;
}

public override void Start()
{ 
if (!this.GetComponent<GUIText>())
{ 
MonoBehaviour.print("FramesPerSecond needs a GUIText component!");
this.enabled = false;
}
else
{ 
this.timeleft = this.updateInterval;
}
}

public override void Update()
{ 
this.timeleft -= Time.deltaTime;
this.accum += Time.timeScale / Time.deltaTime;
this.frames++;
if (this.timeleft <= (float)0)
{ 
this.GetComponent<GUIText>().text = "fps: " + (this.accum / (float)this.frames).ToString("f2");
this.timeleft = this.updateInterval;
this.accum = (float)0;
this.frames = 0;
}
}

public override void Main()
{ 
}
}
