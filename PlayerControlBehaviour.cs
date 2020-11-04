using Game.Data;
using Game.Zombiescape.Commands;
using System;
using System.Linq;
using UnityEngine;


namespace Game.Zombiescape.Monobehaviours
{ 
public class PlayerControlBehaviour : MonoBehaviour
{ 
private IGameInfo game;

private bool moving;

public void Start()
{ 
this.game = this.FindGame().Game;
}

private IGameBehaviour FindGame()
{ 
return UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().OfType<IGameBehaviour>().FirstOrDefault<IGameBehaviour>();
}

public void Update()
{ 
float num = 6f * Time.deltaTime;
if (Input.GetKey(KeyCode.LeftShift))
{ 
num *= 1.5f;
}
float axis = Input.GetAxis("Horizontal");
float axis2 = Input.GetAxis("Vertical");
if (axis != 0f || axis2 != 0f)
{ 
Quaternion rotation = Camera.main.transform.rotation;
rotation.x = 0f;
rotation.z = 0f;
Vector3 a = rotation * new Vector3(axis, 0f, axis2);
a.Normalize();
Vector3 vector = a * num;
if (vector != Vector3.zero)
{ 
this.game.State.SendCommand(new PlayerMovementCommand(vector));
}
}
bool key = Input.GetKey(KeyCode.LeftControl);
if (Input.GetMouseButton(0))
{ 
this.game.State.SendCommand(new PlayerInputCommand(InputType.FirePrimary, key));
}
if (Input.GetMouseButton(1))
{ 
this.game.State.SendCommand(new PlayerInputCommand(InputType.FireSecondary, key));
}
num
axis
axis2
rotation
a
vector
key
}
}
}
