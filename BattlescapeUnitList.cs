using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TileEngine.TileMap;


namespace Game
{ 
public class BattlescapeUnitList : IEnumerable, IEnumerable<Unit>
{ 
private List<Unit> units;

private BattlescapeUnitManager manager;

private List<Unit> removeUnits;

public int Count
{ 
get
{ 
return this.units.Count;
}
}

public Unit this[int index]
{ 
get
{ 
return this.units[index];
}
}

public BattlescapeUnitList(BattlescapeUnitManager manager)
{ 
this.manager = manager;
this.units = new List<Unit>();
this.removeUnits = new List<Unit>();
}

IEnumerator IEnumerable.GetEnumerator()
{ 
return this.GetEnumerator();
}

public void Add(Unit unit)
{ 
unit.Init(this.manager);
unit.index = this.units.Count;
this.units.Add(unit);
this.manager.AddUnitAtPosition(unit);
}

public void Update()
{ 
foreach (Unit current in this.units)
{ 
current.Update();
}
foreach (Unit current2 in this.removeUnits)
{ 
this.units.Remove(current2);
}
this.removeUnits.Clear();
enumerator
current
enumerator2
current2
}

public void FixedUpdate()
{ 
foreach (Unit current in this.units)
{ 
current.FixedUpdate();
}
enumerator
current
}

public void Remove(Unit unit)
{ 
this.removeUnits.Add(unit);
}

public IEnumerator<Unit> GetEnumerator()
{ 
return this.units.GetEnumerator();
}

internal void RemoveFromVisibility(Unit unit)
{ 
foreach (Unit current in this.units)
{ 
current.Visibility.Remove(unit);
}
enumerator
current
}

public bool CanSee(MapCell cell)
{ 
return this.units.Any((Unit u) => u.Visibility.IsVisible(cell));
<CanSee>c__AnonStorey1F
}

public bool CanSee(Unit unit)
{ 
return this.units.Any((Unit u) => u.Visibility.IsVisible(unit));
<CanSee>c__AnonStorey
}

public Unit GetNext(Unit unit)
{ 
int num = this.units.IndexOf(unit);
if (num == this.units.Count - 1)
{ 
num = -1;
}
return this.units[num + 1];
num
}

public Unit GetPrevious(Unit unit)
{ 
int num = this.units.IndexOf(unit);
if (num <= 0)
{ 
num = this.units.Count;
}
return this.units[num - 1];
num
}
}
}
