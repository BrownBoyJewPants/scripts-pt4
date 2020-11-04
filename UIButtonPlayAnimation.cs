using AnimationOrTween;
using System;
using UnityEngine;


[AddComponentMenu("NGUI/Interaction/Button Play Animation")]
public class UIButtonPlayAnimation : MonoBehaviour
{ 
public Animation target;

public string clipName;

public Trigger trigger;

public Direction playDirection = Direction.Forward;

public bool resetOnPlay;

public bool clearSelection;

public EnableCondition ifDisabledOnPlay;

public DisableCondition disableWhenFinished;

public GameObject eventReceiver;

public string callWhenFinished;

public ActiveAnimation.OnFinished onFinished;

private bool mStarted;

private bool mHighlighted;

private void Start()
{ 
this.mStarted = true;
}

private void OnEnable()
{ 
if (this.mStarted && this.mHighlighted)
{ 
this.OnHover(UICamera.IsHighlighted(base.gameObject));
}
}

private void OnHover(bool isOver)
{ 
if (base.enabled)
{ 
if (this.trigger == Trigger.OnHover || (this.trigger == Trigger.OnHoverTrue && isOver) || (this.trigger == Trigger.OnHoverFalse && !isOver))
{ 
this.Play(isOver);
}
this.mHighlighted = isOver;
}
}

private void OnPress(bool isPressed)
{ 
if (base.enabled && (this.trigger == Trigger.OnPress || (this.trigger == Trigger.OnPressTrue && isPressed) || (this.trigger == Trigger.OnPressFalse && !isPressed)))
{ 
this.Play(isPressed);
}
}

private void OnClick()
{ 
if (base.enabled && this.trigger == Trigger.OnClick)
{ 
this.Play(true);
}
}

private void OnDoubleClick()
{ 
if (base.enabled && this.trigger == Trigger.OnDoubleClick)
{ 
this.Play(true);
}
}

private void OnSelect(bool isSelected)
{ 
if (base.enabled && (this.trigger == Trigger.OnSelect || (this.trigger == Trigger.OnSelectTrue && isSelected) || (this.trigger == Trigger.OnSelectFalse && !isSelected)))
{ 
this.Play(true);
}
}

private void OnActivate(bool isActive)
{ 
if (base.enabled && (this.trigger == Trigger.OnActivate || (this.trigger == Trigger.OnActivateTrue && isActive) || (this.trigger == Trigger.OnActivateFalse && !isActive)))
{ 
this.Play(isActive);
}
}

private void Play(bool forward)
{ 
if (this.target == null)
{ 
this.target = base.GetComponentInChildren<Animation>();
}
if (this.target != null)
{ 
if (this.clearSelection && UICamera.selectedObject == base.gameObject)
{ 
UICamera.selectedObject = null;
}
int num = (int)(-(int)this.playDirection);
Direction direction = (Direction)((!forward) ? num : ((int)this.playDirection));
ActiveAnimation activeAnimation = ActiveAnimation.Play(this.target, this.clipName, direction, this.ifDisabledOnPlay, this.disableWhenFinished);
if (activeAnimation == null)
{ 
return;
}
if (this.resetOnPlay)
{ 
activeAnimation.Reset();
}
activeAnimation.onFinished = this.onFinished;
if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
{ 
activeAnimation.eventReceiver = this.eventReceiver;
activeAnimation.callWhenFinished = this.callWhenFinished;
}
else
{ 
activeAnimation.eventReceiver = null;
}
}
num
direction
activeAnimation
}
}
