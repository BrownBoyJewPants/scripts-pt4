using Game.Commands;
using System;
using System.Linq;


namespace Game.GameStates
{ 
public class PlayerTurnGameState : GameState
{ 
private new BattlescapeInfo game;

protected override void Initialize()
{ 
this.game = (BattlescapeInfo)this.game;
base.RegisterHandler<SelectFriendlyUnitCommand>(new Action<SelectFriendlyUnitCommand>(this.FriendlyUnitClicked));
base.RegisterHandler<CellHoverCommand>(new Action<CellHoverCommand>(this.CellHover));
base.RegisterHandler<GameButton>(GameButton.NextFriendlyUnit, new Action(this.game.units.SelectNextFriendly));
base.RegisterHandler<GameButton>(GameButton.PreviousFriendlyUnit, new Action(this.game.units.SelectPreviousFriendly));
base.RegisterHandler<GameButton>(GameButton.SelectFriendlyUnit, new Action<ButtonCommand<GameButton>>(this.SelectFriendly));
base.RegisterHandler<GameButton>(GameButton.MoveUpLevel, new Action(this.MoveUpLevel));
base.RegisterHandler<GameButton>(GameButton.MoveDownLevel, new Action(this.MoveDownLevel));
}

private void MoveDownLevel()
{ 
if (this.game.behaviour.tileMap.VisibleLevel > 1)
{ 
this.game.behaviour.tileMap.VisibleLevel--;
}
this.UpdateVisibleLevel();
}

private void MoveUpLevel()
{ 
if (this.game.behaviour.tileMap.VisibleLevel < 5)
{ 
this.game.behaviour.tileMap.VisibleLevel++;
}
this.UpdateVisibleLevel();
}

private void UpdateVisibleLevel()
{ 
this.game.gui.bindings.UpdateValue("level.current", this.game.behaviour.tileMap.VisibleLevel.ToString());
}

private void SelectFriendly(ButtonCommand<GameButton> cmd)
{ 
Unit unit = this.game.units.Friendlies.FirstOrDefault((Unit u) => u.index == cmd.value);
if (unit != null)
{ 
this.Push(new FriendlyUnitSelectedGameState(unit));
}
<SelectFriendly>c__AnonStorey
unit
}

public override void Start()
{ 
foreach (Unit current in this.game.units.Friendlies)
{ 
current.ResetRoundStats();
}
enumerator
current
}

private void CellHover(CellHoverCommand cmd)
{ 
this.game.gui.effects.DrawSelectedCell(cmd.cell);
}

private void FriendlyUnitClicked(SelectFriendlyUnitCommand cmd)
{ 
this.Push(new FriendlyUnitSelectedGameState(cmd.unit));
}

public override string ToString()
{ 
return "Player turn";
}
}
}
