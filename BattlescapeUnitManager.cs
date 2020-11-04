using Game.Commands;
using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TileEngine.TileMap;
using UnityEngine;


namespace Game
{ 
public class BattlescapeUnitManager : IUnitManager
{ 
private Dictionary<int, Unit> unitByLocation;

private BattlescapeInfo game;

public bool isDirty;

IEnumerable<IUnit> IUnitManager.Friendlies
{ 
get
{ 
BattlescapeUnitManager.<>c__Iterator6 <>c__Iterator = new BattlescapeUnitManager.<>c__Iterator6();
<>c__Iterator.<>f__this = this;
BattlescapeUnitManager.<>c__Iterator6 expr_0E = <>c__Iterator;
expr_0E.$PC = -2;
return expr_0E;
<>c__Iterator
expr_0E
}
}

IEnumerable<IUnit> IUnitManager.Enemies
{ 
get
{ 
BattlescapeUnitManager.<>c__Iterator7 <>c__Iterator = new BattlescapeUnitManager.<>c__Iterator7();
<>c__Iterator.<>f__this = this;
BattlescapeUnitManager.<>c__Iterator7 expr_0E = <>c__Iterator;
expr_0E.$PC = -2;
return expr_0E;
<>c__Iterator
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

public BattlescapeUnitList Friendlies
{ 
get;
private set;
}

public BattlescapeUnitList Enemies
{ 
get;
private set;
}

public BattlescapeInfo Game
{ 
get
{ 
return this.game;
}
}

public BattlescapeUnitManager(BattlescapeInfo game)
{ 
this.game = game;
this.Friendlies = new BattlescapeUnitList(this);
this.Enemies = new BattlescapeUnitList(this);
this.unitByLocation = new Dictionary<int, Unit>();
}

IUnit[] IUnitManager.GetUnitsAt(Bounds bounds)
{ 
return this.GetUnitsAt(bounds);
}

IUnit[] IUnitManager.GetUnitsAt(MapCell cell)
{ 
Unit unitAt = this.GetUnitAt(cell);
if (unitAt == null)
{ 
return null;
}
return new Unit[]
{ 
unitAt
};
unitAt
}

void IUnitManager.Remove(IUnit unit)
{ 
this.Enemies.Remove((Unit)unit);
this.Friendlies.Remove((Unit)unit);
}

public void Update()
{ 
this.Friendlies.Update();
this.Enemies.Update();
}

public void FixedUpdate()
{ 
this.Friendlies.FixedUpdate();
this.Enemies.FixedUpdate();
}

public Unit GetUnitAt(MapCell cell)
{ 
if (cell == null)
{ 
return null;
}
Unit result;
this.unitByLocation.TryGetValue(cell.Id, out result);
return result;
result
}

public Unit[] GetUnitsAt(Bounds bounds)
{ 
List<Unit> units = new List<Unit>();
IntRect intRect = new IntRect(bounds.min, bounds.max);
intRect.ForEach(delegate(int x, int y, int z)
{ 
Unit unitAt = this.GetUnitAt(this.game.map[x, y, z]);
if (unitAt != null)
{ 
units.Add(unitAt);
}
unitAt
});
return units.ToArray();
<GetUnitsAt>c__AnonStorey
intRect
}

public void AddUnitAtPosition(Unit unit)
{ 
this.unitByLocation.Add(unit.Cell.Id, unit);
this.isDirty = true;
}

public void RemoveUnitAtPosition(Unit unit)
{ 
this.unitByLocation.Remove(unit.Cell.Id);
this.isDirty = true;
}

public IUnit GetClosestTo(Vector3 position, float maxDistance)
{ 
return null;
}

public void Destroy(Unit unit)
{ 
if (unit.isFriendly)
{ 
this.Friendlies.Remove(unit);
this.Enemies.RemoveFromVisibility(unit);
}
else
{ 
this.Enemies.Remove(unit);
this.Friendlies.RemoveFromVisibility(unit);
}
this.RemoveUnitAtPosition(unit);
}

internal void SelectNextFriendly()
{ 
for (int i = 0; i < this.Friendlies.Count - 1; i++)
{ 
if (this.Friendlies[i].selected)
{ 
this.game.state.SendCommand(new SelectFriendlyUnitCommand(this.Friendlies[i + 1]));
return;
}
}
if (this.Friendlies.Any((Unit f) => f.IsAlive))
{ 
this.game.state.SendCommand(new SelectFriendlyUnitCommand(this.Friendlies.FirstOrDefault((Unit f) => f.IsAlive)));
}
i
}

internal void SelectPreviousFriendly()
{ 
for (int i = this.Friendlies.Count - 1; i > 0; i--)
{ 
if (this.Friendlies[i].selected)
{ 
this.game.state.SendCommand(new SelectFriendlyUnitCommand(this.Friendlies[i - 1]));
return;
}
}
this.game.state.SendCommand(new SelectFriendlyUnitCommand(this.Friendlies[this.Friendlies.Count - 1]));
i
}
}
}
