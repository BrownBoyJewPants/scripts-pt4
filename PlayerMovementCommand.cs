using Game.Commands;
using System;
using UnityEngine;


namespace Game.Zombiescape.Commands
{ 
public class PlayerMovementCommand : Command
{ 
public readonly Vector3 delta;

public PlayerMovementCommand(Vector3 delta)
{ 
this.delta = delta;
}
}
}
