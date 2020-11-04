using Game.Zombiescape.States.AIStates;
using System;


namespace Game.Zombiescape.States
{ 
public class AIDeadState : AIState
{ 
public AIDeadState()
{ 
this.ismodal = true;
}

public override void Start()
{ 
base.Start();
}
}
}
