using Game.Commands;
using Game.Data.Items;
using System;
using UnityEngine;


namespace Game.UnitStates
{ 
public class EnemySelectedUnitState : UnitState
{ 
private Unit selectedEnemy;

private bool done;

private int calculated;

private int hitMain;

private int hitOff;

public EnemySelectedUnitState(Unit enemy)
{ 
base.RegisterHandler<SelectEnemyUnitCommand>(new Action<SelectEnemyUnitCommand>(this.SelectEnemyUnit));
base.RegisterHandler<UnitKilledCommand>(new Action<UnitKilledCommand>(this.UnitKilled));
base.RegisterHandler<GameButton>(GameButton.FireMainWeapon, delegate
{ 
this.FireWeapon(this.unit.inventory.mainWeapon.item);
});
base.RegisterHandler<GameButton>(GameButton.FireSecondaryWeapon, delegate
{ 
this.FireWeapon(this.unit.inventory.secondaryWeapon.item);
});
this.selectedEnemy = enemy;
}

private void UnitKilled(UnitKilledCommand cmd)
{ 
if (cmd.unit == this.selectedEnemy)
{ 
this.Pop();
}
}

private void FireWeapon(Item item)
{ 
Weapon weapon = item as Weapon;
if (weapon == null)
{ 
return;
}
this.Push(UnitState.FiringWeapon(weapon, this.selectedEnemy));
weapon
}

private void SelectEnemyUnit(SelectEnemyUnitCommand cmd)
{ 
if (cmd.unit != this.selectedEnemy)
{ 
this.Set(UnitState.EnemySelected(cmd.unit));
}
}

public override void EnterForeground()
{ 
this.hitOff = 0;
this.hitMain = 0;
this.calculated = 0;
}

public override void EnterBackground()
{ 
this.game.gui.bindings.UpdateValue("friendly.mainweapon.hitchance", "---");
this.game.gui.bindings.UpdateValue("friendly.secondaryweapon.hitchance", "---");
}

public override void Update()
{ 
if (!this.selectedEnemy.IsAlive)
{ 
this.Pop();
return;
}
if (this.calculated < 1000)
{ 
Weapon weapon = (Weapon)this.unit.inventory.mainWeapon.item;
Vector3 a = this.selectedEnemy.Cell.worldposition + new Vector3(0.5f, 0.75f, 0.5f);
float num = (a - this.unit.Model.transform.position).magnitude + 3f;
for (int i = 0; i < 10; i++)
{ 
float num2 = num;
Ray firingRay = this.unit.GetFiringRay(weapon, ref a);
float num3;
Vector3 vector;
if (this.game.map.IntersectRay(ref firingRay, num2, out num3, out vector, false, true))
{ 
num2 = num3;
}
RaycastHit raycastHit;
if (num2 > 0f && Physics.Raycast(firingRay, out raycastHit, num2))
{ 
UnitBehaviour component = raycastHit.collider.gameObject.GetComponent<UnitBehaviour>();
if (component != null && component.unit == this.selectedEnemy)
{ 
this.hitMain++;
}
}
this.calculated++;
}
weapon = (Weapon)this.unit.inventory.secondaryWeapon.item;
for (int j = 0; j < 10; j++)
{ 
float num4 = num;
Ray firingRay2 = this.unit.GetFiringRay(weapon, ref a);
float num3;
Vector3 vector;
if (this.game.map.IntersectRay(ref firingRay2, num4, out num3, out vector, false, true))
{ 
num4 = num3;
}
RaycastHit raycastHit;
if (num4 > 0f && Physics.Raycast(firingRay2, out raycastHit, num4))
{ 
UnitBehaviour component2 = raycastHit.collider.gameObject.GetComponent<UnitBehaviour>();
if (component2 != null && component2.unit == this.selectedEnemy)
{ 
this.hitOff++;
}
}
}
this.game.gui.bindings.UpdateValue("friendly.mainweapon.hitchance", string.Format("{0:0.0}%", 100f * (float)this.hitMain / (float)this.calculated));
this.game.gui.bindings.UpdateValue("friendly.secondaryweapon.hitchance", string.Format("{0:0.0}%", 100f * (float)this.hitOff / (float)this.calculated));
}
if (this.unit.selected)
{ 
this.game.gui.enemyGui.Draw(this.selectedEnemy);
}
base.Update();
weapon
a
num
i
num2
firingRay
num3
vector
raycastHit
component
j
num4
firingRay2
component2
}
}
}
