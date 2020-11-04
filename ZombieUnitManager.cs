using Game.Data;
using Global.Data.Units;
using System;
using System.Linq;
using UnityEngine;


namespace Game.Zombiescape
{ 
public class ZombieUnitManager : UnitManager
{ 
private Player player;

private ZombiescapeInfo game;

private Vector3 unitSize = new Vector3(0.5f, 0.7f, 0.5f);

public Player Player
{ 
get
{ 
return this.player;
}
}

public ZombieUnitManager(ZombiescapeInfo game, Player player) : base(game)
{ 
this.game = game;
this.player = player;
base.Friendlies.Add(player);
}

private bool IsAir(Bounds bounds, IUnit ignore)
{ 
return !base.GetUnitsAt(bounds).Any((IUnit u) => u != ignore);
<IsAir>c__AnonStorey2B
}

internal void AdjustMovementToAllowed(ref Vector3 movement, Bounds bounds, IUnit unit)
{ 
Vector3 center = bounds.center;
while (true)
{ 
bounds.center = center + new Vector3(movement.x, 0f, 0f);
if (movement.x != 0f && !this.IsAir(bounds, unit))
{ 
movement.x = 0f;
movement.z *= 1.4f;
}
bounds.center = center + movement;
if (movement.z == 0f || this.IsAir(bounds, unit))
{ 
break;
}
movement.z = 0f;
movement.x *= 1.4f;
}
center
}
}
}
