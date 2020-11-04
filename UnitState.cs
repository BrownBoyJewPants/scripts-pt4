using Game.Data.Items;
using Game.States;
using System;
using System.Collections.Generic;
using TileEngine.TileMap;


namespace Game.UnitStates
{ 
public abstract class UnitState : State
{ 
public Unit unit;

public BattlescapeInfo game;

public static UnitState Root()
{ 
return new RootUnitState();
}

public static UnitState Moving(MapCell cell)
{ 
return new MovingUnitState(cell, 0);
}

public static UnitState CellSelected(MapCell cell)
{ 
return new CellSelectedUnitState(cell);
}

public static UnitState EnemySelected(Unit enemy)
{ 
return new EnemySelectedUnitState(enemy);
}

public static UnitState FiringWeapon(Weapon weapon, Unit target)
{ 
return new FiringWeaponUnitState(weapon, target, false);
}

public static UnitState FiringWeapon(Weapon weapon, MapCell target)
{ 
return new FiringWeaponUnitState(weapon, target);
}

public override string ToString()
{ 
return base.GetType().Name.Replace("UnitState", string.Empty);
}

public bool CheckReactionFire()
{ 
bool flag = false;
BattlescapeUnitList battlescapeUnitList = (!this.unit.isFriendly) ? this.game.units.Friendlies : this.game.units.Enemies;
for (int i = 0; i < battlescapeUnitList.Count; i++)
{ 
flag |= this.CheckReactionFire(battlescapeUnitList[i]);
}
return flag;
flag
battlescapeUnitList
i
}

public bool CheckReactionFire(Unit opposition)
{ 
float initiative = this.unit.GetInitiative();
List<Unit> list = new List<Unit>();
if (opposition.CanSee(this.unit) && opposition.GetInitiative() > initiative)
{ 
Weapon weapon = opposition.inventory.mainWeapon.item as Weapon;
if (opposition.CanFire(weapon, this.unit))
{ 
list.Add(opposition);
}
}
if (list.Count > 0)
{ 
this.Push(new ReactionFireUnitState(list.ToArray()));
return true;
}
return false;
initiative
list
weapon
}
}
}
