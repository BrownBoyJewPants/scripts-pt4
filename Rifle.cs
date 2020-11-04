using System;


namespace Game.Data.Items.Weapons
{ 
public class Rifle : Weapon
{ 
public Rifle(Ammo ammo)
{ 
this.name = "Rifle";
this.weight = 3.5f;
this.attackCost = 0.35f;
this.rateOfFire = 600f;
this.roundsPerAttack = 1;
this.accuracy = 1f;
this.ammo = ammo;
this.aimAtGround = false;
}
}
}
