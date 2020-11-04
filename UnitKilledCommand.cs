using System;


namespace Game.Commands
{ 
public class UnitKilledCommand : Command
{ 
public readonly Unit unit;

public UnitKilledCommand(Unit unit)
{ 
this.unit = unit;
}
}
}
using Game.Commands;
using Game.Data;
using System;


namespace Game.Zombiescape.Commands
{ 
public class UnitKilledCommand : Command
{ 
public IUnit unit;

public UnitKilledCommand(IUnit unit)
{ 
this.unit = unit;
}
}
}
