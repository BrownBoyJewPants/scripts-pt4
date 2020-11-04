using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[AddComponentMenu("NGUI/UI/Font"), ExecuteInEditMode]
public class UIFont : MonoBehaviour
{ 
public enum Alignment
{ 
Left,
Center,
Right
}

public enum SymbolStyle
{ 
None,
Uncolored,
Colored
}

[HideInInspector, SerializeField]
private Material mMat;

[HideInInspector, SerializeField]
private Rect mUVRect = new Rect(0f, 0f, 1f, 1f);

[HideInInspector, SerializeField]
private BMFont mFont = new BMFont();

[HideInInspector, SerializeField]
private int mSpacingX;

[HideInInspector, SerializeField]
private int mSpacingY;

[HideInInspector, SerializeField]
private UIAtlas mAtlas;

[HideInInspector, SerializeField]
private UIFont mReplacement;

[HideInInspector, SerializeField]
private float mPixelSize = 1f;

[HideInInspector, SerializeField]
private List<BMSymbol> mSymbols = new List<BMSymbol>();

[HideInInspector, SerializeField]
private Font mDynamicFont;

[HideInInspector, SerializeField]
private int mDynamicFontSize = 16;

[HideInInspector, SerializeField]
private FontStyle mDynamicFontStyle;

[HideInInspector, SerializeField]
private float mDynamicFontOffset;

private UIAtlas.Sprite mSprite;

private int mPMA = -1;

private bool mSpriteSet;

private List<Color> mColors = new List<Color>();

private static CharacterInfo mTemp;

private static CharacterInfo mChar;

public BMFont bmFont
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mFont : this.mReplacement.bmFont;
}
}

public int texWidth
{ 
get
{ 
return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texWidth) : this.mReplacement.texWidth;
}
}

public int texHeight
{ 
get
{ 
return (!(this.mReplacement != null)) ? ((this.mFont == null) ? 1 : this.mFont.texHeight) : this.mReplacement.texHeight;
}
}

public bool hasSymbols
{ 
get
{ 
return (!(this.mReplacement != null)) ? (this.mSymbols.Count != 0) : this.mReplacement.hasSymbols;
}
}

public List<BMSymbol> symbols
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mSymbols : this.mReplacement.symbols;
}
}

public UIAtlas atlas
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mAtlas : this.mReplacement.atlas;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.atlas = value;
}
else if (this.mAtlas != value)
{ 
if (value == null)
{ 
if (this.mAtlas != null)
{ 
this.mMat = this.mAtlas.spriteMaterial;
}
if (this.sprite != null)
{ 
this.mUVRect = this.uvRect;
}
}
this.mPMA = -1;
this.mAtlas = value;
this.MarkAsDirty();
}
}
}

public Material material
{ 
get
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.material;
}
if (this.mAtlas != null)
{ 
return this.mAtlas.spriteMaterial;
}
if (this.mMat != null)
{ 
if (this.mDynamicFont != null && this.mMat != this.mDynamicFont.material)
{ 
this.mMat.mainTexture = this.mDynamicFont.material.mainTexture;
}
return this.mMat;
}
if (this.mDynamicFont != null)
{ 
return this.mDynamicFont.material;
}
return null;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.material = value;
}
else if (this.mMat != value)
{ 
this.mPMA = -1;
this.mMat = value;
this.MarkAsDirty();
}
}
}

public float pixelSize
{ 
get
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.pixelSize;
}
if (this.mAtlas != null)
{ 
return this.mAtlas.pixelSize;
}
return this.mPixelSize;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.pixelSize = value;
}
else if (this.mAtlas != null)
{ 
this.mAtlas.pixelSize = value;
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

public bool premultipliedAlpha
{ 
get
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.premultipliedAlpha;
}
if (this.mAtlas != null)
{ 
return this.mAtlas.premultipliedAlpha;
}
if (this.mPMA == -1)
{ 
Material material = this.material;
this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
}
return this.mPMA == 1;
material
}
}

