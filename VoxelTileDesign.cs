using System;
using System.Runtime.CompilerServices;
using TileEngine;
using UnityEngine;
using VoxelEngine;
using VoxelEngine.Renderers;
using VoxelEngine.VoxelDataStructures;


[ExecuteInEditMode, RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider))]
public class VoxelTileDesign : MonoBehaviour
{ 
private Mesh mesh;

private MeshFilter meshfilter;

private new SimpleRenderBuffer renderer;

private GreedySurfaceExtractor extractor;

[NonSerialized]
public VoxelAssetPart selectedPart;

public Material gameMaterial;

[NonSerialized]
public VoxelData data;

[NonSerialized]
public new MeshCollider collider;

public VoxelAsset Asset;

[HideInInspector]
public int dx;

[HideInInspector]
public int dy;

[HideInInspector]
public int dz;

private VoxelAsset prevAsset;

[NonSerialized]
public Vector3? mouseOverBlock;

[NonSerialized]
public Box? selection;

[NonSerialized]
public Vector3? selectionHit;

[NonSerialized]
public BlockFace selectionFace;

[NonSerialized]
public Vector3? mouseDownBlock;

[NonSerialized]
public float scale = 1f;

public event EventHandler SelectedAssetChanged
{ 
[MethodImpl(MethodImplOptions.Synchronized)]
add
{ 
this.SelectedAssetChanged = (EventHandler)Delegate.Combine(this.SelectedAssetChanged, value);
}
[MethodImpl(MethodImplOptions.Synchronized)]
remove
{ 
this.SelectedAssetChanged = (EventHandler)Delegate.Remove(this.SelectedAssetChanged, value);
}
}

public MeshFilter MeshFilter
{ 
get
{ 
return this.meshfilter;
}
}

public VoxelTileDesign()
{ 
this.dx = (this.dy = (this.dz = 32));
}

private void OnEnable()
{ 
this.meshfilter = base.GetComponent<MeshFilter>();
this.collider = base.GetComponent<MeshCollider>();
this.mesh = new Mesh();
this.meshfilter.mesh = this.mesh;
this.prevAsset = null;
this.Rebuild();
}

private void Start()
{ 
if (Application.isPlaying)
{ 
base.GetComponent<MeshRenderer>().material = this.gameMaterial;
}
}

public void Rebuild()
{ 
if (this.Asset != this.prevAsset || this.data == null)
{ 
if (this.Asset == null)
{ 
this.mesh.Clear();
this.data = null;
}
else
{ 
this.scale = 1f / (float)Mathf.Max(this.Asset.dx, this.Asset.dz);
this.renderer = new SimpleRenderBuffer(true, true);
this.renderer.SetScale(this.scale);
this.SetData(this.Asset.ToVoxelData(0, TileMirror.None));
if (this.SelectedAssetChanged != null)
{ 
this.SelectedAssetChanged(this, new EventArgs());
}
}
this.prevAsset = this.Asset;
}
}

public void UpdateMesh()
{ 
this.data.UpdateBounds(true);
this.extractor.ExtractSurface(this.renderer, this.data, false, null);
this.renderer.UpdateMesh(this.mesh);
if (this.collider != null)
{ 
this.collider.sharedMesh = null;
this.collider.sharedMesh = this.meshfilter.sharedMesh;
this.collider.smoothSphereCollisions = true;
}
}

private void Update()
{ 
if (this.Asset != this.prevAsset)
{ 
this.Rebuild();
}
}

public void Commit()
{ 
}

public void Clear()
{ 
this.data.Clear();
this.UpdateMesh();
}

private void DoAction(Box bounds, Action<int, int, int> action)
{ 
Box box = bounds.Fix();
int num = (int)box.Min.x;
int num2 = (int)box.Max.x;
int num3 = (int)box.Min.y;
int num4 = (int)box.Max.y;
int num5 = (int)box.Min.z;
int num6 = (int)box.Max.z;
for (int i = num5; i <= num6; i++)
{ 
for (int j = num3; j <= num4; j++)
{ 
for (int k = num; k <= num2; k++)
{ 
action(k, j, i);
}
}
}
this.UpdateMesh();
box
num
num2
num3
num4
num5
num6
i
j
k
}

public void FillSelection(int color)
{ 
this.DoAction(this.selection.Value, delegate(int x, int y, int z)
{ 
this.data.SetData(x, y, z, color);
});
<FillSelection>c__AnonStorey1A
}

public void ClearSelection()
{ 
this.DoAction(this.selection.Value, delegate(int x, int y, int z)
{ 
this.data.SetData(x, y, z, 0);
});
}

public void PaintSelection(int color)
{ 
this.DoAction(this.selection.Value, delegate(int x, int y, int z)
{ 
if (this.data.GetData(x, y, z) != 0)
{ 
this.data.SetData(x, y, z, color);
}
});
<PaintSelection>c__AnonStorey1B
}

private void SetData(VoxelData data)
{ 
this.data = data;
this.dx = data.Dx;
this.dy = data.Dy;
this.dz = data.Dz;
this.extractor = new GreedySurfaceExtractor(this.dx, this.dy, this.dz);
this.UpdateMesh();
}

public void Resize(int dx, int dy, int dz)
{ 
ArrayVoxelData arrayVoxelData = new ArrayVoxelData(dx, dy, dz);
this.data.CopyTo(arrayVoxelData);
this.SetData(arrayVoxelData);
arrayVoxelData
}

public void Rescale(int dx, int dy, int dz)
{ 
ArrayVoxelData arrayVoxelData = new ArrayVoxelData(dx, dy, dz);
for (int i = 0; i < dy; i++)
{ 
for (int j = 0; j < dz; j++)
{ 
for (int k = 0; k < dx; k++)
{ 
arrayVoxelData.SetData(k, i, j, this.data.GetData(k * this.data.Dx / dx, i * this.data.Dy / dy, j * this.data.Dz / dz));
}
}
}
this.SetData(arrayVoxelData);
arrayVoxelData
i
j
k
}

public void Move(int ox, int oy, int oz)
{ 
ArrayVoxelData arrayVoxelData = new ArrayVoxelData(this.dx, this.dy, this.dz);
for (int i = 0; i < this.dy; i++)
{ 
for (int j = 0; j < this.dz; j++)
{ 
for (int k = 0; k < this.dx; k++)
{ 
arrayVoxelData.SetData(k, i, j, this.data.GetData((k + ox + this.dx) % this.dx, (i + oy + this.dy) % this.dy, (j + oz + this.dz) % this.dz));
}
}
}
this.SetData(arrayVoxelData);
arrayVoxelData
i
j
k
}

public void MoveSelection(float x, float y, float z)
{ 
this.selection = new Box?(new Box
{ 
Min = this.selection.Value.Min + new Vector3(x, y, z),
Max = this.selection.Value.Max + new Vector3(x, y, z)
});
}

public void CreatePart()
{ 
Box? box = this.selection;
if (!box.HasValue)
{ 
return;
}
Box box2 = this.selection.Value.Fix();
Array.Resize<VoxelAssetPart>(ref this.Asset.parts, this.Asset.parts.Length + 1);
this.Asset.parts[this.Asset.parts.Length - 1] = new VoxelAssetPart
{ 
name = "New part",
bounds = new IntRect(box2.Min, box2.Max)
};
box
box2
}

public void SelectPart(VoxelAssetPart part)
{ 
this.selection = new Box?(new Box
{ 
Min = new Vector3((float)part.bounds.x1, (float)part.bounds.y1, (float)part.bounds.z1),
Max = new Vector3((float)part.bounds.x2, (float)part.bounds.y2, (float)part.bounds.z2)
});
this.selectedPart = part;
}

public void UpdatePart(VoxelAssetPart part)
{ 
Box? box = this.selection;
if (!box.HasValue)
{ 
return;
}
Box box2 = this.selection.Value.Fix();
part.bounds = new IntRect(box2.Min, box2.Max);
box
box2
}

public void DeletePart(VoxelAssetPart part)
{ 
int num = -1;
for (int i = 0; i < this.Asset.parts.Length; i++)
{ 
if (this.Asset.parts[i] == part)
{ 
num = i;
break;
}
}
if (num == -1)
{ 
return;
}
VoxelAssetPart[] array = new VoxelAssetPart[this.Asset.parts.Length - 1];
for (int j = 0; j < array.Length; j++)
{ 
array[j] = this.Asset.parts[(j >= num) ? (j + 1) : j];
}
this.Asset.parts = array;
num
i
array
j
}

public void ReplaceColor(Color32 color, Color32 replaceColor)
{ 
int num = color.ToInt();
int num2 = replaceColor.ToInt();
for (int i = 0; i < this.dy; i++)
{ 
for (int j = 0; j < this.dz; j++)
{ 
for (int k = 0; k < this.dx; k++)
{ 
int num3 = this.data.GetData(k, i, j);
int num4 = num3 & 16777215;
int num5 = num3 & 251658240;
if (this.data.GetData(k, i, j) == num)
{ 
this.data.SetData(k, i, j, num2 | num5);
}
}
}
}
this.UpdateMesh();
num
num2
i
j
k
num3
num4
num5
}
}
