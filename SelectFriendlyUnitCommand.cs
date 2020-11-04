using System;


namespace Game.Commands
{ 
public class SelectFriendlyUnitCommand : Command
{ 
public readonly Unit unit;

public SelectFriendlyUnitCommand(Unit unit)
{ 
this.unit = unit;
}
}
}
