using System;


namespace Game.Data.Items.Weapons
{ 
public class AC130MainGun : Weapon
{ 
public AC130MainGun(Ammo ammo)
{ 
this.name = "AC-130 Main Gun";
this.weight = 3.5f;
this.attackCost = 0.35f;
this.rateOfFire = 750f;
this.roundsPerAttack = 5;
this.accuracy = 1f;
this.ammo = ammo;
this.aimAtGround = false;
}
}
}
