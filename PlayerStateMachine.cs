using Game.States;
using System;


namespace Game.Zombiescape.States.PlayerStates
{ 
public class PlayerStateMachine : StateMachine<PlayerState>
{ 
private Player player;

public PlayerStateMachine(Player player)
{ 
this.player = player;
}

public override void Push(PlayerState state)
{ 
state.player = this.player;
base.Push(state);
}
}
}
