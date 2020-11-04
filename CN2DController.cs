using System;
using UnityEngine;


public class CN2DController : MonoBehaviour
{ 
public CNJoystick movementJoystick;

private Transform transformCache;

private void Awake()
{ 
if (this.movementJoystick == null)
{ 
throw new UnassignedReferenceException("Please specify movement Joystick object");
}
this.movementJoystick.FingerTouchedEvent += new FingerTouchedEventHandler(this.StartMoving);
this.movementJoystick.FingerLiftedEvent += new FingerLiftedEventHandler(this.StopMoving);
this.movementJoystick.JoystickMovedEvent += new JoystickMoveEventHandler(this.Move);
this.transformCache = base.transform;
}

protected virtual void Move(Vector3 relativeVector)
{ 
this.transformCache.position = this.transformCache.position + relativeVector;
this.FaceMovementDirection(relativeVector);
}

private void FaceMovementDirection(Vector3 direction)
{ 
if ((double)direction.sqrMagnitude > 0.1)
{ 
base.transform.up = direction;
}
}

protected virtual void StopMoving()
{ 
}

protected virtual void StartMoving()
{ 
}
}
