using AnimationOrTween;
using System;
using UnityEngine;


[AddComponentMenu("NGUI/Internal/Active Animation"), RequireComponent(typeof(Animation))]
public class ActiveAnimation : IgnoreTimeScale
{ 
public delegate void OnFinished(ActiveAnimation anim);

public ActiveAnimation.OnFinished onFinished;

public GameObject eventReceiver;

public string callWhenFinished;

private Animation mAnim;

private Direction mLastDirection;

private Direction mDisableDirection;

private bool mNotify;

public bool isPlaying
{ 
get
{ 
if (this.mAnim == null)
{ 
return false;
}
foreach (AnimationState animationState in this.mAnim)
{ 
if (this.mAnim.IsPlaying(animationState.name))
{ 
if (this.mLastDirection == Direction.Forward)
{ 
if (animationState.time < animationState.length)
{ 
bool result = true;
return result;
}
}
else
{ 
if (this.mLastDirection != Direction.Reverse)
{ 
bool result = true;
return result;
}
if (animationState.time > 0f)
{ 
bool result = true;
return result;
}
}
}
}
return false;
enumerator
animationState
result
disposable
}
}

public void Reset()
{ 
if (this.mAnim != null)
{ 
foreach (AnimationState animationState in this.mAnim)
{ 
if (this.mLastDirection == Direction.Reverse)
{ 
animationState.time = animationState.length;
}
else if (this.mLastDirection == Direction.Forward)
{ 
animationState.time = 0f;
}
}
}
enumerator
animationState
disposable
}

private void Update()
{ 
float num = base.UpdateRealTimeDelta();
if (num == 0f)
{ 
return;
}
if (this.mAnim != null)
{ 
bool flag = false;
foreach (AnimationState animationState in this.mAnim)
{ 
if (this.mAnim.IsPlaying(animationState.name))
{ 
float num2 = animationState.speed * num;
animationState.time += num2;
if (num2 < 0f)
{ 
if (animationState.time > 0f)
{ 
flag = true;
}
else
{ 
animationState.time = 0f;
}
}
else if (animationState.time < animationState.length)
{ 
flag = true;
}
else
{ 
animationState.time = animationState.length;
}
}
}
this.mAnim.Sample();
if (flag)
{ 
return;
}
base.enabled = false;
if (this.mNotify)
{ 
this.mNotify = false;
if (this.onFinished != null)
{ 
this.onFinished(this);
}
if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
{ 
this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
}
if (this.mDisableDirection != Direction.Toggle && this.mLastDirection == this.mDisableDirection)
{ 
NGUITools.SetActive(base.gameObject, false);
}
}
}
else
{ 
base.enabled = false;
}
num
flag
enumerator
animationState
num2
disposable
}

private void Play(string clipName, Direction playDirection)
{ 
if (this.mAnim != null)
{ 
base.enabled = true;
this.mAnim.enabled = false;
if (playDirection == Direction.Toggle)
{ 
playDirection = ((this.mLastDirection == Direction.Forward) ? Direction.Reverse : Direction.Forward);
}
bool flag = string.IsNullOrEmpty(clipName);
if (flag)
{ 
if (!this.mAnim.isPlaying)
{ 
this.mAnim.Play();
}
}
else if (!this.mAnim.IsPlaying(clipName))
{ 
this.mAnim.Play(clipName);
}
foreach (AnimationState animationState in this.mAnim)
{ 
if (string.IsNullOrEmpty(clipName) || animationState.name == clipName)
{ 
float num = Mathf.Abs(animationState.speed);
animationState.speed = num * (float)playDirection;
if (playDirection == Direction.Reverse && animationState.time == 0f)
{ 
animationState.time = animationState.length;
}
else if (playDirection == Direction.Forward && animationState.time == animationState.length)
{ 
animationState.time = 0f;
}
}
}
this.mLastDirection = playDirection;
this.mNotify = true;
this.mAnim.Sample();
}
flag
enumerator
animationState
num
disposable
}

public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection, EnableCondition enableBeforePlay, DisableCondition disableCondition)
{ 
if (!NGUITools.GetActive(anim.gameObject))
{ 
if (enableBeforePlay != EnableCondition.EnableThenPlay)
{ 
return null;
}
NGUITools.SetActive(anim.gameObject, true);
UIPanel[] componentsInChildren = anim.gameObject.GetComponentsInChildren<UIPanel>();
int i = 0;
int num = componentsInChildren.Length;
while (i < num)
{ 
componentsInChildren[i].Refresh();
i++;
}
}
ActiveAnimation activeAnimation = anim.GetComponent<ActiveAnimation>();
if (activeAnimation == null)
{ 
activeAnimation = anim.gameObject.AddComponent<ActiveAnimation>();
}
activeAnimation.mAnim = anim;
activeAnimation.mDisableDirection = (Direction)disableCondition;
activeAnimation.eventReceiver = null;
activeAnimation.callWhenFinished = null;
activeAnimation.onFinished = null;
activeAnimation.Play(clipName, playDirection);
return activeAnimation;
componentsInChildren
i
num
activeAnimation
}

public static ActiveAnimation Play(Animation anim, string clipName, Direction playDirection)
{ 
return ActiveAnimation.Play(anim, clipName, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
}

public static ActiveAnimation Play(Animation anim, Direction playDirection)
{ 
return ActiveAnimation.Play(anim, null, playDirection, EnableCondition.DoNothing, DisableCondition.DoNotDisable);
}
}
