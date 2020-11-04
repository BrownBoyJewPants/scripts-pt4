using Game.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Global.Data.Units
{ 
public class UnitList<T> : IEnumerable<T>, IEnumerable where T : class, IUnit
{ 
private List<IUnit> units;

private UnitManager<T> manager;

private List<IUnit> removeUnits;

public int Count
{ 
get
{ 
return this.units.Count;
}
}

public T this[int index]
{ 
get
{ 
return (T)((object)this.units[index]);
}
}

public UnitList(UnitManager<T> manager)
{ 
this.manager = manager;
this.units = new List<IUnit>();
this.removeUnits = new List<IUnit>();
}

IEnumerator IEnumerable.GetEnumerator()
{ 
return this.GetEnumerator();
}

public void Add(T unit)
{ 
this.units.Add(unit);
}

public void Update()
{ 
foreach (IUnit current in this.units)
{ 
current.State.Update();
}
foreach (IUnit current2 in this.removeUnits)
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
foreach (IUnit current in this.units)
{ 
current.State.FixedUpdate();
}
enumerator
current
}

public void Remove(IUnit unit)
{ 
this.removeUnits.Add(unit);
}

public IEnumerator<T> GetEnumerator()
{ 
return this.units.Cast<T>().GetEnumerator();
}

public T GetNext(T unit)
{ 
int num = this.units.IndexOf(unit);
if (num == this.units.Count - 1)
{ 
num = -1;
}
return (T)((object)this.units[num + 1]);
num
}

public T GetPrevious(T unit)
{ 
int num = this.units.IndexOf(unit);
if (num <= 0)
{ 
num = this.units.Count;
}
return (T)((object)this.units[num - 1]);
num
}
}
}
