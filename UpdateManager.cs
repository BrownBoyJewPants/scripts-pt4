using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[AddComponentMenu("NGUI/Internal/Update Manager"), ExecuteInEditMode]
public class UpdateManager : MonoBehaviour
{ 
public class UpdateEntry
{ 
public int index;

public UpdateManager.OnUpdate func;

public MonoBehaviour mb;

public bool isMonoBehaviour;
}

public class DestroyEntry
{ 
public UnityEngine.Object obj;

public float time;
}

public delegate void OnUpdate(float delta);

private static UpdateManager mInst;

private List<UpdateManager.UpdateEntry> mOnUpdate = new List<UpdateManager.UpdateEntry>();

private List<UpdateManager.UpdateEntry> mOnLate = new List<UpdateManager.UpdateEntry>();

private List<UpdateManager.UpdateEntry> mOnCoro = new List<UpdateManager.UpdateEntry>();

private BetterList<UpdateManager.DestroyEntry> mDest = new BetterList<UpdateManager.DestroyEntry>();

private float mTime;

private static int Compare(UpdateManager.UpdateEntry a, UpdateManager.UpdateEntry b)
{ 
if (a.index < b.index)
{ 
return 1;
}
if (a.index > b.index)
{ 
return -1;
}
return 0;
}

private static void CreateInstance()
{ 
if (UpdateManager.mInst == null)
{ 
UpdateManager.mInst = (UnityEngine.Object.FindObjectOfType(typeof(UpdateManager)) as UpdateManager);
if (UpdateManager.mInst == null && Application.isPlaying)
{ 
GameObject gameObject = new GameObject("_UpdateManager");
UnityEngine.Object.DontDestroyOnLoad(gameObject);
UpdateManager.mInst = gameObject.AddComponent<UpdateManager>();
}
}
gameObject
}

private void UpdateList(List<UpdateManager.UpdateEntry> list, float delta)
{ 
int i = list.Count;
while (i > 0)
{ 
UpdateManager.UpdateEntry updateEntry = list[--i];
if (updateEntry.isMonoBehaviour)
{ 
if (updateEntry.mb == null)
{ 
list.RemoveAt(i);
continue;
}
if (!updateEntry.mb.enabled || !NGUITools.GetActive(updateEntry.mb.gameObject))
{ 
continue;
}
}
updateEntry.func(delta);
}
i
updateEntry
}

private void Start()
{ 
if (Application.isPlaying)
{ 
this.mTime = Time.realtimeSinceStartup;
base.StartCoroutine(this.CoroutineFunction());
}
}

private void OnApplicationQuit()
{ 
UnityEngine.Object.DestroyImmediate(base.gameObject);
}

private void Update()
{ 
if (UpdateManager.mInst != this)
{ 
NGUITools.Destroy(base.gameObject);
}
else
{ 
this.UpdateList(this.mOnUpdate, Time.deltaTime);
}
}

private void LateUpdate()
{ 
this.UpdateList(this.mOnLate, Time.deltaTime);
if (!Application.isPlaying)
{ 
this.CoroutineUpdate();
}
}

private bool CoroutineUpdate()
{ 
float realtimeSinceStartup = Time.realtimeSinceStartup;
float num = realtimeSinceStartup - this.mTime;
if (num < 0.001f)
{ 
return true;
}
this.mTime = realtimeSinceStartup;
this.UpdateList(this.mOnCoro, num);
bool isPlaying = Application.isPlaying;
int i = this.mDest.size;
while (i > 0)
{ 
UpdateManager.DestroyEntry destroyEntry = this.mDest.buffer[--i];
if (!isPlaying || destroyEntry.time < this.mTime)
{ 
if (destroyEntry.obj != null)
{ 
NGUITools.Destroy(destroyEntry.obj);
destroyEntry.obj = null;
}
this.mDest.RemoveAt(i);
}
}
if (this.mOnUpdate.Count == 0 && this.mOnLate.Count == 0 && this.mOnCoro.Count == 0 && this.mDest.size == 0)
{ 
NGUITools.Destroy(base.gameObject);
return false;
}
return true;
realtimeSinceStartup
num
isPlaying
i
destroyEntry
}

[DebuggerHidden]
private IEnumerator CoroutineFunction()
{ 
UpdateManager.<CoroutineFunction>c__Iterator4 <CoroutineFunction>c__Iterator = new UpdateManager.<CoroutineFunction>c__Iterator4();
<CoroutineFunction>c__Iterator.<>f__this = this;
return <CoroutineFunction>c__Iterator;
<CoroutineFunction>c__Iterator
}

private void Add(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func, List<UpdateManager.UpdateEntry> list)
{ 
int i = 0;
int count = list.Count;
while (i < count)
{ 
UpdateManager.UpdateEntry updateEntry = list[i];
if (updateEntry.func == func)
{ 
return;
}
i++;
}
list.Add(new UpdateManager.UpdateEntry
{ 
index = updateOrder,
func = func,
mb = mb,
isMonoBehaviour = (mb != null)
});
if (updateOrder != 0)
{ 
list.Sort(new Comparison<UpdateManager.UpdateEntry>(UpdateManager.Compare));
}
i
count
updateEntry
}

public static void AddUpdate(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
{ 
UpdateManager.CreateInstance();
UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnUpdate);
}

public static void AddLateUpdate(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
{ 
UpdateManager.CreateInstance();
UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnLate);
}

public static void AddCoroutine(MonoBehaviour mb, int updateOrder, UpdateManager.OnUpdate func)
{ 
UpdateManager.CreateInstance();
UpdateManager.mInst.Add(mb, updateOrder, func, UpdateManager.mInst.mOnCoro);
}

public static void AddDestroy(UnityEngine.Object obj, float delay)
{ 
if (obj == null)
{ 
return;
}
if (Application.isPlaying)
{ 
if (delay > 0f)
{ 
UpdateManager.CreateInstance();
UpdateManager.DestroyEntry destroyEntry = new UpdateManager.DestroyEntry();
destroyEntry.obj = obj;
destroyEntry.time = Time.realtimeSinceStartup + delay;
UpdateManager.mInst.mDest.Add(destroyEntry);
}
else
{ 
UnityEngine.Object.Destroy(obj);
}
}
else
{ 
UnityEngine.Object.DestroyImmediate(obj);
}
destroyEntry
}
}
