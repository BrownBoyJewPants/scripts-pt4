using Game.Data;
using Game.Data.Items;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Zombiescape
{ 
public class AmmoManager
{ 
private List<Ammo> list = new List<Ammo>();

public void Update()
{ 
for (int i = this.list.Count - 1; i >= 0; i--)
{ 
Ammo ammo = this.list[i];
if (!ammo.alive)
{ 
this.list.RemoveAt(i);
}
else
{ 
ammo.Update();
}
}
i
ammo
}

internal void Add(Ammo ammo)
{ 
this.list.Add(ammo);
}

public bool FireWeapon(IUnit unit, Weapon weapon, Vector3 target, bool checkIfPossible = true)
{ 
if (checkIfPossible && Time.timeSinceLevelLoad - weapon.lastFireTime < 60f / weapon.rateOfFire)
{ 
return false;
}
for (int i = 0; i < weapon.roundsPerAttack; i++)
{ 
this.Add(weapon.ammo.Spawn(unit, weapon, target, i));
}
weapon.lastFireTime = Time.timeSinceLevelLoad;
return true;
i
}
}
}
