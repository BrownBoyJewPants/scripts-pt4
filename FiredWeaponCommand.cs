using Game.Commands;
using Game.Data.Items;
using System;


namespace Game.Zombiescape.Commands
{ 
public class FiredWeaponCommand : Command
{ 
public readonly Weapon weapon;

public FiredWeaponCommand(Weapon weapon)
{ 
this.weapon = weapon;
}
}
}
