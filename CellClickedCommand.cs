using System;
using TileEngine.TileMap;


namespace Game.Commands
{ 
public class CellClickedCommand : Command
{ 
public readonly MapCell cell;

public CellClickedCommand(MapCell cell)
{ 
this.cell = cell;
}
}
}
