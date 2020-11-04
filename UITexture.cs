using System;
using UnityEngine;


[AddComponentMenu("NGUI/UI/Texture"), ExecuteInEditMode]
public class UITexture : UIWidget
{ 
[HideInInspector, SerializeField]
private Rect mRect = new Rect(0f, 0f, 1f, 1f);

[HideInInspector, SerializeField]
private Shader mShader;

[HideInInspector, SerializeField]
private Texture mTexture;

[HideInInspector, SerializeField]
private Material mMat;

private bool mCreatingMat;

private Material mDynamicMat;

private int mPMA = -1;

public Rect uvRect
{ 
get
{ 
return this.mRect;
}
set
{ 
if (this.mRect != value)
{ 
this.mRect = value;
this.MarkAsChanged();
}
}
}

public Shader shader
{ 
get
{ 
if (this.mShader == null)
{ 
Material material = this.material;
if (material != null)
{ 
this.mShader = material.shader;
}
if (this.mShader == null)
{ 
this.mShader = Shader.Find("Unlit/Transparent Colored");
}
}
return this.mShader;
material
}
set
{ 
if (this.mShader != value)
{ 
this.mShader = value;
Material material = this.material;
if (material != null)
{ 
material.shader = value;
}
this.mPMA = -1;
}
material
}
}

public bool hasDynamicMaterial
{ 
get
{ 
return this.mDynamicMat != null;
}
}

public override Material material
{ 
get
{ 
if (this.mMat != null)
{ 
return this.mMat;
}
if (this.mDynamicMat != null)
{ 
return this.mDynamicMat;
}
if (!this.mCreatingMat && this.mDynamicMat == null)
{ 
this.mCreatingMat = true;
if (this.mShader == null)
{ 
this.mShader = Shader.Find("Unlit/Texture");
}
this.Cleanup();
this.mDynamicMat = new Material(this.mShader);
this.mDynamicMat.hideFlags = HideFlags.DontSave;
this.mDynamicMat.mainTexture = this.mTexture;
this.mPMA = 0;
this.mCreatingMat = false;
}
return this.mDynamicMat;
}
set
{ 
if (this.mMat != value)
{ 
this.Cleanup();
this.mMat = value;
this.mPMA = -1;
this.MarkAsChanged();
}
}
}

public bool premultipliedAlpha
{ 
get
{ 
if (this.mPMA == -1)
{ 
Material material = this.material;
this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
}
return this.mPMA == 1;
material
}
}

public override Texture mainTexture
{ 
get
{ 
if (this.mMat != null)
{ 
return this.mMat.mainTexture;
}
if (this.mTexture != null)
{ 
return this.mTexture;
}
return null;
}
set
{ 
base.RemoveFromPanel();
Material material = this.material;
if (material != null)
{ 
this.mPanel = null;
this.mTexture = value;
material.mainTexture = value;
if (base.enabled)
{ 
base.CreatePanel();
}
}
material
}
}

private void OnDestroy()
{ 
this.Cleanup();
}

private void Cleanup()
{ 
if (this.mDynamicMat != null)
{ 
NGUITools.Destroy(this.mDynamicMat);
this.mDynamicMat = null;
}
}

public override void MakePixelPerfect()
{ 
Texture mainTexture = this.mainTexture;
if (mainTexture != null)
{ 
Vector3 localScale = base.cachedTransform.localScale;
localScale.x = (float)mainTexture.width * this.uvRect.width;
localScale.y = (float)mainTexture.height * this.uvRect.height;
localScale.z = 1f;
base.cachedTransform.localScale = localScale;
}
base.MakePixelPerfect();
mainTexture
localScale
}

public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
{ 
Color color = base.color;
color.a *= this.mPanel.alpha;
Color32 item = (!this.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
verts.Add(new Vector3(1f, 0f, 0f));
verts.Add(new Vector3(1f, -1f, 0f));
verts.Add(new Vector3(0f, -1f, 0f));
verts.Add(new Vector3(0f, 0f, 0f));
uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMax));
uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMin));
uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMin));
uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMax));
cols.Add(item);
cols.Add(item);
cols.Add(item);
cols.Add(item);
color
item
}
}
