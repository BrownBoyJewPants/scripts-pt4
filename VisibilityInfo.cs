using System;
using System.Collections.Generic;
using System.Linq;
using TileEngine;
using TileEngine.TileMap;


namespace Game
{ 
public class VisibilityInfo
{ 
private Map map;

private Unit unit;

private HashSet<MapCell> cellsHash;

private HashSet<Unit> unitsHash;

public bool isdirty;

public MapCell[] cells;

public Unit[] units;

public VisibilityInfo(Unit unit)
{ 
this.map = unit.Manager.Game.map;
this.unit = unit;
this.unitsHash = new HashSet<Unit>();
this.cellsHash = new HashSet<MapCell>();
this.units = new Unit[0];
this.cells = new MapCell[0];
this.isdirty = true;
}

public bool IsVisible(MapCell cell)
{ 
if (cell == null)
{ 
return false;
}
this.Update();
return this.cellsHash.Contains(cell);
}

public bool IsVisible(Unit unit)
{ 
return this.IsVisible(this.map[unit.Model.transform.position]);
}

public void Update()
{ 
if (!this.isdirty)
{ 
return;
}
this.unitsHash.Clear();
this.cellsHash.Clear();
int num = (int)this.unit.Model.transform.position.x;
int num2 = (int)this.unit.Model.transform.position.z;
float y = this.unit.Model.transform.localRotation.eulerAngles.y;
int firstOctant = 9 - (int)((y + 22.5f) / 45f);
ShadowCaster.ComputeFieldOfViewWithShadowCasting(num * 3 + 1, num2 * 3 + 1, (int)(this.unit.stats.sightrange * 3f), new Func<int, int, bool>(this.CheckCell), delegate(int x, int z)
{ 
this.SetVisible(this.unit, x, z);
}, firstOctant);
this.units = this.unitsHash.ToArray<Unit>();
this.cells = this.cellsHash.ToArray<MapCell>();
this.isdirty = false;
num
num2
y
firstOctant
}

private bool CheckCell(int sx, int sz)
{ 
Map map = this.unit.Manager.Game.map;
int num = sx / 3;
int num2 = sz / 3;
if (num < 0 || num2 < 0 || num >= map.Dx || num2 >= map.Dz)
{ 
return true;
}
int num3 = sx % 3;
int num4 = sz % 3;
TilePart visible = map[num, 1, num2].visible;
return (num3 == 0 && (visible & TilePart.West) == TilePart.None) || (num3 == 2 && (visible & TilePart.East) == TilePart.None) || (num4 == 0 && (visible & TilePart.South) == TilePart.None) || (num4 == 2 && (visible & TilePart.North) == TilePart.None) || ((num3 == 1 || num4 == 1) && (visible & TilePart.Center) == TilePart.None);
map
num
num2
num3
num4
visible
}

private void SetVisible(Unit unit, int sx, int sz)
{ 
if (sx % 3 != 1 || sz % 3 != 1)
{ 
return;
}
int num = sx / 3;
int num2 = sz / 3;
if (num < 0 || num2 < 0 || num >= this.map.Dx || num2 >= this.map.Dz)
{ 
return;
}
int num3 = unit.Cell.mapposition.x - num;
int num4 = unit.Cell.mapposition.z - num2;
MapCell mapCell = this.map[num, unit.Cell.mapposition.y, num2];
this.cellsHash.Add(mapCell);
Unit unitAt = unit.Manager.GetUnitAt(mapCell);
if (unitAt != null && unitAt.isFriendly != unit.isFriendly)
{ 
this.unitsHash.Add(unitAt);
}
num
num2
num3
num4
mapCell
unitAt
}

internal void Remove(Unit unit)
{ 
if (this.unitsHash.Remove(unit))
{ 
this.units = this.unitsHash.ToArray<Unit>();
}
}
}
}
