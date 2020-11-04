using System;
using TileEngine.TileMap;


namespace Game.Commands
{ 
public class CellHoverCommand : Command
{ 
public MapCell cell;

public CellHoverCommand(MapCell cell)
{ 
this.cell = cell;
}
}
}
