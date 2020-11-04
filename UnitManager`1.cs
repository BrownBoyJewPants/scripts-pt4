using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TileEngine.TileMap;
using UnityEngine;


namespace Global.Data.Units
{ 
public class UnitManager<T> : IUnitManager where T : class, IUnit
{ 
private Dictionary<int, List<T>> enemiesByLocation;

private Dictionary<int, List<T>> friendliesByLocation;

private Queue<List<T>> lists = new Queue<List<T>>();

private List<T> tmpList = new List<T>();

private IGameInfo game;

public bool isDirty;

IEnumerable<IUnit> IUnitManager.Friendlies
{ 
get
{ 
UnitManager<T>.<>c__IteratorE <>c__IteratorE = new UnitManager<T>.<>c__IteratorE();
<>c__IteratorE.<>f__this = this;
UnitManager<T>.<>c__IteratorE expr_0E = <>c__IteratorE;
expr_0E.$PC = -2;
return expr_0E;
<>c__IteratorE
expr_0E
}
}

IEnumerable<IUnit> IUnitManager.Enemies
{ 
get
{ 
UnitManager<T>.<>c__IteratorF <>c__IteratorF = new UnitManager<T>.<>c__IteratorF();
<>c__IteratorF.<>f__this = this;
UnitManager<T>.<>c__IteratorF expr_0E = <>c__IteratorF;
expr_0E.$PC = -2;
return expr_0E;
<>c__IteratorF
expr_0E
}
}

bool IUnitManager.Dirty
{ 
get
{ 
return this.isDirty;
}
set
{ 
this.isDirty = value;
}
}

public UnitList<T> Friendlies
{ 
get;
private set;
}

public UnitList<T> Enemies
{ 
get;
private set;
}

public IGameInfo Game
{ 
get
{ 
return this.game;
}
}

public UnitManager(IGameInfo game)
{ 
this.game = game;
this.Friendlies = new UnitList<T>(this);
this.Enemies = new UnitList<T>(this);
this.enemiesByLocation = new Dictionary<int, List<T>>();
this.friendliesByLocation = new Dictionary<int, List<T>>();
}

IUnit[] IUnitManager.GetUnitsAt(Bounds bounds)
{ 
return this.GetUnitsAt(bounds).Cast<IUnit>().ToArray<IUnit>();
}

IUnit[] IUnitManager.GetUnitsAt(MapCell cell)
{ 
return this.GetUnitsAt(cell).ToArray<T>();
}

void IUnitManager.Remove(IUnit unit)
{ 
if (unit.IsFriendly)
{ 
this.Friendlies.Remove(unit);
}
else
{ 
this.Enemies.Remove(unit);
}
}

IUnit IUnitManager.GetClosestTo(Vector3 position, float maxDistance)
{ 
return this.GetClosestTo(position, maxDistance);
}

public virtual void Update()
{ 
this.Friendlies.Update();
this.Enemies.Update();
}

public virtual void FixedUpdate()
{ 
this.Friendlies.FixedUpdate();
this.Enemies.FixedUpdate();
}

private void RemoveUnitAt(MapCell cell, T unit, Dictionary<int, List<T>> dict)
{ 
List<T> list;
if (dict.TryGetValue(cell.Id, out list))
{ 
list.Remove(unit);
if (list.Count == 0)
{ 
dict.Remove(cell.Id);
this.lists.Enqueue(list);
}
}
list
}

public void RemoveUnitAt(MapCell cell, T unit)
{ 
if (cell == null)
{ 
return;
}
if (unit.IsFriendly)
{ 
this.RemoveUnitAt(cell, unit, this.friendliesByLocation);
}
else
{ 
this.RemoveUnitAt(cell, unit, this.enemiesByLocation);
}
this.isDirty = true;
}

private void AddUnitsAtPriv(MapCell cell, T unit, Dictionary<int, List<T>> dict)
{ 
List<T> list;
if (!dict.TryGetValue(cell.Id, out list))
{ 
if (this.lists.Count > 0)
{ 
list = this.lists.Dequeue();
}
else
{ 
list = new List<T>();
}
dict[cell.Id] = list;
}
list.Add(unit);
list
}

public T GetClosestTo(Vector3 position, float maxDistance)
{ 
float num = -1f;
T result = (T)((object)null);
float num2 = maxDistance * maxDistance;
foreach (T current in this.Enemies)
{ 
float sqrMagnitude = (position - current.Model.transform.position).sqrMagnitude;
if (sqrMagnitude <= maxDistance && (num == -1f || sqrMagnitude < num))
{ 
num = sqrMagnitude;
result = current;
}
}
return result;
num
result
num2
enumerator
current
sqrMagnitude
}

public void AddUnitAt(MapCell cell, T unit)
{ 
if (cell == null)
{ 
return;
}
if (unit.IsFriendly)
{ 
this.AddUnitsAtPriv(cell, unit, this.friendliesByLocation);
}
else
{ 
this.AddUnitsAtPriv(cell, unit, this.enemiesByLocation);
}
this.isDirty = true;
}

public IEnumerable<T> GetUnitsAt(MapCell cell)
{ 
if (cell == null)
{ 
return null;
}
List<T> result;
this.enemiesByLocation.TryGetValue(cell.Id, out result);
return result;
result
}

public IEnumerable<T> GetUnitsAt(Bounds bounds)
{ 
this.tmpList.Clear();
IntRect intRect = new IntRect(bounds.min, bounds.max);
intRect.x1--;
intRect.x2++;
intRect.z1--;
intRect.z2++;
intRect.y1 = (intRect.y2 = 1);
intRect.ForEach(delegate(int x, int y, int z)
{ 
IEnumerable<T> unitsAt = this.GetUnitsAt(this.game.Map[x, y, z]);
if (unitsAt != null)
{ 
this.tmpList.AddRange(unitsAt);
}
unitsAt
});
return this.tmpList;
intRect
}

public void Destroy(T unit)
{ 
this.RemoveUnitAt(unit.Cell, unit);
if (unit.IsFriendly)
{ 
this.Friendlies.Remove(unit);
}
else
{ 
this.Enemies.Remove(unit);
}
}
}
}
