using System;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("NGUI/UI/Root"), ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{ 
public enum Scaling
{ 
PixelPerfect,
FixedSize,
FixedSizeOnMobiles
}

private static List<UIRoot> mRoots = new List<UIRoot>();

public UIRoot.Scaling scalingStyle = UIRoot.Scaling.FixedSize;

[HideInInspector]
public bool automatic;

public int manualHeight = 720;

public int minimumHeight = 320;

public int maximumHeight = 1536;

private Transform mTrans;

public static List<UIRoot> list
{ 
get
{ 
return UIRoot.mRoots;
}
}

public int activeHeight
{ 
get
{ 
int num = Mathf.Max(2, Screen.height);
if (this.scalingStyle == UIRoot.Scaling.FixedSize)
{ 
return this.manualHeight;
}
if (num < this.minimumHeight)
{ 
return this.minimumHeight;
}
if (num > this.maximumHeight)
{ 
return this.maximumHeight;
}
return num;
num
}
}

public float pixelSizeAdjustment
{ 
get
{ 
return this.GetPixelSizeAdjustment(Screen.height);
}
}

public static float GetPixelSizeAdjustment(GameObject go)
{ 
UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(go);
return (!(uIRoot != null)) ? 1f : uIRoot.pixelSizeAdjustment;
uIRoot
}

public float GetPixelSizeAdjustment(int height)
{ 
height = Mathf.Max(2, height);
if (this.scalingStyle == UIRoot.Scaling.FixedSize)
{ 
return (float)this.manualHeight / (float)height;
}
if (height < this.minimumHeight)
{ 
return (float)this.minimumHeight / (float)height;
}
if (height > this.maximumHeight)
{ 
return (float)this.maximumHeight / (float)height;
}
return 1f;
}

private void Awake()
{ 
this.mTrans = base.transform;
UIRoot.mRoots.Add(this);
if (this.automatic)
{ 
this.scalingStyle = UIRoot.Scaling.PixelPerfect;
this.automatic = false;
}
}

private void OnDestroy()
{ 
UIRoot.mRoots.Remove(this);
}

private void Start()
{ 
UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
if (componentInChildren != null)
{ 
Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
Camera component = componentInChildren.gameObject.GetComponent<Camera>();
componentInChildren.enabled = false;
if (component != null)
{ 
component.orthographicSize = 1f;
}
}
else
{ 
this.Update();
}
componentInChildren
component
}

private void Update()
{ 
if (this.mTrans != null)
{ 
float num = (float)this.activeHeight;
if (num > 0f)
{ 
float num2 = 2f / num;
Vector3 localScale = this.mTrans.localScale;
if (Mathf.Abs(localScale.x - num2) > 1.401298E-45f || Mathf.Abs(localScale.y - num2) > 1.401298E-45f || Mathf.Abs(localScale.z - num2) > 1.401298E-45f)
{ 
this.mTrans.localScale = new Vector3(num2, num2, num2);
}
}
}
num
num2
localScale
}

public static void Broadcast(string funcName)
{ 
int i = 0;
int count = UIRoot.mRoots.Count;
while (i < count)
{ 
UIRoot uIRoot = UIRoot.mRoots[i];
if (uIRoot != null)
{ 
uIRoot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
}
i++;
}
i
count
uIRoot
}

public static void Broadcast(string funcName, object param)
{ 
if (param == null)
{ 
Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
}
else
{ 
int i = 0;
int count = UIRoot.mRoots.Count;
while (i < count)
{ 
UIRoot uIRoot = UIRoot.mRoots[i];
if (uIRoot != null)
{ 
uIRoot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
}
i++;
}
}
i
count
uIRoot
}
}
