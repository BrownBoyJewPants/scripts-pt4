using System;
using UnityEngine;


public class CNCameraFollow : MonoBehaviour
{ 
private const float minDistance = 2f;

private const float maxDistance = 10f;

public Transform targetObject;

public CNJoystick joystick;

[Range(1f, 15f)]
public float cameraDistance = 3f;

[Range(1f, 100f)]
public float rotateSpeed = 100f;

[Range(1f, 5f)]
public float distanceSpeed = 1f;

[Range(0f, 360f)]
public float cameraYAngle = 270f;

private void Start()
{ 
if (this.targetObject == null)
{ 
throw new UnassignedReferenceException("Please, specify player target to follow");
}
if (this.joystick != null)
{ 
this.joystick.JoystickMovedEvent += new JoystickMoveEventHandler(this.ChangeAngle);
}
}

private void LateUpdate()
{ 
this.SimpleCamera();
base.transform.LookAt(this.targetObject);
}

private void ChangeAngle(Vector3 relativePosition)
{ 
this.cameraYAngle -= relativePosition.x * this.rotateSpeed * Time.deltaTime;
if ((this.cameraDistance < 2f && relativePosition.y < 0f) || (this.cameraDistance > 10f && relativePosition.y > 0f) || (this.cameraDistance >= 2f && this.cameraDistance <= 10f))
{ 
this.cameraDistance -= relativePosition.y * this.distanceSpeed * Time.deltaTime;
}
}

private void SimpleCamera()
{ 
Vector3 position = this.targetObject.position;
position.x = this.targetObject.position.x + this.cameraDistance * Mathf.Sin(this.cameraYAngle * 0.0174532924f);
position.z = this.targetObject.position.z + this.cameraDistance * Mathf.Cos(this.cameraYAngle * 0.0174532924f);
position.y = this.targetObject.position.y + this.cameraDistance;
base.transform.position = position;
position
}
}
