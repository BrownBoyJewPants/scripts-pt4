using System;
using System.Runtime.CompilerServices;
using UnityEngine;


public class CNJoystick : MonoBehaviour
{ 
private delegate void InputHandler();

public bool remoteTesting;

public PlacementSnap placementSnap = PlacementSnap.leftBottom;

public Rect tapZone;

private GameObject joystickBase;

private GameObject joystick;

private float joystickBaseRadius;

private float frustumHeight;

private float frustumWidth;

private int myFingerId = -1;

private Vector3 invokeTouchPosition;

private Vector3 joystickRelativePosition;

private Vector3 screenPointInUnits;

private Vector3 relativeExtentSummand;

private bool isTweaking;

private CNJoystick.InputHandler CurrentInputHandler;

private float distanceToCamera = 0.5f;

private float halfScreenHeight;

private float halfScreenWidth;

private float screenHeight;

private float screenWidth;

private Vector3 snapPosition;

private Vector3 cleanPosition;

private Transform joystickBaseTransform;

private Transform joystickTransform;

private Transform transformCache;

public event JoystickMoveEventHandler JoystickMovedEvent
{ 
[MethodImpl(MethodImplOptions.Synchronized)]
add
{ 
this.JoystickMovedEvent = (JoystickMoveEventHandler)Delegate.Combine(this.JoystickMovedEvent, value);
}
[MethodImpl(MethodImplOptions.Synchronized)]
remove
{ 
this.JoystickMovedEvent = (JoystickMoveEventHandler)Delegate.Remove(this.JoystickMovedEvent, value);
}
}

public event FingerLiftedEventHandler FingerLiftedEvent
{ 
[MethodImpl(MethodImplOptions.Synchronized)]
add
{ 
this.FingerLiftedEvent = (FingerLiftedEventHandler)Delegate.Combine(this.FingerLiftedEvent, value);
}
[MethodImpl(MethodImplOptions.Synchronized)]
remove
{ 
this.FingerLiftedEvent = (FingerLiftedEventHandler)Delegate.Remove(this.FingerLiftedEvent, value);
}
}

public event FingerTouchedEventHandler FingerTouchedEvent
{ 
[MethodImpl(MethodImplOptions.Synchronized)]
add
{ 
this.FingerTouchedEvent = (FingerTouchedEventHandler)Delegate.Combine(this.FingerTouchedEvent, value);
}
[MethodImpl(MethodImplOptions.Synchronized)]
remove
{ 
this.FingerTouchedEvent = (FingerTouchedEventHandler)Delegate.Remove(this.FingerTouchedEvent, value);
}
}

public Camera CurrentCamera
{ 
get;
set;
}

private void Awake()
{ 
this.CurrentCamera = base.transform.parent.camera;
this.transformCache = base.transform;
this.joystickBase = this.transformCache.FindChild("Base").gameObject;
this.joystickBaseTransform = this.joystickBase.transform;
this.joystick = base.transform.FindChild("Joystick").gameObject;
this.joystickTransform = this.joystick.transform;
this.InitialCalculations();
this.CurrentInputHandler = new CNJoystick.InputHandler(this.MouseInputHandler);
if (this.remoteTesting)
{ 
this.CurrentInputHandler = new CNJoystick.InputHandler(this.TouchInputHandler);
}
}

private void Update()
{ 
this.CurrentInputHandler();
}

private void TouchInputHandler()
{ 
int touchCount = Input.touchCount;
if (!this.isTweaking)
{ 
for (int i = 0; i < touchCount; i++)
{ 
Touch touch = Input.GetTouch(i);
if (touch.phase == TouchPhase.Began && this.TouchOccured(touch.position))
{ 
this.myFingerId = touch.fingerId;
if (this.FingerTouchedEvent != null)
{ 
this.FingerTouchedEvent();
}
}
}
}
else
{ 
bool flag = false;
for (int j = 0; j < touchCount; j++)
{ 
Touch touch2 = Input.GetTouch(j);
if (this.myFingerId == touch2.fingerId && touch2.phase == TouchPhase.Ended)
{ 
this.ResetJoystickPosition();
flag = true;
if (this.FingerLiftedEvent != null)
{ 
this.FingerLiftedEvent();
}
}
}
if (!flag)
{ 
int num = this.FindMyFingerId();
if (num != -1)
{ 
this.TweakJoystick(Input.GetTouch(num).position);
}
}
}
touchCount
i
touch
flag
j
touch2
num
}

private void MouseInputHandler()
{ 
if (Input.GetMouseButtonDown(0))
{ 
this.TouchOccured(Input.mousePosition);
}
if (Input.GetMouseButton(0) && this.isTweaking)
{ 
this.TweakJoystick(Input.mousePosition);
}
if (Input.GetMouseButtonUp(0))
{ 
this.ResetJoystickPosition();
}
}

private void InitialCalculations()
{ 
this.halfScreenHeight = this.CurrentCamera.orthographicSize;
this.halfScreenWidth = this.halfScreenHeight * this.CurrentCamera.aspect;
this.screenHeight = this.halfScreenHeight * 2f;
this.screenWidth = this.halfScreenWidth * 2f;
this.snapPosition = default(Vector3);
this.cleanPosition = new Vector3(-this.halfScreenWidth, -this.halfScreenHeight);
switch (this.placementSnap)
{ 
case PlacementSnap.leftTop: 
this.snapPosition.x = -this.halfScreenWidth + this.tapZone.width / 2f - this.tapZone.x;
this.snapPosition.y = this.halfScreenHeight - this.tapZone.height / 2f - this.tapZone.y;
this.tapZone.x = 0f;
this.tapZone.y = this.screenHeight - this.tapZone.height;
break;
case PlacementSnap.leftBottom: 
this.snapPosition.x = -this.halfScreenWidth + this.tapZone.width / 2f - this.tapZone.x;
this.snapPosition.y = -this.halfScreenHeight + this.tapZone.height / 2f - this.tapZone.y;
this.tapZone.x = 0f;
this.tapZone.y = 0f;
break;
case PlacementSnap.rightTop: 
this.snapPosition.x = this.halfScreenWidth - this.tapZone.width / 2f - this.tapZone.x;
this.snapPosition.y = this.halfScreenHeight - this.tapZone.height / 2f - this.tapZone.y;
this.tapZone.x = this.screenWidth - this.tapZone.width;
this.tapZone.y = this.screenHeight - this.tapZone.height;
break;
case PlacementSnap.rightBottom: 
this.snapPosition.x = this.halfScreenWidth - this.tapZone.width / 2f - this.tapZone.x;
this.snapPosition.y = -this.halfScreenHeight + this.tapZone.height / 2f - this.tapZone.y;
this.tapZone.x = this.screenWidth - this.tapZone.width;
this.tapZone.y = 0f;
break;
 }
this.transformCache.localPosition = this.snapPosition;
SpriteRenderer spriteRenderer = this.joystickBase.renderer as SpriteRenderer;
this.joystickBaseRadius = spriteRenderer.bounds.extents.x;
spriteRenderer
}

private bool TouchOccured(Vector3 touchPosition)
{ 
this.ScreenPointToRelativeFrustumPoint(touchPosition);
if (this.tapZone.Contains(this.screenPointInUnits))
{ 
this.isTweaking = true;
this.invokeTouchPosition = this.screenPointInUnits;
this.transformCache.localPosition = this.cleanPosition;
this.joystickBaseTransform.localPosition = this.invokeTouchPosition;
this.joystickTransform.localPosition = this.invokeTouchPosition;
return true;
}
return false;
}

private void TweakJoystick(Vector3 desiredPosition)
{ 
this.ScreenPointToRelativeFrustumPoint(desiredPosition);
Vector3 vector = this.screenPointInUnits - this.invokeTouchPosition;
if (vector.sqrMagnitude <= this.joystickBaseRadius * this.joystickBaseRadius)
{ 
this.joystickTransform.localPosition = this.screenPointInUnits;
vector /= this.joystickBaseRadius;
}
else
{ 
this.joystickTransform.localPosition = this.invokeTouchPosition + vector.normalized * this.joystickBaseRadius;
vector.Normalize();
}
if (this.JoystickMovedEvent != null)
{ 
this.JoystickMovedEvent(vector);
}
vector
}

private void ResetJoystickPosition()
{ 
this.isTweaking = false;
this.transformCache.localPosition = this.snapPosition;
this.joystickBaseTransform.localPosition = Vector3.zero;
this.joystickTransform.localPosition = Vector3.zero;
this.myFingerId = -1;
}

private void ScreenPointToRelativeFrustumPoint(Vector3 point)
{ 
float num = point.x / (float)Screen.width;
float num2 = point.y / (float)Screen.height;
this.screenPointInUnits.x = num * this.screenWidth;
this.screenPointInUnits.y = num2 * this.screenHeight;
this.screenPointInUnits.z = 0f;
num
num2
}

private int FindMyFingerId()
{ 
int touchCount = Input.touchCount;
for (int i = 0; i < touchCount; i++)
{ 
if (Input.GetTouch(i).fingerId == this.myFingerId)
{ 
return i;
}
}
return -1;
touchCount
i
}

private void OnDrawGizmos()
{ 
Gizmos.color = Color.red;
Vector3 center = new Vector3(base.transform.position.x + this.tapZone.x, base.transform.position.y + this.tapZone.y, base.transform.position.z);
Gizmos.DrawWireCube(center, new Vector3(this.tapZone.width, this.tapZone.height));
center
}
}
