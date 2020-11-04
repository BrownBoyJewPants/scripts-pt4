using System;
using UnityEngine;


[AddComponentMenu("NGUI/Internal/Draw Call"), ExecuteInEditMode]
public class UIDrawCall : MonoBehaviour
{ 
public enum Clipping
{ 
None,
HardClip,
AlphaClip,
SoftClip
}

private Transform mTrans;

private Material mSharedMat;

private Mesh mMesh0;

private Mesh mMesh1;

private MeshFilter mFilter;

private MeshRenderer mRen;

private UIDrawCall.Clipping mClipping;

private Vector4 mClipRange;

private Vector2 mClipSoft;

private Material mMat;

private Material mDepthMat;

private int[] mIndices;

private bool mUseDepth;

private bool mReset = true;

private bool mEven = true;

private int mDepth;

public bool depthPass
{ 
get
{ 
return this.mUseDepth;
}
set
{ 
if (this.mUseDepth != value)
{ 
this.mUseDepth = value;
this.mReset = true;
}
}
}

public int depth
{ 
get
{ 
return this.mDepth;
}
set
{ 
if (this.mDepth != value)
{ 
this.mDepth = value;
if (this.mMat != null && this.mSharedMat != null)
{ 
this.mMat.renderQueue = this.mSharedMat.renderQueue + value;
}
}
}
}

public Transform cachedTransform
{ 
get
{ 
if (this.mTrans == null)
{ 
this.mTrans = base.transform;
}
return this.mTrans;
}
}

public Material material
{ 
get
{ 
return this.mSharedMat;
}
set
{ 
this.mSharedMat = value;
}
}

public Texture mainTexture
{ 
get
{ 
return this.mMat.mainTexture;
}
set
{ 
this.mMat.mainTexture = value;
}
}

public int triangles
{ 
get
{ 
Mesh mesh = (!this.mEven) ? this.mMesh1 : this.mMesh0;
return (!(mesh != null)) ? 0 : (mesh.vertexCount >> 1);
mesh
}
}

public bool isClipped
{ 
get
{ 
return this.mClipping != UIDrawCall.Clipping.None;
}
}

public UIDrawCall.Clipping clipping
{ 
get
{ 
return this.mClipping;
}
set
{ 
if (this.mClipping != value)
{ 
this.mClipping = value;
this.mReset = true;
}
}
}

public Vector4 clipRange
{ 
get
{ 
return this.mClipRange;
}
set
{ 
this.mClipRange = value;
}
}

public Vector2 clipSoftness
{ 
get
{ 
return this.mClipSoft;
}
set
{ 
this.mClipSoft = value;
}
}

private Mesh GetMesh(ref bool rebuildIndices, int vertexCount)
{ 
this.mEven = !this.mEven;
if (this.mEven)
{ 
if (this.mMesh0 == null)
{ 
this.mMesh0 = new Mesh();
this.mMesh0.hideFlags = HideFlags.DontSave;
this.mMesh0.name = "Mesh0 for " + this.mSharedMat.name;
this.mMesh0.MarkDynamic();
rebuildIndices = true;
}
else if (rebuildIndices || this.mMesh0.vertexCount != vertexCount)
{ 
rebuildIndices = true;
this.mMesh0.Clear();
}
return this.mMesh0;
}
if (this.mMesh1 == null)
{ 
this.mMesh1 = new Mesh();
this.mMesh1.hideFlags = HideFlags.DontSave;
this.mMesh1.name = "Mesh1 for " + this.mSharedMat.name;
this.mMesh1.MarkDynamic();
rebuildIndices = true;
}
else if (rebuildIndices || this.mMesh1.vertexCount != vertexCount)
{ 
rebuildIndices = true;
this.mMesh1.Clear();
}
return this.mMesh1;
}

private void UpdateMaterials()
{ 
bool flag = this.mClipping != UIDrawCall.Clipping.None;
if (this.mMat == null)
{ 
this.mMat = new Material(this.mSharedMat);
this.mMat.hideFlags = HideFlags.DontSave;
this.mMat.CopyPropertiesFromMaterial(this.mSharedMat);
this.mMat.renderQueue = this.mSharedMat.renderQueue + this.mDepth;
}
if (flag && this.mClipping != UIDrawCall.Clipping.None)
{ 
Shader shader = null;
string text = this.mSharedMat.shader.name;
text = text.Replace(" (AlphaClip)", string.Empty);
text = text.Replace(" (SoftClip)", string.Empty);
if (this.mClipping == UIDrawCall.Clipping.HardClip || this.mClipping == UIDrawCall.Clipping.AlphaClip)
{ 
shader = Shader.Find(text + " (AlphaClip)");
}
else if (this.mClipping == UIDrawCall.Clipping.SoftClip)
{ 
shader = Shader.Find(text + " (SoftClip)");
}
if (shader != null)
{ 
this.mMat.shader = shader;
}
else
{ 
this.mClipping = UIDrawCall.Clipping.None;
Debug.LogError(text + " doesn't have a clipped shader version for " + this.mClipping);
}
}
if (this.mUseDepth)
{ 
if (this.mDepthMat == null)
{ 
Shader shader2 = Shader.Find("Unlit/Depth Cutout");
this.mDepthMat = new Material(shader2);
this.mDepthMat.hideFlags = HideFlags.DontSave;
}
this.mDepthMat.mainTexture = this.mSharedMat.mainTexture;
}
else if (this.mDepthMat != null)
{ 
NGUITools.Destroy(this.mDepthMat);
this.mDepthMat = null;
}
if (this.mDepthMat != null)
{ 
if (this.mRen.sharedMaterials != null && this.mRen.sharedMaterials.Length == 2 && this.mRen.sharedMaterials[1] == this.mMat)
{ 
return;
}
this.mRen.sharedMaterials = new Material[]
{ 
this.mDepthMat,
this.mMat
};
}
else if (this.mRen.sharedMaterial != this.mMat)
{ 
this.mRen.sharedMaterials = new Material[]
{ 
this.mMat
};
}
flag
shader
text
shader2
}

public void Set(BetterList<Vector3> verts, BetterList<Vector3> norms, BetterList<Vector4> tans, BetterList<Vector2> uvs, BetterList<Color32> cols)
{ 
int size = verts.size;
if (size > 0 && size == uvs.size && size == cols.size && size % 4 == 0)
{ 
if (this.mFilter == null)
{ 
this.mFilter = base.gameObject.GetComponent<MeshFilter>();
}
if (this.mFilter == null)
{ 
this.mFilter = base.gameObject.AddComponent<MeshFilter>();
}
if (this.mRen == null)
{ 
this.mRen = base.gameObject.GetComponent<MeshRenderer>();
}
if (this.mRen == null)
{ 
this.mRen = base.gameObject.AddComponent<MeshRenderer>();
this.UpdateMaterials();
}
else if (this.mMat != null && this.mMat.mainTexture != this.mSharedMat.mainTexture)
{ 
this.UpdateMaterials();
}
if (verts.size < 65000)
{ 
int num = (size >> 1) * 3;
bool flag = this.mIndices == null || this.mIndices.Length != num;
if (flag)
{ 
this.mIndices = new int[num];
int num2 = 0;
for (int i = 0; i < size; i += 4)
{ 
this.mIndices[num2++] = i;
this.mIndices[num2++] = i + 1;
this.mIndices[num2++] = i + 2;
this.mIndices[num2++] = i + 2;
this.mIndices[num2++] = i + 3;
this.mIndices[num2++] = i;
}
}
Mesh mesh = this.GetMesh(ref flag, verts.size);
mesh.vertices = verts.ToArray();
if (norms != null)
{ 
mesh.normals = norms.ToArray();
}
if (tans != null)
{ 
mesh.tangents = tans.ToArray();
}
mesh.uv = uvs.ToArray();
mesh.colors32 = cols.ToArray();
if (flag)
{ 
mesh.triangles = this.mIndices;
}
mesh.RecalculateBounds();
this.mFilter.mesh = mesh;
}
else
{ 
if (this.mFilter.mesh != null)
{ 
this.mFilter.mesh.Clear();
}
Debug.LogError("Too many vertices on one panel: " + verts.size);
}
}
else
{ 
if (this.mFilter.mesh != null)
{ 
this.mFilter.mesh.Clear();
}
Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size);
}
size
num
flag
num2
i
mesh
}

private void OnWillRenderObject()
{ 
if (this.mReset)
{ 
this.mReset = false;
this.UpdateMaterials();
}
if (this.mMat != null && this.isClipped)
{ 
this.mMat.mainTextureOffset = new Vector2(-this.mClipRange.x / this.mClipRange.z, -this.mClipRange.y / this.mClipRange.w);
this.mMat.mainTextureScale = new Vector2(1f / this.mClipRange.z, 1f / this.mClipRange.w);
Vector2 v = new Vector2(1000f, 1000f);
if (this.mClipSoft.x > 0f)
{ 
v.x = this.mClipRange.z / this.mClipSoft.x;
}
if (this.mClipSoft.y > 0f)
{ 
v.y = this.mClipRange.w / this.mClipSoft.y;
}
this.mMat.SetVector("_ClipSharpness", v);
}
v
}

private void OnDestroy()
{ 
NGUITools.DestroyImmediate(this.mMesh0);
NGUITools.DestroyImmediate(this.mMesh1);
NGUITools.DestroyImmediate(this.mMat);
NGUITools.DestroyImmediate(this.mDepthMat);
}
}