public Texture2D texture
{ 
get
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.texture;
}
Material material = this.material;
return (!(material != null)) ? null : (material.mainTexture as Texture2D);
material
}
}

public Rect uvRect
{ 
get
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.uvRect;
}
if (this.mAtlas != null && this.mSprite == null && this.sprite != null)
{ 
Texture texture = this.mAtlas.texture;
if (texture != null)
{ 
this.mUVRect = this.mSprite.outer;
if (this.mAtlas.coordinates == UIAtlas.Coordinates.Pixels)
{ 
this.mUVRect = NGUIMath.ConvertToTexCoords(this.mUVRect, texture.width, texture.height);
}
if (this.mSprite.hasPadding)
{ 
Rect rect = this.mUVRect;
this.mUVRect.xMin = rect.xMin - this.mSprite.paddingLeft * rect.width;
this.mUVRect.yMin = rect.yMin - this.mSprite.paddingBottom * rect.height;
this.mUVRect.xMax = rect.xMax + this.mSprite.paddingRight * rect.width;
this.mUVRect.yMax = rect.yMax + this.mSprite.paddingTop * rect.height;
}
if (this.mSprite.hasPadding)
{ 
this.Trim();
}
}
}
return this.mUVRect;
texture
rect
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.uvRect = value;
}
else if (this.sprite == null && this.mUVRect != value)
{ 
this.mUVRect = value;
this.MarkAsDirty();
}
}
}

public string spriteName
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mFont.spriteName : this.mReplacement.spriteName;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.spriteName = value;
}
else if (this.mFont.spriteName != value)
{ 
this.mFont.spriteName = value;
this.MarkAsDirty();
}
}
}

public int horizontalSpacing
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mSpacingX : this.mReplacement.horizontalSpacing;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.horizontalSpacing = value;
}
else if (this.mSpacingX != value)
{ 
this.mSpacingX = value;
this.MarkAsDirty();
}
}
}

public int verticalSpacing
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mSpacingY : this.mReplacement.verticalSpacing;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.verticalSpacing = value;
}
else if (this.mSpacingY != value)
{ 
this.mSpacingY = value;
this.MarkAsDirty();
}
}
}

public bool isValid
{ 
get
{ 
return this.mDynamicFont != null || this.mFont.isValid;
}
}

public int size
{ 
get
{ 
return (!(this.mReplacement != null)) ? ((!this.isDynamic) ? this.mFont.charSize : this.mDynamicFontSize) : this.mReplacement.size;
}
}

public UIAtlas.Sprite sprite
{ 
get
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.sprite;
}
if (!this.mSpriteSet)
{ 
this.mSprite = null;
}
if (this.mSprite == null)
{ 
if (this.mAtlas != null && !string.IsNullOrEmpty(this.mFont.spriteName))
{ 
this.mSprite = this.mAtlas.GetSprite(this.mFont.spriteName);
if (this.mSprite == null)
{ 
this.mSprite = this.mAtlas.GetSprite(base.name);
}
this.mSpriteSet = true;
if (this.mSprite == null)
{ 
this.mFont.spriteName = null;
}
}
int i = 0;
int count = this.mSymbols.Count;
while (i < count)
{ 
this.symbols[i].MarkAsDirty();
i++;
}
}
return this.mSprite;
i
count
}
}

public UIFont replacement
{ 
get
{ 
return this.mReplacement;
}
set
{ 
UIFont uIFont = value;
if (uIFont == this)
{ 
uIFont = null;
}
if (this.mReplacement != uIFont)
{ 
if (uIFont != null && uIFont.replacement == this)
{ 
uIFont.replacement = null;
}
if (this.mReplacement != null)
{ 
this.MarkAsDirty();
}
this.mReplacement = uIFont;
this.MarkAsDirty();
}
uIFont
}
}

public bool isDynamic
{ 
get
{ 
return this.mDynamicFont != null;
}
}

