using Game.Zombiescape;
using Game.Zombiescape.Commands;
using System;
using System.Linq;
using UnityEngine;


public class TouchScreenControl : MonoBehaviour
{ 
private ZombiescapeInfo game;

public CNJoystick left;

public CNJoystick right;

public CNJoystick rightTop;

private void Start()
{ 
IGameBehaviour gameBehaviour = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().FirstOrDefault((MonoBehaviour b) => b is IGameBehaviour) as IGameBehaviour;
if (gameBehaviour != null)
{ 
this.game = (ZombiescapeInfo)gameBehaviour.Game;
}
this.left.JoystickMovedEvent += new JoystickMoveEventHandler(this.left_JoystickMovedEvent);
this.right.JoystickMovedEvent += new JoystickMoveEventHandler(this.right_JoystickMovedEvent);
this.rightTop.JoystickMovedEvent += new JoystickMoveEventHandler(this.rightTop_JoystickMovedEvent);
gameBehaviour
}

private void rightTop_JoystickMovedEvent(Vector3 relativeVector)
{ 
Vector3 worldDirection = this.GetWorldDirection(ref relativeVector);
if (worldDirection == Vector3.zero)
{ 
return;
}
float d = 8f * (1f - worldDirection.y * 0.5f);
Vector3 b = worldDirection * d;
this.game.State.SendCommand(new PlayerFireWeaponCommand(this.game.player.secondaryWeapon, this.game.player.Model.transform.position + b));
worldDirection
d
b
}

private void right_JoystickMovedEvent(Vector3 relativeVector)
{ 
Vector3 position = this.game.player.primaryWeapon.FirePoint.position;
position.y = this.game.player.Model.transform.position.y + 0.7f;
this.game.State.SendCommand(new PlayerFireWeaponCommand(this.game.player.primaryWeapon, position + this.GetWorldDirection(ref relativeVector) * 4f));
position
}

private void left_JoystickMovedEvent(Vector3 relativeVector)
{ 
Vector3 delta = this.GetWorldDirection(ref relativeVector) * 5f * Time.deltaTime;
this.game.State.SendCommand(new PlayerMovementCommand(delta));
delta
}

private Vector3 GetWorldDirection(ref Vector3 relativeVector)
{ 
Quaternion rotation = Camera.main.transform.rotation;
rotation.x = 0f;
rotation.z = 0f;
Vector3 result = rotation * new Vector3(relativeVector.x, 0f, relativeVector.y);
result.Normalize();
return result;
rotation
result
}
}
