using System;


namespace Game.Data.Items.Weapons
{ 
public class RocketLauncher : Weapon
{ 
public RocketLauncher(Ammo ammo)
{ 
this.name = "Rocket launcher";
this.weight = 9f;
this.attackCost = 0.8f;
this.rateOfFire = 60f;
this.roundsPerAttack = 1;
this.aimAtGround = true;
this.accuracy = 1f;
this.ammo = ammo;
}
}
}