public Font dynamicFont
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mDynamicFont : this.mReplacement.dynamicFont;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.dynamicFont = value;
}
else if (this.mDynamicFont != value)
{ 
if (this.mDynamicFont != null)
{ 
this.material = null;
}
this.mDynamicFont = value;
this.MarkAsDirty();
}
}
}

public int dynamicFontSize
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mDynamicFontSize : this.mReplacement.dynamicFontSize;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.dynamicFontSize = value;
}
else
{ 
value = Mathf.Clamp(value, 4, 128);
if (this.mDynamicFontSize != value)
{ 
this.mDynamicFontSize = value;
this.MarkAsDirty();
}
}
}
}

public FontStyle dynamicFontStyle
{ 
get
{ 
return (!(this.mReplacement != null)) ? this.mDynamicFontStyle : this.mReplacement.dynamicFontStyle;
}
set
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.dynamicFontStyle = value;
}
else if (this.mDynamicFontStyle != value)
{ 
this.mDynamicFontStyle = value;
this.MarkAsDirty();
}
}
}

private Texture dynamicTexture
{ 
get
{ 
if (this.mReplacement)
{ 
return this.mReplacement.dynamicTexture;
}
if (this.isDynamic)
{ 
return this.mDynamicFont.material.mainTexture;
}
return null;
}
}

private void Trim()
{ 
Texture texture = this.mAtlas.texture;
if (texture != null && this.mSprite != null)
{ 
Rect rect = NGUIMath.ConvertToPixels(this.mUVRect, this.texture.width, this.texture.height, true);
Rect rect2 = (this.mAtlas.coordinates != UIAtlas.Coordinates.TexCoords) ? this.mSprite.outer : NGUIMath.ConvertToPixels(this.mSprite.outer, texture.width, texture.height, true);
int xMin = Mathf.RoundToInt(rect2.xMin - rect.xMin);
int yMin = Mathf.RoundToInt(rect2.yMin - rect.yMin);
int xMax = Mathf.RoundToInt(rect2.xMax - rect.xMin);
int yMax = Mathf.RoundToInt(rect2.yMax - rect.yMin);
this.mFont.Trim(xMin, yMin, xMax, yMax);
}
texture
rect
rect2
xMin
yMin
xMax
yMax
}

private bool References(UIFont font)
{ 
return !(font == null) && (font == this || (this.mReplacement != null && this.mReplacement.References(font)));
}

public static bool CheckIfRelated(UIFont a, UIFont b)
{ 
return !(a == null) && !(b == null) && ((a.isDynamic && b.isDynamic && a.dynamicFont.fontNames[0] == b.dynamicFont.fontNames[0]) || a == b || a.References(b) || b.References(a));
}

public void MarkAsDirty()
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.MarkAsDirty();
}
this.mSprite = null;
UILabel[] array = NGUITools.FindActive<UILabel>();
int i = 0;
int num = array.Length;
while (i < num)
{ 
UILabel uILabel = array[i];
if (uILabel.enabled && NGUITools.GetActive(uILabel.gameObject) && UIFont.CheckIfRelated(this, uILabel.font))
{ 
UIFont font = uILabel.font;
uILabel.font = null;
uILabel.font = font;
}
i++;
}
int j = 0;
int count = this.mSymbols.Count;
while (j < count)
{ 
this.symbols[j].MarkAsDirty();
j++;
}
array
i
num
uILabel
font
j
count
}

public void Request(string text)
{ 
if (!string.IsNullOrEmpty(text))
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.Request(text);
}
else if (this.mDynamicFont != null)
{ 
this.mDynamicFont.RequestCharactersInTexture("j", this.mDynamicFontSize, this.mDynamicFontStyle);
this.mDynamicFont.GetCharacterInfo('j', out UIFont.mTemp, this.mDynamicFontSize, this.mDynamicFontStyle);
this.mDynamicFontOffset = (float)this.mDynamicFontSize + UIFont.mTemp.vert.yMax;
this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize, this.mDynamicFontStyle);
}
}
}

