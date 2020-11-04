using Game.Data;
using Game.States;
using System;


namespace Game.GameStates
{ 
public class GameStateMachine : StateMachine<GameState>
{ 
private IGameInfo game;

public GameStateMachine(IGameInfo game)
{ 
this.game = game;
}

public override void Push(GameState state)
{ 
state.game = this.game;
base.Push(state);
}
}
}
