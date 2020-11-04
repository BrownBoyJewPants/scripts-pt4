using System;
using System.Collections.Generic;
using System.Linq;
using TileEngine;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Data
{ 
public class BaseMap
{ 
private List<PlacedModule> placed;

private Map map;

private VoxelResourceManager resources;

public IntVector2 size;

public IntVector2 gridSize;

public IEnumerable<PlacedModule> Placed
{ 
get
{ 
return this.placed;
}
}

public Map TileMap
{ 
get
{ 
return this.map;
}
}

public BaseMap(VoxelResourceManager resources, IntVector2 size, IntVector2 gridSize)
{ 
this.resources = resources;
this.size = size;
this.gridSize = gridSize;
this.placed = new List<PlacedModule>();
this.map = new Map(size.x, 5, size.y);
}

public PlacedModule GetModuleAt(IntVector2 position)
{ 
return this.placed.FirstOrDefault((PlacedModule p) => p.bounds.Contains(position));
<GetModuleAt>c__AnonStorey1C
}

public bool CanPlace(BaseModule module, IntVector2 position)
{ 
IntRect2 bounds = new IntRect2(position, position + new IntVector2(module.template.dx - 1, module.template.dz - 1));
return this.placed.All((PlacedModule p) => !p.bounds.Intersects(ref bounds));
<CanPlace>c__AnonStorey1D
}

public bool PlaceModule(BaseModule module, IntVector2 position)
{ 
if (!this.CanPlace(module, position))
{ 
return false;
}
PlacedModule placedModule = new PlacedModule(module, position);
this.placed.Add(placedModule);
this.CopyToMap(placedModule);
return true;
placedModule
}

private void CopyToMap(PlacedModule pm)
{ 
TemplateAsset template = pm.module.template;
int num = 0;
for (int i = 0; i < template.dz; i++)
{ 
for (int j = 0; j < template.dy; j++)
{ 
for (int k = 0; k < template.dx; k++)
{ 
TemplateTile[] tiles = template.cells[num++].tiles;
for (int l = 0; l < tiles.Length; l++)
{ 
TemplateTile templateTile = tiles[l];
VoxelResource resource = this.resources.GetResource(templateTile.tile, templateTile.facing, templateTile.mirror);
this.map[k + pm.position.x, j, i + pm.position.y].AddTile(resource, new Vector3?(templateTile.offset), true);
}
}
}
}
if (pm.module.name != null)
{ 
this.CheckHallways(pm);
}
template
num
i
j
k
tiles
l
templateTile
resource
}

private void CheckHallways(PlacedModule pm)
{ 
TemplateAsset template = pm.module.template;
for (int i = this.gridSize.x / 2; i < template.dx; i += this.gridSize.x)
{ 
int num = pm.position.x + i;
int num2 = pm.position.y;
PlacedModule moduleAt = this.GetModuleAt(new IntVector2(num, num2 - 1));
if (moduleAt != null && moduleAt.module.name != null)
{ 
this.CreateHallway(num - 1, num2 - 2, num + 1, num2 + 1, false);
}
num2 = pm.position.y + this.gridSize.y;
moduleAt = this.GetModuleAt(new IntVector2(num, num2));
if (moduleAt != null && moduleAt.module.name != null)
{ 
this.CreateHallway(num - 1, num2 - 1, num + 1, num2 + 2, false);
}
}
for (int j = this.gridSize.y / 2; j < template.dz; j += this.gridSize.y)
{ 
int num3 = pm.position.x;
int num4 = pm.position.y + j;
PlacedModule moduleAt2 = this.GetModuleAt(new IntVector2(num3 - 1, num4));
if (moduleAt2 != null && moduleAt2.module.name != null)
{ 
this.CreateHallway(num3 - 2, num4 - 1, num3 + 1, num4 + 1, true);
}
num3 = pm.position.x + this.gridSize.x;
moduleAt2 = this.GetModuleAt(new IntVector2(num3, num4));
if (moduleAt2 != null && moduleAt2.module.name != null)
{ 
this.CreateHallway(num3 - 1, num4 - 1, num3 + 2, num4 + 1, true);
}
}
template
i
num
num2
moduleAt
j
num3
num4
moduleAt2
}

private void CreateHallway(int x1, int y1, int x2, int y2, bool horizontal)
{ 
VoxelResource resource = this.resources.GetResource("BaseFloor", TileFacing.None);
VoxelResource resource2 = this.resources.GetResource("BaseWall", (!horizontal) ? TileFacing.West : TileFacing.South);
VoxelResource resource3 = this.resources.GetResource("BaseWall", (!horizontal) ? TileFacing.East : TileFacing.North);
for (int i = x1; i <= x2; i++)
{ 
for (int j = y1; j <= y2; j++)
{ 
this.map[i, 0, j].Clear();
this.map[i, 0, j].AddTile(resource, null, true);
this.map[i, 1, j].Clear();
}
}
if (horizontal)
{ 
for (int k = x1 + 1; k < x2; k++)
{ 
this.map[k, 1, y1].AddTile(resource2, null, true);
this.map[k, 1, y2].AddTile(resource3, null, true);
}
}
else
{ 
for (int l = y1 + 1; l < y2; l++)
{ 
this.map[x1, 1, l].AddTile(resource2, null, true);
this.map[x2, 1, l].AddTile(resource3, null, true);
}
}
resource
resource2
resource3
i
j
k
l
}
}
}
