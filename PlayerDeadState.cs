using System;


namespace Game.Zombiescape.States.PlayerStates
{ 
public class PlayerDeadState : PlayerState
{ 
public PlayerDeadState()
{ 
this.ismodal = true;
}

public override void Start()
{ 
this.player.Model.animator.SetBool("Running", false);
this.player.Model.animator.SetBool("Strafing", false);
this.player.Model.animator.SetBool("FiringRifle", false);
this.player.Model.animator.SetBool("HoldingRifle", false);
this.player.Model.animator.SetBool("Dead", true);
}
}
}
