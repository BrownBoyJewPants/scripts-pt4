using System;
using UnityEngine;


[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(CNCameraRelativeSetup))]
public class SkeletonAnimator : MonoBehaviour
{ 
private const string IDLE = "Idle";

private const string WALK = "Walk";

private const string RUN = "Run";

private const string ATTACK = "Attack";

private const string ATTACK_1 = "Attack1";

private const float WALK_SPEED_MULTIPLIER = 0.6f;

private const float RUN_SPEED_MULTIPLIER = 2f;

private CharacterController charController;

private CNCameraRelativeSetup cameraRelativeSetup;

private CNJoystick joystick;

private void Awake()
{ 
this.charController = base.GetComponent<CharacterController>();
this.cameraRelativeSetup = base.GetComponent<CNCameraRelativeSetup>();
this.joystick = this.cameraRelativeSetup.joystick;
this.joystick.JoystickMovedEvent += new JoystickMoveEventHandler(this.AnimateMovement);
this.joystick.FingerLiftedEvent += new FingerLiftedEventHandler(this.StoppedMoving);
}

private void Update()
{ 
Debug.Log(this.charController.velocity);
}

private void AnimateMovement(Vector3 relativeMovement)
{ 
float sqrMagnitude = relativeMovement.sqrMagnitude;
if (sqrMagnitude > 0f)
{ 
if (sqrMagnitude >= 0.3f)
{ 
base.animation["Walk"].speed = this.charController.velocity.magnitude / 2f;
base.animation.CrossFade("Run");
}
else
{ 
base.animation["Walk"].speed = this.charController.velocity.magnitude / 0.6f;
base.animation.CrossFade("Walk");
}
}
sqrMagnitude
}

private void StoppedMoving()
{ 
base.animation.CrossFade("Idle");
}
}
