using Game.Gui;
using System;
using System.Diagnostics;
using TileEngine.Batching;
using TileEngine.TileMap;
using UnityEngine;


namespace TileEngine.MonoBehaviours
{ 
[RequireComponent(typeof(Material))]
public class GraphicsTileMapBehaviour : MonoBehaviour, ITileMapBehaviour
{ 
public int VisibleLevel;

public Material material;

public Material invisibleMaterial;

public bool wrapAround;

private Map map;

private new Camera camera;

public MapCell mouseOverCell;

private Mesh m;

Material ITileMapBehaviour.Material
{ 
get
{ 
return this.material;
}
}

public Map Map
{ 
get
{ 
return this.map;
}
}

private void Awake()
{ 
this.VisibleLevel = 1;
this.camera = Camera.main;
if (this.material == null)
{ 
UnityEngine.Debug.LogError("No material attached to TileMap!");
return;
}
}

private void Start()
{ 
this.CreateCollider(-1, -1, this.map.Dx + 1, 0);
this.CreateCollider(-1, -1, 0, this.map.Dz + 1);
this.CreateCollider(this.map.Dx, 0, this.map.Dx + 1, this.map.Dz);
this.CreateCollider(0, this.map.Dz, this.map.Dx + 1, this.map.Dz + 1);
}

private void CreateCollider(int x1, int y1, int x2, int y2)
{ 
GameObject gameObject = new GameObject();
BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
boxCollider.center = new Vector3((float)(x1 + x2) * 0.5f, 2f, (float)(y1 + y2) * 0.5f);
boxCollider.size = new Vector3((float)(x2 - x1), 4f, (float)(y2 - y1));
boxCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
gameObject
boxCollider
}

private Quaternion GetRotation(TileFacing fromFacing, TileFacing toFacing)
{ 
return Quaternion.Euler(0f, (float)(toFacing - fromFacing) * 90f, 0f);
}

private void ProcessMouseInput()
{ 
if (GameGui.HitGUI(Input.mousePosition))
{ 
return;
}
Vector3 point = CameraBase.ScreenToGround(this.camera, Input.mousePosition, 1.5f);
point = new Vector3((float)((int)point.x) + 0.5f, point.y + 0.75f, (float)((int)point.z) + 0.5f);
this.mouseOverCell = this.map[point];
point
}

private void Update()
{ 
if (this.map == null)
{ 
return;
}
this.ProcessMouseInput();
ChunkManager chunks = this.map.Chunks;
Vector3 vector = CameraBase.ViewportToGround(this.camera, Vector2.zero);
Vector3 vector2 = CameraBase.ViewportToGround(this.camera, Vector2.one);
Vector3 vector3 = CameraBase.ViewportToGround(this.camera, new Vector2(0f, 1f));
Vector3 vector4 = CameraBase.ViewportToGround(this.camera, new Vector2(1f, 0f));
int num = 1;
Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
int num2 = (int)Mathf.Max(0f, Mathf.Min(new float[]
{ 
vector.x,
vector2.x,
vector3.x,
vector4.x
}) / (float)this.map.Chunks.ChunkSize - (float)num);
int num3 = (int)Mathf.Min((float)chunks.Dx, Mathf.Max(new float[]
{ 
vector.x,
vector2.x,
vector3.x,
vector4.x
}) / (float)this.map.Chunks.ChunkSize + 1f + (float)num);
int num4 = (int)Mathf.Max(0f, Mathf.Min(new float[]
{ 
vector.z,
vector2.z,
vector3.z,
vector4.z
}) / (float)this.map.Chunks.ChunkSize - (float)num);
int num5 = (int)Mathf.Min((float)chunks.Dz, Mathf.Max(new float[]
{ 
vector.z,
vector2.z,
vector3.z,
vector4.z
}) / (float)this.map.Chunks.ChunkSize + 1f + (float)num);
Stopwatch stopwatch = Stopwatch.StartNew();
for (int i = num4; i < num5; i++)
{ 
for (int j = 0; j < chunks.Dy; j++)
{ 
for (int k = num2; k < num3; k++)
{ 
ChunkManager.ignoreDirty = (stopwatch.ElapsedMilliseconds > 15L);
Chunk chunk = chunks[k, j, i];
if (GeometryUtility.TestPlanesAABB(planes, chunk.largeBounds))
{ 
bool flag = j <= this.VisibleLevel;
Graphics.DrawMesh(chunk.Mesh, Vector3.zero, Quaternion.identity, (!flag) ? this.invisibleMaterial : this.material, 0, Camera.main, 0, null, j > 0, flag);
}
}
}
}
chunks
vector
vector2
vector3
vector4
num
planes
num2
num3
num4
num5
stopwatch
i
j
k
chunk
flag
}

private int Wrap(int value, int max)
{ 
return (value % max + max) % max;
}

public void SetMap(Map map)
{ 
this.map = map;
}
}
}
