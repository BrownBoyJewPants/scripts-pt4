using SimplexNoise;
using System;
using TileEngine;
using TileEngine.Particles;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine.TileEngine.Generators;


public class RandomOutDoorWinterMap
{ 
private VoxelResource ground;

private TemplateResourceManager templates;

private VoxelResource[] trees;

private VoxelResource[] flowers;

private VoxelResource[] treetops;

private VoxelResource[] grass;

private Map map;

private Vector2 ufoLocation;

private int nxo;

private int nzo;

private RandomOutDoorWinterMap(VoxelResourceManager resources, TemplateResourceManager templates)
{ 
this.templates = templates;
this.nxo = UnityEngine.Random.Range(0, 10000);
this.nzo = UnityEngine.Random.Range(0, 10000);
this.ground = resources.GetResource("ground", TileFacing.None);
this.grass = new VoxelResource[]
{ 
resources.GetResource("flowergrass", TileFacing.East),
resources.GetResource("flowergrass", TileFacing.West),
resources.GetResource("flowergrass", TileFacing.South),
resources.GetResource("flowergrass", TileFacing.North)
};
this.flowers = new VoxelResource[]
{ 
resources.GetResource("flowerred", TileFacing.None),
resources.GetResource("flowerwhite", TileFacing.None),
resources.GetResource("floweryellow", TileFacing.None)
};
this.trees = new VoxelResource[]
{ 
resources.GetResource("tree", TileFacing.North, TileMirror.None, true, new TileFacing?(TileFacing.North), 1f),
resources.GetResource("tree", TileFacing.South, TileMirror.None, true, new TileFacing?(TileFacing.North), 1f),
resources.GetResource("tree", TileFacing.East, TileMirror.None, true, new TileFacing?(TileFacing.North), 1f),
resources.GetResource("tree", TileFacing.West, TileMirror.None, true, new TileFacing?(TileFacing.North), 1f)
};
this.treetops = new VoxelResource[]
{ 
resources.GetResource("treetop", TileFacing.North, TileMirror.None, true, new TileFacing?(TileFacing.North), 2.3f),
resources.GetResource("treetop", TileFacing.South, TileMirror.None, true, new TileFacing?(TileFacing.North), 2.3f),
resources.GetResource("treetop", TileFacing.East, TileMirror.None, true, new TileFacing?(TileFacing.North), 2.3f),
resources.GetResource("treetop", TileFacing.West, TileMirror.None, true, new TileFacing?(TileFacing.North), 2.3f)
};
}

public static Map Create(VoxelResourceManager resources, TemplateResourceManager templates)
{ 
return new RandomOutDoorWinterMap(resources, templates).Create();
}

public Map Create()
{ 
if (TEParticleSystem.IsAndroid)
{ 
this.map = new Map(75, 2, 75);
}
else
{ 
this.map = new Map(100, 2, 100);
}
TemplateAsset resource = this.templates.GetResource("UfoSmall");
int i;
int num;
do
{ 
i = UnityEngine.Random.Range(2, this.map.Dz - 5 - resource.dz);
num = UnityEngine.Random.Range(this.map.Dx - 50 - resource.dx, this.map.Dx - 5 - resource.dx);
}
while (!this.map.IsEmptyRegion(num - 2, 0, i - 2, resource.dx + 4, 2, resource.dz + 4));
this.templates.CopyToMap(this.map, resource, 0, new Vector3((float)num, 0f, (float)i));
this.map.UfoCoreLocation = this.map[num + 4, 1, i + 4];
this.ufoLocation = new Vector2((float)(num + 4), (float)(i + 4));
int num2 = this.map.Dx * this.map.Dz / 20;
for (int j = 0; j < num2; j++)
{ 
int num3 = UnityEngine.Random.Range(3, this.map.Dx - 3);
i = UnityEngine.Random.Range(3, this.map.Dz - 3);
int num4 = this.map.Dx / 2;
int num5 = this.map.Dz / 2;
if (Mathf.Abs(num4 - num3) >= 3 && Mathf.Abs(num5 - i) >= 3)
{ 
if (this.map.IsEmptyRegion(num3 - 3, 0, i - 3, 6, 2, 6))
{ 
this.CreateTree(num3, i);
}
}
}
float num6 = GenerationSettings.decoration * 0.1f;
float num7 = GenerationSettings.decoration * 0.2f;
for (int k = 0; k < this.map.Dx; k++)
{ 
for (i = 0; i < this.map.Dz; i++)
{ 
if (this.map[k, 0, i].TileCount == 0)
{ 
float num8 = this.pnoise(k, i);
this.map[k, 0, i].AddTile(this.ground, new Vector3?(new Vector3(0f, num8, 0f)), true);
if (UnityEngine.Random.Range(0f, 1f) <= num6)
{ 
Vector3 value = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), num8 + UnityEngine.Random.Range(-0.1f, 0f), UnityEngine.Random.Range(-0.5f, 0.5f));
Tile tile = this.map[k, 1, i].AddTile(this.flowers[UnityEngine.Random.Range(0, this.flowers.Length)], new Vector3?(value), false);
}
if (UnityEngine.Random.Range(0f, 1f) <= num7)
{ 
Vector3 value2 = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), num8, UnityEngine.Random.Range(-0.5f, 0.5f));
Tile tile2 = this.map[k, 1, i].AddTile(this.grass[UnityEngine.Random.Range(0, this.grass.Length)], new Vector3?(value2), false);
}
}
}
}
return this.map;
resource
i
num
num2
j
num3
num4
num5
num6
num7
k
num8
value
tile
value2
tile2
}

private float pnoise(int x, int z)
{ 
float magnitude = (this.ufoLocation - new Vector2((float)x, (float)z)).magnitude;
x += this.nxo;
z += this.nzo;
float num = (Noise.Generate((float)x / 20f, (float)z / 20f) - 1f) * 0.4f;
if (magnitude < 12f)
{ 
num = Mathf.Lerp(0f, num, Mathf.Max(0f, magnitude - 4f) / 8f);
}
return num;
magnitude
num
}

private void CreateTree(int x, int z)
{ 
Vector3 value = new Vector3(0f, this.pnoise(x, z), 0f);
int num = UnityEngine.Random.Range(0, this.trees.Length);
this.map[x, 1, z].AddTile(this.trees[num], new Vector3?(value), true);
value
num
}
}
