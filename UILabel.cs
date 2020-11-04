using System;
using UnityEngine;


[AddComponentMenu("NGUI/UI/Label"), ExecuteInEditMode]
public class UILabel : UIWidget
{ 
public enum Effect
{ 
None,
Shadow,
Outline
}

[HideInInspector, SerializeField]
private UIFont mFont;

[HideInInspector, SerializeField]
private string mText = string.Empty;

[HideInInspector, SerializeField]
private int mMaxLineWidth;

[HideInInspector, SerializeField]
private int mMaxLineHeight;

[HideInInspector, SerializeField]
private bool mEncoding = true;

[HideInInspector, SerializeField]
private int mMaxLineCount;

[HideInInspector, SerializeField]
private bool mPassword;

[HideInInspector, SerializeField]
private bool mShowLastChar;

[HideInInspector, SerializeField]
private UILabel.Effect mEffectStyle;

[HideInInspector, SerializeField]
private Color mEffectColor = Color.black;

[HideInInspector, SerializeField]
private UIFont.SymbolStyle mSymbols = UIFont.SymbolStyle.Uncolored;

[HideInInspector, SerializeField]
private Vector2 mEffectDistance = Vector2.one;

[HideInInspector, SerializeField]
private bool mShrinkToFit;

[HideInInspector, SerializeField]
private float mLineWidth;

[HideInInspector, SerializeField]
private bool mMultiline = true;

private bool mShouldBeProcessed = true;

private string mProcessedText;

private Vector3 mLastScale = Vector3.one;

private Vector2 mSize = Vector2.zero;

private bool mPremultiply;

public override float alpha
{ 
get
{ 
return base.alpha;
}
set
{ 
base.alpha = value;
this.mEffectColor.a = value;
}
}

private bool hasChanged
{ 
get
{ 
return this.mShouldBeProcessed;
}
set
{ 
if (value)
{ 
this.mChanged = true;
this.mShouldBeProcessed = true;
}
else
{ 
this.mShouldBeProcessed = false;
}
}
}

public override Material material
{ 
get
{ 
return (!(this.mFont != null)) ? null : this.mFont.material;
}
}

public UIFont font
{ 
get
{ 
return this.mFont;
}
set
{ 
if (this.mFont != value)
{ 
if (this.mFont != null && this.mFont.dynamicFont != null)
{ 
Font expr_43 = this.mFont.dynamicFont;
expr_43.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Remove(expr_43.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
}
base.RemoveFromPanel();
this.mFont = value;
this.hasChanged = true;
if (this.mFont != null && this.mFont.dynamicFont != null)
{ 
Font expr_AB = this.mFont.dynamicFont;
expr_AB.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Combine(expr_AB.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
this.mFont.Request(this.mText);
}
this.MarkAsChanged();
}
expr_43
expr_AB
}
}

public string text
{ 
get
{ 
return this.mText;
}
set
{ 
if (string.IsNullOrEmpty(value))
{ 
if (!string.IsNullOrEmpty(this.mText))
{ 
this.mText = string.Empty;
}
this.hasChanged = true;
}
else if (this.mText != value)
{ 
this.mText = value;
this.hasChanged = true;
if (this.mFont != null)
{ 
this.mFont.Request(value);
}
if (this.shrinkToFit)
{ 
this.MakePixelPerfect();
}
}
}
}

public bool supportEncoding
{ 
get
{ 
return this.mEncoding;
}
set
{ 
if (this.mEncoding != value)
{ 
this.mEncoding = value;
this.hasChanged = true;
if (value)
{ 
this.mPassword = false;
}
}
}
}

public UIFont.SymbolStyle symbolStyle
{ 
get
{ 
return this.mSymbols;
}
set
{ 
if (this.mSymbols != value)
{ 
this.mSymbols = value;
this.hasChanged = true;
}
}
}

public int lineWidth
{ 
get
{ 
return this.mMaxLineWidth;
}
set
{ 
if (this.mMaxLineWidth != value)
{ 
this.mMaxLineWidth = value;
this.hasChanged = true;
if (this.shrinkToFit)
{ 
this.MakePixelPerfect();
}
}
}
}

public int lineHeight
{ 
get
{ 
return this.mMaxLineHeight;
}
set
{ 
if (this.mMaxLineHeight != value)
{ 
this.mMaxLineHeight = value;
this.hasChanged = true;
if (this.shrinkToFit)
{ 
this.MakePixelPerfect();
}
}
}
}

public bool multiLine
{ 
get
{ 
return this.mMaxLineCount != 1;
}
set
{ 
if (this.mMaxLineCount != 1 != value)
{ 
this.mMaxLineCount = ((!value) ? 1 : 0);
this.hasChanged = true;
if (value)
{ 
this.mPassword = false;
}
}
}
}

public int maxLineCount
{ 
get
{ 
return this.mMaxLineCount;
}
set
{ 
if (this.mMaxLineCount != value)
{ 
this.mMaxLineCount = Mathf.Max(value, 0);
if (value != 1)
{ 
this.mPassword = false;
}
this.hasChanged = true;
if (this.shrinkToFit)
{ 
this.MakePixelPerfect();
}
}
}
}

public bool password
{ 
get
{ 
return this.mPassword;
}
set
{ 
if (this.mPassword != value)
{ 
if (value)
{ 
this.mMaxLineCount = 1;
this.mEncoding = false;
}
this.mPassword = value;
this.hasChanged = true;
}
}
}

public bool showLastPasswordChar
{ 
get
{ 
return this.mShowLastChar;
}
set
{ 
if (this.mShowLastChar != value)
{ 
this.mShowLastChar = value;
this.hasChanged = true;
}
}
}

public UILabel.Effect effectStyle
{ 
get
{ 
return this.mEffectStyle;
}
set
{ 
if (this.mEffectStyle != value)
{ 
this.mEffectStyle = value;
this.hasChanged = true;
}
}
}

public Color effectColor
{ 
get
{ 
return this.mEffectColor;
}
set
{ 
if (!this.mEffectColor.Equals(value))
{ 
this.mEffectColor = value;
if (this.mEffectStyle != UILabel.Effect.None)
{ 
this.hasChanged = true;
}
}
}
}

public Vector2 effectDistance
{ 
get
{ 
return this.mEffectDistance;
}
set
{ 
if (this.mEffectDistance != value)
{ 
this.mEffectDistance = value;
this.hasChanged = true;
}
}
}

public bool shrinkToFit
{ 
get
{ 
return this.mShrinkToFit;
}
set
{ 
if (this.mShrinkToFit != value)
{ 
this.mShrinkToFit = value;
this.hasChanged = true;
}
}
}

public string processedText
{ 
get
{ 
if (this.mLastScale != base.cachedTransform.localScale)
{ 
this.mLastScale = base.cachedTransform.localScale;
this.mShouldBeProcessed = true;
}
if (this.hasChanged)
{ 
this.ProcessText();
}
return this.mProcessedText;
}
}

public override Vector2 relativeSize
{ 
get
{ 
if (this.mFont == null)
{ 
return Vector3.one;
}
if (this.hasChanged)
{ 
this.ProcessText();
}
return this.mSize;
}
}

protected override void OnEnable()
{ 
if (this.mFont != null && this.mFont.dynamicFont != null)
{ 
Font expr_32 = this.mFont.dynamicFont;
expr_32.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Combine(expr_32.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
}
base.OnEnable();
expr_32
}

protected override void OnDisable()
{ 
if (this.mFont != null && this.mFont.dynamicFont != null)
{ 
Font expr_32 = this.mFont.dynamicFont;
expr_32.textureRebuildCallback = (Font.FontTextureRebuildCallback)Delegate.Remove(expr_32.textureRebuildCallback, new Font.FontTextureRebuildCallback(this.MarkAsChanged));
}
base.OnDisable();
expr_32
}

protected override void OnStart()
{ 
if (this.mLineWidth > 0f)
{ 
this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
this.mLineWidth = 0f;
}
if (!this.mMultiline)
{ 
this.mMaxLineCount = 1;
this.mMultiline = true;
}
this.mPremultiply = (this.font != null && this.font.material != null && this.font.material.shader.name.Contains("Premultiplied"));
if (this.mFont != null)
{ 
this.mFont.Request(this.mText);
}
}

public override void MarkAsChanged()
{ 
this.hasChanged = true;
base.MarkAsChanged();
}

private void ProcessText()
{ 
this.mChanged = true;
this.hasChanged = false;
float num = Mathf.Abs(base.cachedTransform.localScale.x);
if (num > 0f)
{ 
do
{ 
bool flag = true;
if (this.mPassword)
{ 
this.mProcessedText = string.Empty;
if (this.mShowLastChar)
{ 
int i = 0;
int num2 = this.mText.Length - 1;
while (i < num2)
{ 
this.mProcessedText += "*";
i++;
}
if (this.mText.Length > 0)
{ 
this.mProcessedText += this.mText[this.mText.Length - 1];
}
}
else
{ 
int j = 0;
int length = this.mText.Length;
while (j < length)
{ 
this.mProcessedText += "*";
j++;
}
}
flag = this.mFont.WrapText(this.mProcessedText, out this.mProcessedText, (float)this.mMaxLineWidth / num, (float)this.mMaxLineHeight / num, this.mMaxLineCount, false, UIFont.SymbolStyle.None);
}
else if (this.mMaxLineWidth > 0 || this.mMaxLineHeight > 0)
{ 
flag = this.mFont.WrapText(this.mText, out this.mProcessedText, (float)this.mMaxLineWidth / num, (float)this.mMaxLineHeight / num, this.mMaxLineCount, this.mEncoding, this.mSymbols);
}
else
{ 
this.mProcessedText = this.mText;
}
this.mSize = (string.IsNullOrEmpty(this.mProcessedText) ? Vector2.one : this.mFont.CalculatePrintedSize(this.mProcessedText, this.mEncoding, this.mSymbols));
if (!this.mShrinkToFit)
{ 
goto IL_280;
}
if (flag)
{ 
break;
}
num = Mathf.Round(num - 1f);
}
while (num > 1f);
if (this.mMaxLineWidth > 0)
{ 
float num3 = (float)this.mMaxLineWidth / num;
float a = (this.mSize.x * num <= num3) ? num : (num3 / this.mSize.x * num);
num = Mathf.Min(a, num);
}
num = Mathf.Round(num);
base.cachedTransform.localScale = new Vector3(num, num, 1f);
IL_280:
this.mSize.x = Mathf.Max(this.mSize.x, (num <= 0f) ? 1f : ((float)this.mMaxLineWidth / num));
this.mSize.y = Mathf.Max(this.mSize.y, (num <= 0f) ? 1f : ((float)this.mMaxLineHeight / num));
}
else
{ 
this.mSize.x = 1f;
this.mSize.y = 1f;
num = (float)this.mFont.size;
base.cachedTransform.localScale = new Vector3(num, num, 1f);
this.mProcessedText = string.Empty;
}
this.mSize.y = Mathf.Max(Mathf.Max(this.mSize.y, 1f), (float)this.mMaxLineHeight / num);
num
flag
i
num2
j
length
num3
a
}

public override void MakePixelPerfect()
{ 
if (this.mFont != null)
{ 
float pixelSize = this.font.pixelSize;
Vector3 localScale = base.cachedTransform.localScale;
localScale.x = (float)this.mFont.size * pixelSize;
localScale.y = localScale.x;
localScale.z = 1f;
Vector3 localPosition = base.cachedTransform.localPosition;
localPosition.x = (float)(Mathf.CeilToInt(localPosition.x / pixelSize * 4f) >> 2);
localPosition.y = (float)(Mathf.CeilToInt(localPosition.y / pixelSize * 4f) >> 2);
localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
localPosition.x *= pixelSize;
localPosition.y *= pixelSize;
base.cachedTransform.localPosition = localPosition;
base.cachedTransform.localScale = localScale;
if (this.shrinkToFit)
{ 
this.ProcessText();
}
}
else
{ 
base.MakePixelPerfect();
}
pixelSize
localScale
localPosition
}

private void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
{ 
Color color = this.mEffectColor;
color.a *= this.alpha * this.mPanel.alpha;
Color32 color2 = (!this.font.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color);
for (int i = start; i < end; i++)
{ 
verts.Add(verts.buffer[i]);
uvs.Add(uvs.buffer[i]);
cols.Add(cols.buffer[i]);
Vector3 vector = verts.buffer[i];
vector.x += x;
vector.y += y;
verts.buffer[i] = vector;
cols.buffer[i] = color2;
}
color
color2
i
vector
}

public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
{ 
if (this.mFont == null)
{ 
return;
}
UIWidget.Pivot pivot = base.pivot;
int start = verts.size;
Color c = base.color;
c.a *= this.mPanel.alpha;
if (this.font.premultipliedAlpha)
{ 
c = NGUITools.ApplyPMA(c);
}
if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
{ 
this.mFont.Print(this.processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Left, 0, this.mPremultiply);
}
else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
{ 
this.mFont.Print(this.processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Right, Mathf.RoundToInt(this.relativeSize.x * (float)this.mFont.size), this.mPremultiply);
}
else
{ 
this.mFont.Print(this.processedText, c, verts, uvs, cols, this.mEncoding, this.mSymbols, UIFont.Alignment.Center, Mathf.RoundToInt(this.relativeSize.x * (float)this.mFont.size), this.mPremultiply);
}
if (this.effectStyle != UILabel.Effect.None)
{ 
int size = verts.size;
float num = 1f / ((float)this.mFont.size * this.mFont.pixelSize);
float num2 = num * this.mEffectDistance.x;
float num3 = num * this.mEffectDistance.y;
this.ApplyShadow(verts, uvs, cols, start, size, num2, -num3);
if (this.effectStyle == UILabel.Effect.Outline)
{ 
start = size;
size = verts.size;
this.ApplyShadow(verts, uvs, cols, start, size, -num2, num3);
start = size;
size = verts.size;
this.ApplyShadow(verts, uvs, cols, start, size, num2, num3);
start = size;
size = verts.size;
this.ApplyShadow(verts, uvs, cols, start, size, -num2, -num3);
}
}
pivot
start
c
size
num
num2
num3
}
}
