using Game.UnitStates;
using System;
using TileEngine;
using TileEngine.TileMap;


namespace Game.States
{ 
public class BattleScapeAIState : UnitState
{ 
protected Unit GetClosestVisibleEnemy()
{ 
Unit result = null;
float num = 3.40282347E+38f;
Unit[] units = this.unit.Visibility.units;
for (int i = 0; i < units.Length; i++)
{ 
float sqrMagnitude = (units[i].Cell.worldposition - this.unit.Cell.worldposition).sqrMagnitude;
if (sqrMagnitude < num)
{ 
num = sqrMagnitude;
result = units[i];
}
}
return result;
result
num
units
i
sqrMagnitude
}

protected int GetVisibleEnemyCount()
{ 
return this.unit.Visibility.units.Length;
}

protected Unit[] GetVisibleEnemies()
{ 
return this.unit.Visibility.units;
}

private int GetOpenAreaPenalty(MapCell cell)
{ 
Map map = cell.Map;
if (!map.CanMove(cell, TilePart.North) || !map.CanMove(cell, TilePart.South) || !map.CanMove(cell, TilePart.East) || !map.CanMove(cell, TilePart.West))
{ 
return 0;
}
return 500;
map
}

public override string ToString()
{ 
return base.GetType().Name.Replace("AIState", string.Empty);
}

public virtual void RoundStart()
{ 
}
}
}
