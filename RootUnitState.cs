using Game.Commands;
using Game.Data.Items;
using System;
using TileEngine.TileMap;


namespace Game.UnitStates
{ 
public class RootUnitState : UnitState
{ 
public RootUnitState()
{ 
base.RegisterHandler<CellClickedCommand>(new Action<CellClickedCommand>(this.CellClicked));
base.RegisterHandler<SelectEnemyUnitCommand>(new Action<SelectEnemyUnitCommand>(this.SelectEnemyUnit));
base.RegisterHandler<LookAtCellCommand>(new Action<LookAtCellCommand>(this.LookAtCell));
base.RegisterHandler<GameButton>(GameButton.FireMainWeapon, delegate
{ 
this.FireWeapon(this.unit.inventory.mainWeapon.item);
});
base.RegisterHandler<GameButton>(GameButton.FireSecondaryWeapon, delegate
{ 
this.FireWeapon(this.unit.inventory.secondaryWeapon.item);
});
base.RegisterHandler<GameButton>(GameButton.Move, new Action(this.DoMove));
base.RegisterHandler<GameButton>(GameButton.FocusOnEnemy, new Action<ButtonCommand<GameButton>>(this.FocusOnEnemy));
}

private void DoMove()
{ 
MapCell mouseOverCell = this.game.behaviour.tileMap.mouseOverCell;
if (mouseOverCell != null)
{ 
this.Push(new MovingUnitState(mouseOverCell, 0));
}
mouseOverCell
}

private void FocusOnEnemy(ButtonCommand<GameButton> cmd)
{ 
if (cmd.value < 1 || cmd.value > this.unit.Visibility.units.Length)
{ 
return;
}
Unit unit = this.unit.Visibility.units[cmd.value - 1];
this.Push(UnitState.EnemySelected(unit));
CameraBase.ScrollTo(unit.Model.transform.position, false);
unit
}

private void LookAtCell(LookAtCellCommand cmd)
{ 
this.unit.LookAt(cmd.cell, false);
}

private void SelectEnemyUnit(SelectEnemyUnitCommand cmd)
{ 
this.Push(UnitState.EnemySelected(cmd.unit));
}

private void CellClicked(CellClickedCommand cmd)
{ 
this.Push(UnitState.CellSelected(cmd.cell));
}

private void FireWeapon(Item item)
{ 
Weapon weapon = item as Weapon;
if (weapon == null)
{ 
return;
}
this.Push(UnitState.FiringWeapon(weapon, this.game.behaviour.tileMap.mouseOverCell));
weapon
}
}
}
