using Game.Commands;
using Game.Data.Items;
using System;
using UnityEngine;


namespace Game.Zombiescape.Commands
{ 
public class PlayerFireWeaponCommand : Command
{ 
public readonly Weapon weapon;

public readonly Vector3 target;

public PlayerFireWeaponCommand(Weapon weapon, Vector3 target)
{ 
this.weapon = weapon;
this.target = target;
}
}
}
