using System;
using TileEngine.Pathfinding;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine;
using VoxelEngine.Renderers;


namespace Game.Gui
{ 
public class PathFieldGui
{ 
private Material material;

private SimpleRenderBuffer buffer;

private Mesh mesh;

private PathField meshedField;

private MapCell meshedCell;

public PathFieldGui(Material material)
{ 
this.material = material;
this.mesh = new Mesh();
this.buffer = new SimpleRenderBuffer(true, false);
}

public void Draw(PathField field)
{ 
if (this.meshedField != field)
{ 
this.buffer.Begin(new Vector3(0f, 0f, 0f));
for (int i = 0; i < field.Dx; i++)
{ 
for (int j = 0; j < field.Dy; j++)
{ 
for (int k = 0; k < field.Dz; k++)
{ 
MapCell mapCell = field.Map[i, j, k];
if (mapCell != null)
{ 
byte b = (byte)(field.GetValue(mapCell) * 2f);
Color32 color = new Color32(b, b, b, 255);
Vector3 worldposition = mapCell.worldposition;
worldposition.y -= 1.48f;
this.buffer.AddFace(ref worldposition, BlockFace.Top, ref color, 1f, 1f, 1f);
}
}
}
}
this.buffer.UpdateMesh(this.mesh);
this.meshedField = field;
}
Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, this.material, 0, Camera.main, 0, null, false, true);
i
j
k
mapCell
b
color
worldposition
}

public void Draw(Unit unit, MapCell target)
{ 
if (target == null)
{ 
return;
}
}
}
}
