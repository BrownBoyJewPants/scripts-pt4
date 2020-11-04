using System;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class CNCameraRelativeSetup : MonoBehaviour
{ 
public CNJoystick joystick;

public float runSpeed = 6f;

private CharacterController characterController;

private Camera mainCamera;

private float gravity;

private Vector3 totalMove;

private bool tweakedLastFrame;

private void Awake()
{ 
this.joystick.JoystickMovedEvent += new JoystickMoveEventHandler(this.JoystickMovedEventHandler);
this.joystick.FingerLiftedEvent += new FingerLiftedEventHandler(this.StopMoving);
this.characterController = base.GetComponent<CharacterController>();
this.mainCamera = Camera.main;
this.gravity = -Physics.gravity.y;
this.totalMove = Vector3.zero;
this.tweakedLastFrame = false;
}

private void StopMoving()
{ 
this.totalMove = Vector3.zero;
}

private void Update()
{ 
if (!this.tweakedLastFrame)
{ 
this.totalMove = Vector3.zero;
}
if (!this.characterController.isGrounded)
{ 
this.totalMove.y = (Vector3.down * this.gravity).y;
}
this.characterController.Move(this.totalMove * Time.deltaTime);
this.tweakedLastFrame = false;
}

private void JoystickMovedEventHandler(Vector3 dragVector)
{ 
dragVector.z = dragVector.y;
dragVector.y = 0f;
Vector3 direction = this.mainCamera.transform.TransformDirection(dragVector);
direction.y = 0f;
this.totalMove.x = direction.x * this.runSpeed;
this.totalMove.z = direction.z * this.runSpeed;
this.FaceMovementDirection(direction);
this.tweakedLastFrame = true;
direction
}

private void FaceMovementDirection(Vector3 direction)
{ 
if ((double)direction.sqrMagnitude > 0.1)
{ 
base.transform.forward = direction;
}
}
}
