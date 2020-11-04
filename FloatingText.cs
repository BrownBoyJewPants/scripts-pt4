using System;
using UnityEngine;


public class FloatingText : MonoBehaviour
{ 
private float alpha;

private UILabel label;

private float starttime;

public float fadestart = 0.5f;

public float duration = 1f;

public float scrollspeed = 1f;

public Vector3 position;

public Color32 color = Color.white;

public PooledPrefab pool;

public string text;

public bool Finished
{ 
get
{ 
return this.alpha < 0f;
}
}

private void OnEnable()
{ 
if (this.text == null)
{ 
base.enabled = false;
return;
}
this.alpha = 1f;
this.label = base.GetComponent<UILabel>();
if (this.label != null)
{ 
this.label.color = this.color;
this.label.enabled = true;
this.label.text = this.text;
}
this.UpdatePosition();
this.starttime = Time.timeSinceLevelLoad;
}

private void UpdatePosition()
{ 
Vector3 vector = Camera.main.WorldToScreenPoint(this.position);
base.transform.localPosition = new Vector3(vector.x, vector.y, 0f);
vector
}

private void Update()
{ 
if (this.label == null || this.alpha <= 0f)
{ 
this.text = null;
this.label.enabled = false;
base.enabled = false;
if (this.pool != null)
{ 
this.pool.Destroy(base.gameObject);
}
return;
}
float num = Time.timeSinceLevelLoad - this.starttime;
if (num > this.fadestart)
{ 
if (this.duration <= this.fadestart)
{ 
this.alpha = 0f;
}
else
{ 
this.alpha = 1f - (num - this.fadestart) / (this.duration - this.fadestart);
}
}
if (this.alpha < 0f)
{ 
this.alpha = 0f;
}
Color32 c = this.color;
c.a = (byte)(this.alpha * 255f);
this.label.color = c;
c = this.label.effectColor;
c.a = (byte)(this.alpha * 255f);
this.label.effectColor = c;
this.UpdatePosition();
base.transform.localPosition += Vector3.up * this.scrollspeed * num;
num
c
}
}
