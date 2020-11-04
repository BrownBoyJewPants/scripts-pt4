using System;


namespace Game.Commands
{ 
public class SelectEnemyUnitCommand : Command
{ 
public readonly Unit unit;

public SelectEnemyUnitCommand(Unit unit)
{ 
this.unit = unit;
}
}
}
