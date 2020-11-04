using Assets.Scripts.Game.States.MenuStates;
using Game.Commands;
using System;


namespace Game.GameStates
{ 
public class FriendlyUnitSelectedGameState : GameState
{ 
private new BattlescapeInfo game;

private Unit unit;

public FriendlyUnitSelectedGameState(Unit unit)
{ 
this.unit = unit;
}

protected override void Initialize()
{ 
this.game = (BattlescapeInfo)this.game;
base.RegisterHandler<GameButton>(GameButton.CenterOnFriendlyUnit, new Action(this.CenterOnUnit));
base.RegisterHandler<GameButton>(GameButton.ShowSoldierStats, new Action(this.ShowSoldierStats));
base.RegisterHandler<GameButton>(GameButton.SelectFriendlyUnit, new Action<ButtonCommand<GameButton>>(this.SelectFriendlyUnit));
this.subState = this.unit.State;
}

private void SelectFriendlyUnit(ButtonCommand<GameButton> cmd)
{ 
if (cmd.value == this.unit.index)
{ 
this.CenterOnUnit();
}
else
{ 
cmd.handled = false;
}
}

private void ShowSoldierStats()
{ 
this.Push(new SoldierStatsMenuState(this.game.behaviour.Menu.FindMenu("SoldierStats"), this.unit));
}

private void CenterOnUnit()
{ 
CameraBase.ScrollTo(this.unit.Model.transform.position, false);
}

public override void Update()
{ 
if (!this.unit.IsAlive)
{ 
this.Pop();
this.game.state.SendButton<GameButton>(GameButton.NextFriendlyUnit, 0);
}
this.game.gui.friendlyGui.Draw(this.unit);
}

public override void Start()
{ 
this.unit.selected = true;
this.unit.UpdateGui();
}

public override void End()
{ 
this.unit.selected = false;
this.unit.UpdateGui();
}

public override string ToString()
{ 
return "Unit->" + this.unit.State;
}
}
}
