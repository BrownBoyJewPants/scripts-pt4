using Game.Data;
using Game.States;
using System;


namespace Game.Zombiescape.States.AIStates
{ 
public class AIStateMachine : StateMachine<AIState>
{ 
private IUnit unit;

private ZombiescapeInfo game;

public AIStateMachine(ZombiescapeInfo game, IUnit unit)
{ 
this.game = game;
this.unit = unit;
this.Push(new AIRootState());
}

public override void Push(AIState state)
{ 
state.game = this.game;
state.unit = this.unit;
base.Push(state);
}
}
}
