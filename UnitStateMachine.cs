using Game.States;
using System;


namespace Game.UnitStates
{ 
public class UnitStateMachine : StateMachine<UnitState>
{ 
private Unit unit;

private BattlescapeInfo game;

public UnitStateMachine(BattlescapeInfo game, Unit unit)
{ 
this.game = game;
this.unit = unit;
}

public override void Push(UnitState state)
{ 
state.game = this.game;
state.unit = this.unit;
base.Push(state);
}
}
}