public Vector2 CalculatePrintedSize(string text, bool encoding, UIFont.SymbolStyle symbolStyle)
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.CalculatePrintedSize(text, encoding, symbolStyle);
}
Vector2 zero = Vector2.zero;
bool isDynamic = this.isDynamic;
if (isDynamic || (this.mFont != null && this.mFont.isValid && !string.IsNullOrEmpty(text)))
{ 
if (encoding)
{ 
text = NGUITools.StripSymbols(text);
}
if (this.mDynamicFont != null)
{ 
this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize);
}
int length = text.Length;
int num = 0;
int num2 = 0;
int num3 = 0;
int num4 = 0;
int size = this.size;
int num5 = size + this.mSpacingY;
bool flag = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols;
for (int i = 0; i < length; i++)
{ 
char c = text[i];
if (c == '\n')
{ 
if (num2 > num)
{ 
num = num2;
}
num2 = 0;
num3 += num5;
num4 = 0;
}
else if (c < ' ')
{ 
num4 = 0;
}
else if (!isDynamic)
{ 
BMSymbol bMSymbol = (!flag) ? null : this.MatchSymbol(text, i, length);
if (bMSymbol == null)
{ 
BMGlyph glyph = this.mFont.GetGlyph((int)c);
if (glyph != null)
{ 
num2 += this.mSpacingX + ((num4 == 0) ? glyph.advance : (glyph.advance + glyph.GetKerning(num4)));
num4 = (int)c;
}
}
else
{ 
num2 += this.mSpacingX + bMSymbol.width;
i += bMSymbol.length - 1;
num4 = 0;
}
}
else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
{ 
num2 += (int)((float)this.mSpacingX + UIFont.mChar.width);
}
}
float num6 = (size <= 0) ? 1f : (1f / (float)size);
zero.x = num6 * (float)((num2 <= num) ? num : num2);
zero.y = num6 * (float)(num3 + num5);
}
return zero;
zero
isDynamic
length
num
num2
num3
num4
size
num5
flag
i
c
bMSymbol
glyph
num6
}

private static void EndLine(ref StringBuilder s)
{ 
int num = s.Length - 1;
if (num > 0 && s[num] == ' ')
{ 
s[num] = '\n';
}
else
{ 
s.Append('\n');
}
num
}

public string GetEndOfLineThatFits(string text, float maxWidth, bool encoding, UIFont.SymbolStyle symbolStyle)
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.GetEndOfLineThatFits(text, maxWidth, encoding, symbolStyle);
}
int num = Mathf.RoundToInt(maxWidth * (float)this.size);
if (num < 1)
{ 
return text;
}
if (this.mDynamicFont != null)
{ 
this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize);
}
int length = text.Length;
int num2 = num;
BMGlyph bMGlyph = null;
int num3 = length;
bool flag = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols;
bool isDynamic = this.isDynamic;
while (num3 > 0 && num2 > 0)
{ 
char c = text[--num3];
BMSymbol bMSymbol = (!flag) ? null : this.MatchSymbol(text, num3, length);
int num4 = this.mSpacingX;
if (!isDynamic)
{ 
if (bMSymbol != null)
{ 
num4 += bMSymbol.advance;
}
else
{ 
BMGlyph glyph = this.mFont.GetGlyph((int)c);
if (glyph == null)
{ 
bMGlyph = null;
continue;
}
num4 += glyph.advance + ((bMGlyph != null) ? bMGlyph.GetKerning((int)c) : 0);
bMGlyph = glyph;
}
}
else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
{ 
num4 += (int)UIFont.mChar.width;
}
num2 -= num4;
}
if (num2 < 0)
{ 
num3++;
}
return text.Substring(num3, length - num3);
num
length
num2
bMGlyph
num3
flag
isDynamic
c
bMSymbol
num4
glyph
}

