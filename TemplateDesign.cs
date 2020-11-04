using System;
using System.Collections.Generic;
using TileEngine;
using UnityEngine;


[ExecuteInEditMode, RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(MeshRenderer))]
public class TemplateDesign : MonoBehaviour
{ 
public Material material;

public VoxelResourceManager resourceManager;

public TemplateAsset template;

public int currentLayer;

public int visibleLayer;

public int selectedTileIndex;

public string selectedResource;

public int rotation;

public GameObject selectedTile;

public Vector3 selectedCell;

[NonSerialized]
public Vector3 mouseOverCell;

public TileMirror mirror;

public TemplateDesign()
{ 
this.selectedCell = Vector3.zero;
this.selectedCell.x = -1f;
}

private void OnEnable()
{ 
this.mouseOverCell = Vector3.zero;
this.mouseOverCell.x = -1f;
this.RebuildTemplate();
}

public float GetLayerOffset(int currentLayer)
{ 
float num = (float)currentLayer * 2f;
if (currentLayer > 0)
{ 
num -= 0.5f;
}
return num;
num
}

public float GetLayerHeight(int layer)
{ 
return (layer != 0) ? 2f : 1.5f;
}

public TemplateCell GetCell(Vector3 position)
{ 
int num = (int)position.x;
int num2 = (int)position.y;
int num3 = (int)position.z;
if (num < 0 || num2 < 0 || num3 < 0 || num >= this.template.dx || num2 >= this.template.dy || num3 >= this.template.dz)
{ 
return null;
}
return this.template.cells[num + num2 * this.template.dx + num3 * this.template.dx * this.template.dy];
num
num2
num3
}

public void RebuildTemplate()
{ 
if (this.template == null)
{ 
return;
}
List<CombineInstance> list = new List<CombineInstance>();
for (int i = 0; i < this.template.dz; i++)
{ 
for (int j = 0; j < Mathf.Min(this.visibleLayer + 1, this.template.dy); j++)
{ 
for (int k = 0; k < this.template.dx; k++)
{ 
TemplateCell templateCell = this.template.cells[k + j * this.template.dx + i * this.template.dx * this.template.dy];
TemplateTile[] tiles = templateCell.tiles;
for (int l = 0; l < tiles.Length; l++)
{ 
TemplateTile templateTile = tiles[l];
VoxelResource resource = this.resourceManager.GetResource(templateTile.tile, templateTile.facing, templateTile.mirror);
if (resource != null)
{ 
float num = (resource.scale - 1f) * 0.5f;
Vector3 b = new Vector3(num, 0f, num);
list.Add(new CombineInstance
{ 
mesh = resource.mesh,
transform = Matrix4x4.TRS(new Vector3((float)k, this.GetLayerOffset(j), (float)i) - b + resource.centerOffset + templateTile.offset, Quaternion.identity, Vector3.one)
});
}
}
}
}
}
MeshFilter component = base.GetComponent<MeshFilter>();
if (component.sharedMesh == null)
{ 
component.sharedMesh = new Mesh();
}
component.sharedMesh.CombineMeshes(list.ToArray());
list
i
j
k
templateCell
tiles
l
templateTile
resource
num
b
component
}

public void Resize(int dx, int dy, int dz)
{ 
TemplateCell[] array = new TemplateCell[dx * dy * dz];
int num = Mathf.Min(dx, this.template.dx);
int num2 = Mathf.Min(dy, this.template.dy);
int num3 = Mathf.Min(dz, this.template.dz);
for (int i = 0; i < dz; i++)
{ 
for (int j = 0; j < dy; j++)
{ 
for (int k = 0; k < dx; k++)
{ 
if (k < num && j < num2 && i < num3)
{ 
array[k + j * dx + i * dx * dy] = this.template.cells[k + j * this.template.dx + i * this.template.dx * this.template.dy];
}
else
{ 
array[k + j * dx + i * dx * dy] = new TemplateCell();
}
}
}
}
this.template.dx = dx;
this.template.dy = dy;
this.template.dz = dz;
this.template.cells = array;
this.RebuildTemplate();
array
num
num2
num3
i
j
k
}

public void Move(int x, int y, int z)
{ 
int num = this.template.cells.Length;
TemplateCell[] array = new TemplateCell[num];
int num2 = x + y * this.template.dx + z * this.template.dx * this.template.dy + num;
for (int i = 0; i < array.Length; i++)
{ 
array[i] = this.template.cells[(i + num2) % num];
}
this.template.cells = array;
num
array
num2
i
}

public void FixTileFields()
{ 
TemplateCell[] cells = this.template.cells;
for (int i = 0; i < cells.Length; i++)
{ 
TemplateCell templateCell = cells[i];
TemplateTile[] tiles = templateCell.tiles;
for (int j = 0; j < tiles.Length; j++)
{ 
TemplateTile templateTile = tiles[j];
VoxelResource resource = this.resourceManager.GetResource(templateTile.tile, TileFacing.None);
templateTile.resourceScale = resource.scale;
templateTile.resourceFacing = resource.facing;
}
}
cells
i
templateCell
tiles
j
templateTile
resource
}
}
