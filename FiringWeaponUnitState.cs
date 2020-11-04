using Game.Data.Items;
using System;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.UnitStates
{ 
public class FiringWeaponUnitState : UnitState
{ 
private Weapon weapon;

private Unit target;

private MapCell cellTarget;

private Ammo[] rounds;

private int currentRound = -1;

private float elapsedTime;

private float fireTime;

private bool isReaction;

public FiringWeaponUnitState(Weapon weapon, Unit target, bool isReaction = false)
{ 
this.ismodal = true;
this.weapon = weapon;
this.target = target;
this.cellTarget = target.Cell;
this.isReaction = isReaction;
}

public FiringWeaponUnitState(Weapon weapon, MapCell target)
{ 
this.ismodal = true;
this.weapon = weapon;
this.cellTarget = target;
}

public override void Start()
{ 
if (this.weapon.rateOfFire == 0f)
{ 
Debug.LogError("Error, weapon has no rate of fire: " + this.weapon.name);
this.fireTime = 1f;
}
else
{ 
this.fireTime = 60f / this.weapon.rateOfFire;
}
if (!this.unit.CanFire(this.weapon, this.target))
{ 
this.Pop();
return;
}
this.unit.LookAt(this.cellTarget, true);
this.unit.stats.timeUnits -= (float)((int)(this.weapon.attackCost * this.unit.maxStats.timeUnits));
this.unit.UpdateGui();
this.rounds = new Ammo[this.weapon.roundsPerAttack];
this.FireRound();
}

private void FireRound()
{ 
if (this.currentRound == this.weapon.roundsPerAttack - 1)
{ 
return;
}
this.currentRound++;
Vector3 b = new Vector3(0.5f, (!this.weapon.aimAtGround) ? 0.75f : 0f, 0.5f);
this.rounds[this.currentRound] = this.weapon.ammo.Spawn(this.unit, this.weapon, this.cellTarget.worldposition + b, 0);
this.game.gui.effects.DrawMuzzleFlash(this.unit.Model.transform.position, 0.01f);
b
}

public override void Update()
{ 
if (this.currentRound < this.weapon.roundsPerAttack)
{ 
this.CheckWeaponFire();
}
bool flag = false;
for (int i = 0; i <= this.currentRound; i++)
{ 
if (this.rounds[i].alive)
{ 
this.rounds[i].Update();
flag = true;
}
}
if (!flag && this.currentRound >= this.weapon.roundsPerAttack - 1)
{ 
this.Pop();
this.game.map.isdirty = true;
if (!this.isReaction)
{ 
base.CheckReactionFire();
}
}
flag
i
}

private void CheckWeaponFire()
{ 
this.elapsedTime += Time.fixedDeltaTime;
while (this.elapsedTime > this.fireTime)
{ 
this.elapsedTime -= this.fireTime;
this.FireRound();
}
}
}
}
