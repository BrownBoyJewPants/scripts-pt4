using Game.Data.Items;
using Game.UnitStates;
using System;
using TileEngine.Pathfinding;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.States
{ 
public class PatrolAIState : BattleScapeAIState
{ 
private MapCell target;

private MovingUnitState movingState;

private int reserve;

public override void Start()
{ 
this.reserve = this.unit.NeededTimeUnits((Weapon)this.unit.inventory.mainWeapon.item);
PathField pathField = this.unit.GetPathField();
MapCell[] pathTo;
do
{ 
this.target = this.game.map[this.unit.Cell.mapposition.x + UnityEngine.Random.Range(-20, 20), this.unit.Cell.mapposition.y, this.unit.Cell.mapposition.z + UnityEngine.Random.Range(-20, 20)];
pathTo = pathField.GetPathTo(this.target);
}
while (pathTo == null);
this.StartMoving();
base.Initialize();
pathField
pathTo
}

public override void RoundStart()
{ 
this.StartMoving();
}

private void StartMoving()
{ 
this.Push(this.movingState = new MovingUnitState(this.target, this.reserve));
}

public override void EnterForeground()
{ 
base.EnterForeground();
if (base.GetVisibleEnemyCount() > 0)
{ 
this.Pop();
}
else if (this.target == this.unit.Cell)
{ 
this.Pop();
}
else if (this.movingState.stoppedDueToReactionFire)
{ 
this.StartMoving();
}
}
}
}
