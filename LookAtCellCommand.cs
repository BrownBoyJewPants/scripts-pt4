using System;
using TileEngine.TileMap;


namespace Game.Commands
{ 
public class LookAtCellCommand : Command
{ 
public readonly MapCell cell;

public LookAtCellCommand(MapCell cell)
{ 
this.cell = cell;
}
}
}
