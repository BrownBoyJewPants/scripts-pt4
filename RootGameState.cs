using Game.Commands;
using System;
using System.Linq;


namespace Game.GameStates
{ 
public class RootGameState : GameState
{ 
private int turncount;

private bool isfriendlyturn;

private Unit selectedUnit;

protected override void Initialize()
{ 
base.RegisterHandler<GameButton>(GameButton.EndTurn, new Action(this.EndTurn));
}

private void EndTurn()
{ 
if (!this.isfriendlyturn)
{ 
return;
}
this.SavePlayerState();
base.MakeActiveState();
}

public override void Update()
{ 
if (!this.isactive)
{ 
return;
}
this.isfriendlyturn = !this.isfriendlyturn;
if (this.isfriendlyturn)
{ 
this.turncount++;
this.Push(new PlayerTurnGameState());
this.LoadPlayerState();
}
else
{ 
this.Push(new EnemyTurnGameState());
}
}

private void LoadPlayerState()
{ 
if (this.selectedUnit != null)
{ 
this.game.State.SendCommand(new SelectFriendlyUnitCommand(this.selectedUnit));
}
if (this.turncount == 1)
{ 
this.game.State.SendButton<GameButton>(GameButton.NextFriendlyUnit, 0);
this.game.State.SendButton<GameButton>(GameButton.CenterOnFriendlyUnit, 0);
}
}

private void SavePlayerState()
{ 
this.selectedUnit = this.game.Services.Get<BattlescapeUnitManager>().Friendlies.FirstOrDefault((Unit f) => f.selected);
}

public override string ToString()
{ 
return "Root";
}
}
}
