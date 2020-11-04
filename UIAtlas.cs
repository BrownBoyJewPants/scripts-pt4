using System;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{ 
[Serializable]
public class Sprite
{ 
public string name = "Unity Bug";

public Rect outer = new Rect(0f, 0f, 1f, 1f);

public Rect inner = new Rect(0f, 0f, 1f, 1f);

public bool rotated;

public float paddingLeft;

public float paddingRight;

public float paddingTop;

public float paddingBottom;

public bool hasPadding
{ 
get
{ 
return this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f || this.paddingBottom != 0f;
}
}
}

public enum Coordinates
{ 
Pixels,
TexCoords
}

[HideInInspector, SerializeField]
private Material material;

[HideInInspector, SerializeField]
private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

[HideInInspector, SerializeField]
private UIAtlas.Coordinates mCoordinates;

[HideInInspector, SerializeField]
private float mPixelSize = 1f;

[HideInInspector, SerializeField]
private UIAtlas mReplacement;

private int mPMA = -1;

public Material spriteMaterial
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.material : this.mReplacement.spriteMaterial;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.spriteMaterial = value;
}
else if (this.material == null)
{ 
this.mPMA = 0;
this.material = value;
}
else
{ 
this.MarkAsDirty();
this.mPMA = -1;
this.material = value;
this.MarkAsDirty();
}
}
}

public bool premultipliedAlpha
{ 
get
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.premultipliedAlpha;
}
if (this.mPMA == -1)
{ 
Material spriteMaterial = this.spriteMaterial;
this.mPMA = ((!(spriteMaterial != null) || !(spriteMaterial.shader != null) || !spriteMaterial.shader.name.Contains("Premultiplied")) ? 0 : 1);
}
return this.mPMA == 1;
spriteMaterial
}
}

public List<UIAtlas.Sprite> spriteList
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.sprites : this.mReplacement.spriteList;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.spriteList = value;
}
else
{ 
this.sprites = value;
}
}
}

public Texture texture
{ 
get
{ 
return (!(this.mReplacement != null)) ? ((!(this.material != null)) ? null : this.material.mainTexture) : this.mReplacement.texture;
}
}

public UIAtlas.Coordinates coordinates
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mCoordinates : this.mReplacement.coordinates;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.coordinates = value;
}
else if (this.mCoordinates != value)
{ 
if (this.material == null || this.material.mainTexture == null)
{ 
Debug.LogError("Can't switch coordinates until the atlas material has a valid texture");
return;
}
this.mCoordinates = value;
Texture mainTexture = this.material.mainTexture;
int i = 0;
int count = this.sprites.Count;
while (i < count)
{ 
UIAtlas.Sprite sprite = this.sprites[i];
if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
{ 
sprite.outer = NGUIMath.ConvertToTexCoords(sprite.outer, mainTexture.width, mainTexture.height);
sprite.inner = NGUIMath.ConvertToTexCoords(sprite.inner, mainTexture.width, mainTexture.height);
}
else
{ 
sprite.outer = NGUIMath.ConvertToPixels(sprite.outer, mainTexture.width, mainTexture.height, true);
sprite.inner = NGUIMath.ConvertToPixels(sprite.inner, mainTexture.width, mainTexture.height, true);
}
i++;
}
}
mainTexture
i
count
sprite
}
}

public float pixelSize
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mPixelSize : this.mReplacement.pixelSize;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.pixelSize = value;
}
else
{ 
float num = Mathf.Clamp(value, 0.25f, 4f);
if (this.mPixelSize != num)
{ 
this.mPixelSize = num;
this.MarkAsDirty();
}
}
num
}
}

public UIAtlas replacement
{ 
get
{ 
return this.mReplacement;
}
set
{ 
UIAtlas uIAtlas = value;
if (uIAtlas == this)
{ 
uIAtlas = null;
}
if (this.mReplacement != uIAtlas)
{ 
if (uIAtlas != null && uIAtlas.replacement == this)
{ 
uIAtlas.replacement = null;
}
if (this.mReplacement != null)
{ 
this.MarkAsDirty();
}
this.mReplacement = uIAtlas;
this.MarkAsDirty();
}
uIAtlas
}
}

