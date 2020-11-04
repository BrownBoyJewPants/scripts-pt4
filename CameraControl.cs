using Game.Gui;
using System;
using UnityEngine;


public class CameraControl : CameraBase
{ 
private int ScrollTouchID = -1;

private Vector2 scrollTouchOrigin;

private void UpdateTouch()
{ 
if (GameGui.HitGUI(Input.mousePosition))
{ 
this.ScrollTouchID = -1;
return;
}
if (Input.touches.Length != 1)
{ 
return;
}
Touch[] touches = Input.touches;
for (int i = 0; i < touches.Length; i++)
{ 
Touch touch = touches[i];
if (touch.phase == TouchPhase.Began)
{ 
Vector3? targetPosition = this.targetPosition;
if (!targetPosition.HasValue)
{ 
Vector3 offset = base.GetOffset();
offset.y = 0f;
this.targetPosition = new Vector3?(base.transform.position + offset);
}
if (this.ScrollTouchID == -1)
{ 
this.ScrollTouchID = touch.fingerId;
this.scrollTouchOrigin = touch.position;
this.prevTargetPosition = this.targetPosition.Value;
}
}
if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
{ 
this.ScrollTouchID = -1;
}
if (touch.phase == TouchPhase.Moved && touch.fingerId == this.ScrollTouchID)
{ 
Vector3 vector = base.ScreenToGround(this.scrollTouchOrigin, 0f) - base.ScreenToGround(touch.position, 0f);
this.targetPosition = new Vector3?(this.prevTargetPosition + new Vector3(vector.x, 0f, vector.z));
this.scrollTouchOrigin = touch.position;
this.prevTargetPosition = this.targetPosition.Value;
}
}
touches
i
touch
targetPosition
offset
vector
}

private void Update()
{ 
this.UpdateTouch();
float num = Input.GetAxis("Horizontal") * Time.deltaTime * base.transform.position.y;
float num2 = Input.GetAxis("Vertical") * Time.deltaTime * base.transform.position.y;
float num3 = Input.GetAxis("Mouse ScrollWheel") * 10f;
if (num3 != 0f)
{ 
Vector3 position = base.transform.position;
position.y += num3;
base.transform.position = position;
base.UpdateCameraStats();
}
if (num != 0f || num2 != 0f)
{ 
Quaternion rotation = base.transform.rotation;
rotation.x = 0f;
rotation.z = 0f;
Vector3 offset = base.GetOffset();
offset.y = 0f;
Vector3? targetPosition = this.targetPosition;
if (!targetPosition.HasValue)
{ 
this.targetPosition = new Vector3?(base.transform.position + offset);
}
Vector3? targetPosition2 = this.targetPosition;
this.targetPosition = ((!targetPosition2.HasValue) ? null : new Vector3?(targetPosition2.Value + rotation * new Vector3(num, 0f, num2)));
}
Vector3? targetPosition3 = this.targetPosition;
if (targetPosition3.HasValue)
{ 
base.MoveToTarget(true);
}
num
num2
num3
position
rotation
offset
targetPosition
targetPosition2
targetPosition3
}
}
