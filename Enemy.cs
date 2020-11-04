using Game.Data;
using Game.Data.Items;
using Game.States;
using Game.Zombiescape.Commands;
using Game.Zombiescape.States.AIStates;
using System;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine.Models;


namespace Game.Zombiescape.Data
{ 
public class Enemy : IUnit
{ 
private ZombiescapeInfo game;

private float fireHeight = 0.7f;

private float life = 10f;

public AIStateMachine state;

public Animator animator;

private MapCell cell;

public float fallSpeed;

public bool IsDisabled
{ 
get;
set;
}

public Weapon MainWeapon
{ 
get;
set;
}

public VoxelModel Model
{ 
get;
private set;
}

public Color32 BloodColor
{ 
get;
set;
}

public bool IsAlive
{ 
get
{ 
return this.life > 0f;
}
}

public float Speed
{ 
get;
set;
}

public Vector3 Velocity
{ 
get
{ 
return Vector3.zero;
}
}

public MapCell Cell
{ 
get
{ 
if (this.cell != null && (this.Model.transform.position - this.cell.worldposition).sqrMagnitude > 0.25f)
{ 
this.cell = null;
}
if (this.cell == null)
{ 
this.cell = this.game.map[(int)this.Model.transform.position.x, 1, (int)this.Model.transform.position.z];
}
return this.cell;
}
}

public bool IsFriendly
{ 
get
{ 
return false;
}
}

public IStateMachine State
{ 
get
{ 
return this.state;
}
}

public Enemy(ZombiescapeInfo game, VoxelModel model)
{ 
this.Speed = 2f;
this.game = game;
this.Model = model;
this.fallSpeed = 1f;
this.BloodColor = new Color32(110, 0, 0, 255);
this.state = new AIStateMachine(game, this);
}

public Ray GetFiringRay(Weapon weapon, ref Vector3 target)
{ 
Ray result = default(Ray);
Vector3 position = weapon.FirePoint.position;
position.y = this.Model.transform.position.y + this.fireHeight;
result.origin = position;
result.direction = target - result.origin;
result.direction = HelperFunctions.CreateRandomDirection(result.direction, 0f, 2f);
return result;
result
position
}

public void TakeDamage(DamageType type, float amount, Vector3 direction)
{ 
if (!this.IsAlive)
{ 
return;
}
this.life -= amount;
if (!this.IsAlive)
{ 
this.game.State.SendCommand(new UnitKilledCommand(this));
this.state.SendCommand(new DieCommand());
}
}

public void Disable()
{ 
this.IsDisabled = true;
}
}
}