public UIAtlas.Sprite GetSprite(string name)
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.GetSprite(name);
}
if (!string.IsNullOrEmpty(name))
{ 
int i = 0;
int count = this.sprites.Count;
while (i < count)
{ 
UIAtlas.Sprite sprite = this.sprites[i];
if (!string.IsNullOrEmpty(sprite.name) && name == sprite.name)
{ 
return sprite;
}
i++;
}
}
return null;
i
count
sprite
}

private static int CompareString(string a, string b)
{ 
return a.CompareTo(b);
}

public BetterList<string> GetListOfSprites()
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.GetListOfSprites();
}
BetterList<string> betterList = new BetterList<string>();
int i = 0;
int count = this.sprites.Count;
while (i < count)
{ 
UIAtlas.Sprite sprite = this.sprites[i];
if (sprite != null && !string.IsNullOrEmpty(sprite.name))
{ 
betterList.Add(sprite.name);
}
i++;
}
return betterList;
betterList
i
count
sprite
}

public BetterList<string> GetListOfSprites(string match)
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.GetListOfSprites(match);
}
if (string.IsNullOrEmpty(match))
{ 
return this.GetListOfSprites();
}
BetterList<string> betterList = new BetterList<string>();
int i = 0;
int count = this.sprites.Count;
while (i < count)
{ 
UIAtlas.Sprite sprite = this.sprites[i];
if (sprite != null && !string.IsNullOrEmpty(sprite.name) && string.Equals(match, sprite.name, StringComparison.OrdinalIgnoreCase))
{ 
betterList.Add(sprite.name);
return betterList;
}
i++;
}
string[] array = match.Split(new char[]
{ 
' '
}, StringSplitOptions.RemoveEmptyEntries);
for (int j = 0; j < array.Length; j++)
{ 
array[j] = array[j].ToLower();
}
int k = 0;
int count2 = this.sprites.Count;
while (k < count2)
{ 
UIAtlas.Sprite sprite2 = this.sprites[k];
if (sprite2 != null && !string.IsNullOrEmpty(sprite2.name))
{ 
string text = sprite2.name.ToLower();
int num = 0;
for (int l = 0; l < array.Length; l++)
{ 
if (text.Contains(array[l]))
{ 
num++;
}
}
if (num == array.Length)
{ 
betterList.Add(sprite2.name);
}
}
k++;
}
return betterList;
betterList
i
count
sprite
array
j
k
count2
sprite2
text
num
l
}

private bool References(UIAtlas atlas)
{ 
return !(atlas == null) && (atlas == this || (this.mReplacement != null && this.mReplacement.References(atlas)));
}

public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
{ 
return !(a == null) && !(b == null) && (a == b || a.References(b) || b.References(a));
}

public void MarkAsDirty()
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.MarkAsDirty();
}
UISprite[] array = NGUITools.FindActive<UISprite>();
int i = 0;
int num = array.Length;
while (i < num)
{ 
UISprite uISprite = array[i];
if (UIAtlas.CheckIfRelated(this, uISprite.atlas))
{ 
UIAtlas atlas = uISprite.atlas;
uISprite.atlas = null;
uISprite.atlas = atlas;
}
i++;
}
UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
int j = 0;
int num2 = array2.Length;
while (j < num2)
{ 
UIFont uIFont = array2[j];
if (UIAtlas.CheckIfRelated(this, uIFont.atlas))
{ 
UIAtlas atlas2 = uIFont.atlas;
uIFont.atlas = null;
uIFont.atlas = atlas2;
}
j++;
}
UILabel[] array3 = NGUITools.FindActive<UILabel>();
int k = 0;
int num3 = array3.Length;
while (k < num3)
{ 
UILabel uILabel = array3[k];
if (uILabel.font != null && UIAtlas.CheckIfRelated(this, uILabel.font.atlas))
{ 
UIFont font = uILabel.font;
uILabel.font = null;
uILabel.font = font;
}
k++;
}
array
i
num
uISprite
atlas
array2
j
num2
uIFont
atlas2
array3
k
num3
uILabel
font
}
}
