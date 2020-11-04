using Game.Data.Items;
using System;
using System.Linq;


namespace Game.UnitStates
{ 
public class ReactionFireUnitState : UnitState
{ 
private Unit[] reactingUnits;

private UnitState firingState;

public ReactionFireUnitState(Unit[] reactingUnits)
{ 
this.reactingUnits = reactingUnits;
}

public override void Start()
{ 
Unit[] array = this.reactingUnits;
for (int i = 0; i < array.Length; i++)
{ 
Unit unit = array[i];
unit.State.Push(this.firingState = new FiringWeaponUnitState(unit.inventory.mainWeapon.item as Weapon, this.unit, true));
}
array
i
unit
}

public override void Update()
{ 
if (this.reactingUnits.Any((Unit u) => u.IsAlive && u.State.IsModal))
{ 
return;
}
this.Pop();
}
}
}
