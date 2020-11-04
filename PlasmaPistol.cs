using System;


namespace Game.Data.Items.Weapons
{ 
public class PlasmaPistol : Weapon
{ 
public PlasmaPistol(Ammo ammo)
{ 
this.ammo = ammo;
this.name = "Plasma pistol";
this.weight = 3.5f;
this.attackCost = 0.35f;
this.rateOfFire = 80f;
this.roundsPerAttack = 1;
this.accuracy = 0.5f;
this.aimAtGround = false;
}
}
}
