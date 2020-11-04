using System;
using UnityEngine;
using VoxelEngine.Models;


namespace Game.Data.Items
{ 
public class Weapon : Item
{ 
private new VoxelModel model;

private Transform ejectionpoint;

private Transform firepoint;

public bool firingAtGround;

public float lastFireTime;

public float rateOfFire;

public int roundsPerAttack;

public float attackCost;

public float fireCircle;

public Ammo ammo;

public bool aimAtGround;

public float accuracy;

public Transform FirePoint
{ 
get
{ 
return this.firepoint;
}
}

public Transform EjectionPoint
{ 
get
{ 
return this.ejectionpoint;
}
}

public VoxelModel Model
{ 
get
{ 
return this.model;
}
set
{ 
this.model = value;
this.firepoint = value.GetTag("firepoint");
this.ejectionpoint = value.GetTag("ejectionpoint");
}
}
}
}
