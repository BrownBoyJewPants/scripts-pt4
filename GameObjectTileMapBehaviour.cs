using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TileEngine.Batching;
using TileEngine.TileMap;
using UnityEngine;


namespace TileEngine.MonoBehaviours
{ 
public class GameObjectTileMapBehaviour : MonoBehaviour, ITileMapBehaviour
{ 
public int VisibleLevel;

public Material material;

public Material invisibleMaterial;

private Map map;

private Dictionary<Chunk, MeshFilter> filtersByChunk;

private Stopwatch sw;

Material ITileMapBehaviour.Material
{ 
get
{ 
return this.material;
}
}

void ITileMapBehaviour.SetMap(Map map)
{ 
this.map = map;
this.BuildMap();
}

private void Awake()
{ 
ChunkManager.usePhysicsObjects = true;
}

private Quaternion GetRotation(TileFacing fromFacing, TileFacing toFacing)
{ 
return Quaternion.Euler(0f, (float)(toFacing - fromFacing) * 90f, 0f);
}

private void Start()
{ 
this.sw = new Stopwatch();
this.CreateCollider(-1, -1, this.map.Dx + 1, 0);
this.CreateCollider(-1, -1, 0, this.map.Dz + 1);
this.CreateCollider(this.map.Dx, 0, this.map.Dx + 1, this.map.Dz);
this.CreateCollider(0, this.map.Dz, this.map.Dx + 1, this.map.Dz + 1);
}

private void CreateCollider(int x1, int y1, int x2, int y2)
{ 
GameObject gameObject = new GameObject();
BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
boxCollider.center = new Vector3((float)(x1 + x2) * 0.5f, 1.5f, (float)(y1 + y2) * 0.5f);
boxCollider.size = new Vector3((float)(x2 - x1), 2f, (float)(y2 - y1));
boxCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
gameObject
boxCollider
}

private void Update()
{ 
if (this.map == null)
{ 
return;
}
ChunkManager chunks = this.map.Chunks;
Camera main = Camera.main;
Vector3 vector = CameraBase.ViewportToGround(main, Vector2.zero);
Vector3 vector2 = CameraBase.ViewportToGround(main, Vector2.one);
Vector3 vector3 = CameraBase.ViewportToGround(main, new Vector2(0f, 1f));
Vector3 vector4 = CameraBase.ViewportToGround(main, new Vector2(1f, 0f));
int num = 1;
Plane[] array = GeometryUtility.CalculateFrustumPlanes(Camera.main);
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
this.sw.Reset();
this.sw.Start();
for (int i = num4; i < num5; i++)
{ 
for (int j = 0; j < chunks.Dy; j++)
{ 
for (int k = num2; k < num3; k++)
{ 
Chunk chunk = chunks[k, j, i];
if (chunk.dirty)
{ 
this.filtersByChunk[chunk].sharedMesh = chunk.Mesh;
}
if (this.sw.ElapsedMilliseconds > 15L)
{ 
this.sw.Stop();
return;
}
}
}
}
this.sw.Stop();
chunks
main
vector
vector2
vector3
vector4
num
array
num2
num3
num4
num5
i
j
k
chunk
}

private void BuildMap()
{ 
HashSet<int> hashSet = new HashSet<int>();
Stopwatch stopwatch = Stopwatch.StartNew();
this.filtersByChunk = new Dictionary<Chunk, MeshFilter>();
GameObject gameObject = new GameObject("MapRoot");
gameObject.transform.position = Vector3.zero;
foreach (Chunk current in this.map.Chunks.Enumerate)
{ 
GameObject gameObject2 = new GameObject(current.ToString());
gameObject2.transform.parent = gameObject.transform;
gameObject2.isStatic = true;
gameObject2.transform.localPosition = Vector3.zero;
this.CreateColliders(gameObject2, current);
MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
meshFilter.sharedMesh = current.Mesh;
this.filtersByChunk[current] = meshFilter;
MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
meshRenderer.sharedMaterial = ((current.y1 <= this.VisibleLevel) ? this.material : this.invisibleMaterial);
hashSet.Clear();
Tile[] physicsObjects = current.physicsObjects;
Tile tile;
for (int i = 0; i < physicsObjects.Length; i++)
{ 
tile = physicsObjects[i];
tile.cell.RemoveTile(tile);
tile.cell = null;
if (tile.groupId == 0 || hashSet.Add(tile.groupId))
{ 
GameObject gameObject3 = new GameObject(tile.ToString());
gameObject3.transform.parent = gameObject2.transform;
gameObject3.transform.position = tile.bounds.center;
meshRenderer = gameObject3.AddComponent<MeshRenderer>();
meshRenderer.sharedMaterial = this.material;
meshFilter = gameObject3.AddComponent<MeshFilter>();
if (tile.groupId == 0)
{ 
meshFilter.sharedMesh = tile.GetMesh(false);
}
else
{ 
meshFilter.sharedMesh = this.GetGroupedMesh((from p in current.physicsObjects
where p.groupId == tile.groupId
select p).ToArray<Tile>(), tile.bounds.center);
}
Rigidbody rigidbody = gameObject3.AddComponent<Rigidbody>();
rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
rigidbody.mass = 20f;
rigidbody.drag = 0.5f;
rigidbody.Sleep();
MeshCollider meshCollider = gameObject3.AddComponent<MeshCollider>();
meshCollider.sharedMesh = meshFilter.sharedMesh;
SphereCollider sphereCollider = gameObject3.AddComponent<SphereCollider>();
sphereCollider.radius = Mathf.Min(new float[]
{ 
tile.bounds.size.x,
tile.bounds.size.y,
tile.bounds.size.z
}) * 0.4f;
}
}
}
UnityEngine.Debug.Log("Time to build map: " + stopwatch.ElapsedMilliseconds);
hashSet
stopwatch
gameObject
enumerator
current
gameObject2
meshFilter
meshRenderer
<BuildMap>c__AnonStorey2D
physicsObjects
i
gameObject3
rigidbody
meshCollider
sphereCollider
}

private void CreateColliders(GameObject chunkObj, Chunk chunk)
{ 
for (int i = chunk.z1; i <= chunk.z2; i++)
{ 
for (int j = chunk.y1; j <= chunk.y2; j++)
{ 
for (int k = chunk.x1; k <= chunk.x2; k++)
{ 
foreach (Tile current in this.map[k, j, i].Enumerate)
{ 
if ((current.flags & TileFlags.IgnoreBlockingInfo) == TileFlags.None)
{ 
current.collider = new GameObject("Collider " + current.resource.name)
{ 
transform = 
{ 
parent = chunkObj.transform,
localPosition = current.bounds.center
},
layer = 2
}.AddComponent<BoxCollider>();
current.UpdateCollider();
}
}
}
}
}
i
j
k
enumerator
current
}

private Mesh GetGroupedMesh(Tile[] tiles, Vector3 offset)
{ 
Mesh mesh = new Mesh();
CombineInstance[] array = new CombineInstance[tiles.Length];
for (int i = 0; i < array.Length; i++)
{ 
array[i] = new CombineInstance
{ 
mesh = tiles[i].GetMesh(false),
transform = Matrix4x4.TRS(tiles[i].bounds.center - offset, Quaternion.identity, Vector3.one)
};
}
mesh.CombineMeshes(array);
return mesh;
mesh
array
i
}
}
}
