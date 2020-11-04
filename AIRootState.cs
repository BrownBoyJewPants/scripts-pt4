using Game.Data;
using Game.Zombiescape.Commands;
using Game.Zombiescape.Data;
using System;
using TileEngine.Pathfinding;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Zombiescape.States.AIStates
{ 
public class AIRootState : AIState
{ 
private float fireDistance = 6f;

private float turnSpeed = 720f;

private MapCell lastcell;

private Vector3 offset = new Vector3(0.5f, 0f, 0.5f);

private Vector3 fireOffset = new Vector3(0f, 0.5f, 0f);

private Bounds bounds;

private bool hitGround;

private bool moving;

protected override void Initialize()
{ 
base.RegisterHandler<DieCommand>(new Action<DieCommand>(this.Die));
this.lastcell = this.unit.Cell;
this.bounds = new Bounds(this.unit.Model.collider.bounds.center, new Vector3(0.1f, this.unit.Model.collider.bounds.size.y, 0.1f));
base.Initialize();
}

public override void Update()
{ 
if (!this.isactive)
{ 
return;
}
if (!this.game.units.Player.IsAlive || this.unit.IsDisabled)
{ 
this.moving = false;
this.unit.Model.animator.SetBool("Running", false);
return;
}
if (this.unit.Cell == null)
{ 
this.unit.TakeDamage(DamageType.Normal, 1000f, Vector3.down);
return;
}
bool flag = false;
Vector3 vector = this.game.player.Model.transform.position - this.unit.Model.transform.position;
float magnitude = vector.magnitude;
if (magnitude < this.fireDistance)
{ 
vector.Normalize();
Ray ray = new Ray(this.unit.Model.transform.position + this.fireOffset, vector);
float num;
Vector3 vector2;
if (!this.game.map.IntersectRay(ref ray, magnitude, out num, out vector2, false, true))
{ 
this.FireWeapon();
flag = true;
}
}
if (!this.hitGround)
{ 
if (((Enemy)this.unit).fallSpeed == 0f)
{ 
this.hitGround = true;
}
return;
}
MapCell mapCell = null;
Vector3 vector3 = Vector3.zero;
this.unit.Model.animator.SetBool("AimingPistol", flag);
if (!flag)
{ 
PathField pathField = this.game.player.pathField;
mapCell = pathField.GetReverseTargetCell(this.unit.Cell);
}
if (mapCell == null)
{ 
mapCell = this.game.player.Cell;
}
if (mapCell != null)
{ 
Vector3 position = this.unit.Model.transform.position;
position.y = 0f;
Vector3 a = mapCell.worldposition + this.offset;
a.y = 0f;
Vector3 a2 = a - position;
a2.Normalize();
vector3 = a2 * this.unit.Speed * Time.deltaTime;
this.DoMovement(ref vector3);
if (flag)
{ 
this.unit.Model.transform.localRotation = Quaternion.RotateTowards(this.unit.Model.transform.rotation, Quaternion.LookRotation(vector), this.turnSpeed * Time.deltaTime);
}
}
else
{ 
this.StopMoving();
}
flag
vector
magnitude
ray
num
vector2
mapCell
vector3
pathField
position
a
a2
}

private void FireWeapon()
{ 
if (this.unit.MainWeapon == null || this.game.ammo.FireWeapon(this.unit, this.unit.MainWeapon, this.game.units.Player.Model.transform.position + this.fireOffset, true))
{ 
}
}

private void StopMoving()
{ 
if (this.moving)
{ 
this.unit.Model.animator.SetBool("Running", false);
this.moving = false;
}
}

private void DoMovement(ref Vector3 movement)
{ 
this.bounds.center = this.unit.Model.collider.bounds.center;
this.game.map.AdjustMovementToAllowed(ref movement, this.bounds, 0.14f);
this.unit.Model.transform.position += movement;
if (movement.sqrMagnitude > 0f)
{ 
if (!this.moving)
{ 
this.unit.Model.animator.SetBool("HoldingRifle", true);
this.unit.Model.animator.SetBool("Running", true);
this.moving = true;
}
movement.y = 0f;
movement.Normalize();
this.unit.Model.transform.localRotation = Quaternion.RotateTowards(this.unit.Model.transform.localRotation, Quaternion.LookRotation(movement), this.turnSpeed * Time.deltaTime);
if (this.unit.Cell != this.lastcell)
{ 
this.game.units.RemoveUnitAt(this.lastcell, this.unit);
this.game.units.AddUnitAt(this.unit.Cell, this.unit);
this.lastcell = this.unit.Cell;
}
}
else
{ 
this.StopMoving();
}
}

private void Die(DieCommand cmd)
{ 
this.Push(new AIDeadState());
}
}
}
