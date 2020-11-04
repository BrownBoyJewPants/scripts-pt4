using Game.Data;
using Game.Data.Items;
using Game.States;
using Game.Zombiescape.Commands;
using Game.Zombiescape.States.PlayerStates;
using System;
using TileEngine.Pathfinding;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine.Models;


namespace Game.Zombiescape
{ 
public class Player : IUnit
{ 
public const float fireHeight = 0.7f;

public IGameInfo game;

private VoxelModel model;

public Weapon primaryWeapon;

public Weapon secondaryWeapon;

public PathField pathField;

private float lastPrimaryShot;

private float lastSecondaryShot;

public float life;

public float maxlife = 100f;

private Plane invisiblePlane;

IStateMachine IUnit.State
{ 
get
{ 
return this.State;
}
}

Weapon IUnit.MainWeapon
{ 
get
{ 
return this.primaryWeapon;
}
}

public Vector3 Velocity
{ 
get;
set;
}

public VoxelModel Model
{ 
get
{ 
return this.model;
}
}

public bool IsAlive
{ 
get
{ 
return this.life > 0f;
}
}

public Color32 BloodColor
{ 
get;
private set;
}

public float Speed
{ 
get;
set;
}

public bool IsDisabled
{ 
get;
set;
}

public PlayerStateMachine State
{ 
get;
private set;
}

public Rigidbody Rigid
{ 
get
{ 
return this.model.rigidBody;
}
}

public MapCell Cell
{ 
get
{ 
return this.game.Map[(int)this.Model.transform.position.x, 1, (int)this.Model.transform.position.z];
}
}

public bool IsFriendly
{ 
get
{ 
return true;
}
}

public Player(IGameInfo game, VoxelModel model)
{ 
this.life = this.maxlife;
this.model = model;
this.game = game;
this.State = new PlayerStateMachine(this);
this.invisiblePlane = new Plane(Vector3.down, 3.49f);
this.BloodColor = new Color32(128, 0, 0, 1);
}

void IUnit.TakeDamage(DamageType type, float damage, Vector3 direction)
{ 
this.life -= damage;
this.State.SendCommand(new TookDamageCommand(damage));
}

public void Disable()
{ 
}

public Vector3 GetTarget(ref bool fireAtGround)
{ 
if (fireAtGround)
{ 
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
float num;
if (this.invisiblePlane.Raycast(ray, out num))
{ 
ray.origin = ray.GetPoint(num);
}
float num2 = -1f;
Vector3 vector;
if (this.game.Map.IntersectRay(ref ray, 100f, out num, out vector, false, true))
{ 
num2 = num;
}
RaycastHit raycastHit;
if (Physics.Raycast(ray, out raycastHit, 100f) && raycastHit.rigidbody != this.Rigid && (num2 != -1f || num2 > raycastHit.distance))
{ 
return raycastHit.point;
}
if (num2 != -1f)
{ 
return ray.GetPoint(num2);
}
}
else
{ 
Ray ray2 = Camera.main.ScreenPointToRay(Input.mousePosition);
RaycastHit raycastHit2;
if (Physics.Raycast(ray2, out raycastHit2, 100f) && raycastHit2.rigidbody != this.Rigid)
{ 
fireAtGround = true;
return raycastHit2.point;
}
}
return CameraBase.ScreenToGround(Camera.main, Input.mousePosition, this.Model.transform.position.y + 0.7f);
ray
num
num2
vector
raycastHit
ray2
raycastHit2
}

public Ray GetFiringRay(Weapon weapon, ref Vector3 target)
{ 
Ray result = default(Ray);
if (weapon.FirePoint != null)
{ 
result.origin = weapon.FirePoint.position;
}
else
{ 
result.origin = this.Model.transform.position + new Vector3(0f, 0.7f, 0f);
}
Vector3 origin = result.origin;
origin.y = this.Model.transform.position.y + 0.7f;
result.origin = origin;
if (weapon.firingAtGround)
{ 
result.direction = target - result.origin;
}
else
{ 
result.direction = this.Model.transform.forward;
}
result.direction = HelperFunctions.CreateRandomDirection(result.direction, 0f, 2f + (1f - weapon.accuracy) * 30f);
return result;
result
origin
}
}
}