public bool WrapText(string text, out string finalText, float width, float height, int lines, bool encoding, UIFont.SymbolStyle symbolStyle)
{ 
if (this.mReplacement != null)
{ 
return this.mReplacement.WrapText(text, out finalText, width, height, lines, encoding, symbolStyle);
}
if (width == 0f)
{ 
width = 100000f;
}
if (height == 0f)
{ 
height = 100000f;
}
int num = Mathf.FloorToInt(width * (float)this.size);
int num2 = Mathf.FloorToInt(height * (float)this.size);
if (num < 1 || num2 < 1)
{ 
finalText = string.Empty;
return false;
}
int num3 = (lines <= 0) ? 999999 : lines;
if (height != 0f)
{ 
num3 = Mathf.Min(num3, Mathf.FloorToInt(height));
if (num3 == 0)
{ 
finalText = string.Empty;
return false;
}
}
if (this.mDynamicFont != null)
{ 
this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize);
}
StringBuilder stringBuilder = new StringBuilder();
int length = text.Length;
int num4 = num;
int num5 = 0;
int num6 = 0;
int i = 0;
bool flag = true;
bool flag2 = lines != 1;
int num7 = 1;
bool flag3 = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols;
bool isDynamic = this.isDynamic;
while (i < length)
{ 
char c = text[i];
if (c == '\n')
{ 
if (!flag2 || num7 == num3)
{ 
break;
}
num4 = num;
if (num6 < i)
{ 
stringBuilder.Append(text.Substring(num6, i - num6 + 1));
}
else
{ 
stringBuilder.Append(c);
}
flag = true;
num7++;
num6 = i + 1;
num5 = 0;
}
else
{ 
if (c == ' ' && num5 != 32 && num6 < i)
{ 
stringBuilder.Append(text.Substring(num6, i - num6 + 1));
flag = false;
num6 = i + 1;
num5 = (int)c;
}
if (encoding && c == '[' && i + 2 < length)
{ 
if (text[i + 1] == '-' && text[i + 2] == ']')
{ 
i += 2;
goto IL_45B;
}
if (i + 7 < length && text[i + 7] == ']' && NGUITools.EncodeColor(NGUITools.ParseColor(text, i + 1)) == text.Substring(i + 1, 6).ToUpper())
{ 
i += 7;
goto IL_45B;
}
}
BMSymbol bMSymbol = (!flag3) ? null : this.MatchSymbol(text, i, length);
int num8 = this.mSpacingX;
if (!isDynamic)
{ 
if (bMSymbol != null)
{ 
num8 += bMSymbol.advance;
}
else
{ 
BMGlyph bMGlyph = (bMSymbol != null) ? null : this.mFont.GetGlyph((int)c);
if (bMGlyph == null)
{ 
goto IL_45B;
}
num8 += ((num5 == 0) ? bMGlyph.advance : (bMGlyph.advance + bMGlyph.GetKerning(num5)));
}
}
else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
{ 
num8 += Mathf.RoundToInt(UIFont.mChar.width);
}
num4 -= num8;
if (num4 < 0)
{ 
if (flag || !flag2 || num7 == num3)
{ 
stringBuilder.Append(text.Substring(num6, Mathf.Max(0, i - num6)));
if (!flag2 || num7 == num3)
{ 
num6 = i;
break;
}
UIFont.EndLine(ref stringBuilder);
flag = true;
num7++;
if (c == ' ')
{ 
num6 = i + 1;
num4 = num;
}
else
{ 
num6 = i;
num4 = num - num8;
}
num5 = 0;
}
else
{ 
while (num6 < length && text[num6] == ' ')
{ 
num6++;
}
flag = true;
num4 = num;
i = num6 - 1;
num5 = 0;
if (!flag2 || num7 == num3)
{ 
break;
}
num7++;
UIFont.EndLine(ref stringBuilder);
goto IL_45B;
}
}
else
{ 
num5 = (int)c;
}
if (!isDynamic && bMSymbol != null)
{ 
i += bMSymbol.length - 1;
num5 = 0;
}
}
IL_45B:
i++;
}
if (num6 < i)
{ 
stringBuilder.Append(text.Substring(num6, i - num6));
}
finalText = stringBuilder.ToString();
return !flag2 || i == length || (lines > 0 && num7 <= lines);
num
num2
num3
stringBuilder
length
num4
num5
num6
i
flag
flag2
num7
flag3
isDynamic
c
bMSymbol
num8
bMGlyph
}

