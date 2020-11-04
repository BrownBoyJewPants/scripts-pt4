using Game.Commands;
using Game.Data.Items;
using System;
using TileEngine.TileMap;


namespace Game.UnitStates
{ 
public class CellSelectedUnitState : UnitState
{ 
private MapCell selectedCell;

public CellSelectedUnitState(MapCell cell)
{ 
this.selectedCell = cell;
base.RegisterHandler<CellClickedCommand>(new Action<CellClickedCommand>(this.CellClicked));
base.RegisterHandler<GameButton>(GameButton.Move, new Action(this.DoMove));
}

private void DoMove()
{ 
this.Push(new MovingUnitState(this.selectedCell, 0));
}

public override void Update()
{ 
if (this.unit.selected)
{ 
this.game.gui.pathfield.Draw(this.unit, this.selectedCell);
}
}

private void CellClicked(CellClickedCommand cmd)
{ 
if (cmd.cell == this.selectedCell)
{ 
this.Push(UnitState.Moving(this.selectedCell));
}
else
{ 
this.selectedCell = cmd.cell;
}
}

public override void EnterForeground()
{ 
if (this.selectedCell == this.unit.Cell)
{ 
this.selectedCell = null;
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
this.Push(UnitState.FiringWeapon(weapon, this.selectedCell));
weapon
}
}
}
