using System;
using TileEngine.TileMap;
using UnityEngine;


namespace TileEngine.MonoBehaviours
{ 
[ExecuteInEditMode]
public class TileTemplateBehaviour : MonoBehaviour
{ 
public Material material;

public Material invisibleMaterial;

private Map map;

private Camera lastCamera;

public void SetMap(Map map)
{ 
this.map = map;
this.UpdateObjects();
}

private void UpdateObjects()
{ 
Transform transform = base.transform.Find("Cells");
if (transform != null)
{ 
UnityEngine.Object.DestroyImmediate(transform);
}
transform = new GameObject("Cells").transform;
transform.parent = base.transform;
for (int i = 0; i < this.map.Dx; i++)
{ 
for (int j = 0; j < 2; j++)
{ 
for (int k = 0; k < this.map.Dz; k++)
{ 
GameObject gameObject = new GameObject(string.Concat(new object[]
{ 
i,
",",
j,
",",
k
}));
gameObject.transform.parent = transform;
gameObject.transform.position = new Vector3((float)i, (float)j, (float)k);
foreach (Tile current in this.map[i, j, k].Enumerate)
{ 
GameObject gameObject2 = new GameObject(current.sharedResource.name);
gameObject2.transform.parent = gameObject.transform;
gameObject2.transform.position = current.bounds.center;
MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
meshFilter.mesh = current.sharedResource.mesh;
MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
meshRenderer.material = this.material;
}
}
}
}
transform
i
j
k
gameObject
enumerator
current
gameObject2
meshFilter
meshRenderer
}
}
}