public bool WrapText(string text, out string finalText, float maxWidth, float maxHeight, int maxLineCount, bool encoding)
{ 
return this.WrapText(text, out finalText, maxWidth, maxHeight, maxLineCount, encoding, UIFont.SymbolStyle.None);
}

public bool WrapText(string text, out string finalText, float maxWidth, float maxHeight, int maxLineCount)
{ 
return this.WrapText(text, out finalText, maxWidth, maxHeight, maxLineCount, false, UIFont.SymbolStyle.None);
}

private void Align(BetterList<Vector3> verts, int indexOffset, UIFont.Alignment alignment, int x, int lineWidth)
{ 
if (alignment != UIFont.Alignment.Left)
{ 
int size = this.size;
if (size > 0)
{ 
float num;
if (alignment == UIFont.Alignment.Right)
{ 
num = (float)Mathf.RoundToInt((float)(lineWidth - x));
if (num < 0f)
{ 
num = 0f;
}
num /= (float)this.size;
}
else
{ 
num = (float)Mathf.RoundToInt((float)(lineWidth - x) * 0.5f);
if (num < 0f)
{ 
num = 0f;
}
num /= (float)this.size;
if ((lineWidth & 1) == 1)
{ 
num += 0.5f / (float)size;
}
}
for (int i = indexOffset; i < verts.size; i++)
{ 
Vector3 vector = verts.buffer[i];
vector.x += num;
verts.buffer[i] = vector;
}
}
}
size
num
i
vector
}

