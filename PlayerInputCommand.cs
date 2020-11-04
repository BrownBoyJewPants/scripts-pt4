using Game.Commands;
using System;


namespace Game.Zombiescape.Commands
{ 
public class PlayerInputCommand : Command
{ 
public readonly InputType type;

public bool forceAimAtGround;

public PlayerInputCommand(InputType type, bool forceAimAtGround)
{ 
this.forceAimAtGround = forceAimAtGround;
this.type = type;
}
}
}
