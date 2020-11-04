using Game.Commands;
using System;


namespace Game.Zombiescape.Commands
{ 
public class TookDamageCommand : Command
{ 
public float amount;

public TookDamageCommand(float amount)
{ 
this.amount = amount;
}
}
}