public void Print(string text, Color32 color, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, bool encoding, UIFont.SymbolStyle symbolStyle, UIFont.Alignment alignment, int lineWidth, bool premultiply)
{ 
if (this.mReplacement != null)
{ 
this.mReplacement.Print(text, color, verts, uvs, cols, encoding, symbolStyle, alignment, lineWidth, premultiply);
}
else if (text != null)
{ 
if (!this.isValid)
{ 
Debug.LogError("Attempting to print using an invalid font!");
return;
}
if (this.mDynamicFont != null)
{ 
this.mDynamicFont.RequestCharactersInTexture(text, this.mDynamicFontSize);
}
bool isDynamic = this.isDynamic;
this.mColors.Clear();
this.mColors.Add(color);
int size = this.size;
Vector2 vector = (size <= 0) ? Vector2.one : new Vector2(1f / (float)size, 1f / (float)size);
int size2 = verts.size;
int num = 0;
int num2 = 0;
int num3 = 0;
int num4 = 0;
int num5 = size + this.mSpacingY;
Vector3 zero = Vector3.zero;
Vector3 zero2 = Vector3.zero;
Vector2 zero3 = Vector2.zero;
Vector2 zero4 = Vector2.zero;
float num6 = this.uvRect.width / (float)this.mFont.texWidth;
float num7 = this.mUVRect.height / (float)this.mFont.texHeight;
int length = text.Length;
bool flag = encoding && symbolStyle != UIFont.SymbolStyle.None && this.hasSymbols && this.sprite != null;
for (int i = 0; i < length; i++)
{ 
char c = text[i];
if (c == '\n')
{ 
if (num2 > num)
{ 
num = num2;
}
if (alignment != UIFont.Alignment.Left)
{ 
this.Align(verts, size2, alignment, num2, lineWidth);
size2 = verts.size;
}
num2 = 0;
num3 += num5;
num4 = 0;
}
else if (c < ' ')
{ 
num4 = 0;
}
else
{ 
if (encoding && c == '[')
{ 
int num8 = NGUITools.ParseSymbol(text, i, this.mColors, premultiply);
if (num8 > 0)
{ 
color = this.mColors[this.mColors.Count - 1];
i += num8 - 1;
goto IL_94E;
}
}
if (!isDynamic)
{ 
BMSymbol bMSymbol = (!flag) ? null : this.MatchSymbol(text, i, length);
if (bMSymbol == null)
{ 
BMGlyph glyph = this.mFont.GetGlyph((int)c);
if (glyph == null)
{ 
goto IL_94E;
}
if (num4 != 0)
{ 
num2 += glyph.GetKerning(num4);
}
if (c == ' ')
{ 
num2 += this.mSpacingX + glyph.advance;
num4 = (int)c;
goto IL_94E;
}
zero.x = vector.x * (float)(num2 + glyph.offsetX);
zero.y = -vector.y * (float)(num3 + glyph.offsetY);
zero2.x = zero.x + vector.x * (float)glyph.width;
zero2.y = zero.y - vector.y * (float)glyph.height;
zero3.x = this.mUVRect.xMin + num6 * (float)glyph.x;
zero3.y = this.mUVRect.yMax - num7 * (float)glyph.y;
zero4.x = zero3.x + num6 * (float)glyph.width;
zero4.y = zero3.y - num7 * (float)glyph.height;
num2 += this.mSpacingX + glyph.advance;
num4 = (int)c;
if (glyph.channel == 0 || glyph.channel == 15)
{ 
for (int j = 0; j < 4; j++)
{ 
cols.Add(color);
}
}
else
{ 
Color color2 = color;
color2 *= 0.49f;
switch (glyph.channel)
{ 
case 1: 
color2.b += 0.51f;
break;
case 2: 
color2.g += 0.51f;
break;
case 4: 
color2.r += 0.51f;
break;
case 8: 
color2.a += 0.51f;
break;
 }
for (int k = 0; k < 4; k++)
{ 
cols.Add(color2);
}
}
}
else
{ 
zero.x = vector.x * (float)(num2 + bMSymbol.offsetX);
zero.y = -vector.y * (float)(num3 + bMSymbol.offsetY);
zero2.x = zero.x + vector.x * (float)bMSymbol.width;
zero2.y = zero.y - vector.y * (float)bMSymbol.height;
Rect uvRect = bMSymbol.uvRect;
zero3.x = uvRect.xMin;
zero3.y = uvRect.yMax;
zero4.x = uvRect.xMax;
zero4.y = uvRect.yMin;
num2 += this.mSpacingX + bMSymbol.advance;
i += bMSymbol.length - 1;
num4 = 0;
if (symbolStyle == UIFont.SymbolStyle.Colored)
{ 
for (int l = 0; l < 4; l++)
{ 
cols.Add(color);
}
}
else
{ 
Color32 item = Color.white;
item.a = color.a;
for (int m = 0; m < 4; m++)
{ 
cols.Add(item);
}
}
}
verts.Add(new Vector3(zero2.x, zero.y));
verts.Add(new Vector3(zero2.x, zero2.y));
verts.Add(new Vector3(zero.x, zero2.y));
verts.Add(new Vector3(zero.x, zero.y));
uvs.Add(new Vector2(zero4.x, zero3.y));
uvs.Add(new Vector2(zero4.x, zero4.y));
uvs.Add(new Vector2(zero3.x, zero4.y));
uvs.Add(new Vector2(zero3.x, zero3.y));
}
else if (this.mDynamicFont.GetCharacterInfo(c, out UIFont.mChar, this.mDynamicFontSize, this.mDynamicFontStyle))
{ 
zero.x = vector.x * ((float)num2 + UIFont.mChar.vert.xMin);
zero.y = -vector.y * ((float)num3 - UIFont.mChar.vert.yMax + this.mDynamicFontOffset);
zero2.x = zero.x + vector.x * UIFont.mChar.vert.width;
zero2.y = zero.y - vector.y * UIFont.mChar.vert.height;
zero3.x = UIFont.mChar.uv.xMin;
zero3.y = UIFont.mChar.uv.yMin;
zero4.x = UIFont.mChar.uv.xMax;
zero4.y = UIFont.mChar.uv.yMax;
num2 += this.mSpacingX + (int)UIFont.mChar.width;
for (int n = 0; n < 4; n++)
{ 
cols.Add(color);
}
if (UIFont.mChar.flipped)
{ 
uvs.Add(new Vector2(zero3.x, zero4.y));
uvs.Add(new Vector2(zero3.x, zero3.y));
uvs.Add(new Vector2(zero4.x, zero3.y));
uvs.Add(new Vector2(zero4.x, zero4.y));
}
else
{ 
uvs.Add(new Vector2(zero4.x, zero3.y));
uvs.Add(new Vector2(zero3.x, zero3.y));
uvs.Add(new Vector2(zero3.x, zero4.y));
uvs.Add(new Vector2(zero4.x, zero4.y));
}
verts.Add(new Vector3(zero2.x, zero.y));
verts.Add(new Vector3(zero.x, zero.y));
verts.Add(new Vector3(zero.x, zero2.y));
verts.Add(new Vector3(zero2.x, zero2.y));
}
}
IL_94E:;
}
if (alignment != UIFont.Alignment.Left && size2 < verts.size)
{ 
this.Align(verts, size2, alignment, num2, lineWidth);
size2 = verts.size;
}
}
isDynamic
size
vector
size2
num
num2
num3
num4
num5
zero
zero2
zero3
zero4
num6
num7
length
flag
i
c
num8
bMSymbol
glyph
j
color2
k
uvRect
l
item
m
n
}

