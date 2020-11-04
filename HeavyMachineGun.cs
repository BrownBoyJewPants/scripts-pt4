using System;


namespace Game.Data.Items.Weapons
{ 
public class HeavyMachineGun : Weapon
{ 
public HeavyMachineGun(Ammo ammo)
{ 
this.name = "Heavy Machine Gun";
this.weight = 3.5f;
this.attackCost = 0.35f;
this.rateOfFire = 600f;
this.roundsPerAttack = 200;
this.accuracy = 0.8f;
this.fireCircle = 0.2f;
this.ammo = ammo;
this.aimAtGround = false;
}
}
}
