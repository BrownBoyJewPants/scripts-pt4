using Game.Data.Items;
using System;
using UnityEngine;


namespace Game.Zombiescape.Data
{ 
public class SpecialWeapon
{ 
public int killsRequired;

public int kills;

public readonly int shots;

public int shotsMade;

public readonly KeyCode keyToActivate;

public readonly Weapon weapon;

public readonly bool isPrimary;

public readonly string name;

public SpecialWeapon(string name, Weapon weapon, KeyCode key, int killsRequired, int shots, bool isPrimaryWeapon)
{ 
this.name = name;
this.weapon = weapon;
this.keyToActivate = key;
this.killsRequired = killsRequired;
this.shots = shots;
this.isPrimary = isPrimaryWeapon;
}
}
}