private BMSymbol GetSymbol(string sequence, bool createIfMissing)
{ 
int i = 0;
int count = this.mSymbols.Count;
while (i < count)
{ 
BMSymbol bMSymbol = this.mSymbols[i];
if (bMSymbol.sequence == sequence)
{ 
return bMSymbol;
}
i++;
}
if (createIfMissing)
{ 
BMSymbol bMSymbol2 = new BMSymbol();
bMSymbol2.sequence = sequence;
this.mSymbols.Add(bMSymbol2);
return bMSymbol2;
}
return null;
i
count
bMSymbol
bMSymbol2
}

private BMSymbol MatchSymbol(string text, int offset, int textLength)
{ 
int count = this.mSymbols.Count;
if (count == 0)
{ 
return null;
}
textLength -= offset;
for (int i = 0; i < count; i++)
{ 
BMSymbol bMSymbol = this.mSymbols[i];
int length = bMSymbol.length;
if (length != 0 && textLength >= length)
{ 
bool flag = true;
for (int j = 0; j < length; j++)
{ 
if (text[offset + j] != bMSymbol.sequence[j])
{ 
flag = false;
break;
}
}
if (flag && bMSymbol.Validate(this.atlas))
{ 
return bMSymbol;
}
}
}
return null;
count
i
bMSymbol
length
flag
j
}

public void AddSymbol(string sequence, string spriteName)
{ 
BMSymbol symbol = this.GetSymbol(sequence, true);
symbol.spriteName = spriteName;
this.MarkAsDirty();
symbol
}

public void RemoveSymbol(string sequence)
{ 
BMSymbol symbol = this.GetSymbol(sequence, false);
if (symbol != null)
{ 
this.symbols.Remove(symbol);
}
this.MarkAsDirty();
symbol
}

public void RenameSymbol(string before, string after)
{ 
BMSymbol symbol = this.GetSymbol(before, false);
if (symbol != null)
{ 
symbol.sequence = after;
}
this.MarkAsDirty();
symbol
}

public bool UsesSprite(string s)
{ 
if (!string.IsNullOrEmpty(s))
{ 
if (s.Equals(this.spriteName))
{ 
return true;
}
int i = 0;
int count = this.symbols.Count;
while (i < count)
{ 
BMSymbol bMSymbol = this.symbols[i];
if (s.Equals(bMSymbol.spriteName))
{ 
return true;
}
i++;
}
}
return false;
i
count
bMSymbol
}
}
