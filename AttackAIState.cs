using Game.Data.Items;
using Game.UnitStates;
using System;


namespace Game.States
{ 
public class AttackAIState : BattleScapeAIState
{ 
public override void AIUpdate()
{ 
Unit closestVisibleEnemy = base.GetClosestVisibleEnemy();
if (closestVisibleEnemy == null)
{ 
this.Pop();
return;
}
Weapon weapon = this.unit.inventory.mainWeapon.item as Weapon;
if (this.unit.CanFire(weapon, closestVisibleEnemy))
{ 
this.Push(new FiringWeaponUnitState(weapon, closestVisibleEnemy, false));
return;
}
weapon = (this.unit.inventory.secondaryWeapon.item as Weapon);
if (this.unit.CanFire(weapon, closestVisibleEnemy))
{ 
this.Push(new FiringWeaponUnitState((Weapon)this.unit.inventory.secondaryWeapon.item, closestVisibleEnemy, false));
return;
}
closestVisibleEnemy
weapon
}
}
}
