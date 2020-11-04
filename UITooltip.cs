using System;
using UnityEngine;


[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{ 
private static UITooltip mInstance;

public Camera uiCamera;

public UILabel text;

public UISprite background;

public float appearSpeed = 10f;

public bool scalingTransitions = true;

private Transform mTrans;

private float mTarget;

private float mCurrent;

private Vector3 mPos;

private Vector3 mSize;

private UIWidget[] mWidgets;

private void Awake()
{ 
UITooltip.mInstance = this;
}

private void OnDestroy()
{ 
UITooltip.mInstance = null;
}

private void Start()
{ 
this.mTrans = base.transform;
this.mWidgets = base.GetComponentsInChildren<UIWidget>();
this.mPos = this.mTrans.localPosition;
this.mSize = this.mTrans.localScale;
if (this.uiCamera == null)
{ 
this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
}
this.SetAlpha(0f);
}

private void Update()
{ 
if (this.mCurrent != this.mTarget)
{ 
this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, Time.deltaTime * this.appearSpeed);
if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
{ 
this.mCurrent = this.mTarget;
}
this.SetAlpha(this.mCurrent * this.mCurrent);
if (this.scalingTransitions)
{ 
Vector3 b = this.mSize * 0.25f;
b.y = -b.y;
Vector3 localScale = Vector3.one * (1.5f - this.mCurrent * 0.5f);
Vector3 localPosition = Vector3.Lerp(this.mPos - b, this.mPos, this.mCurrent);
this.mTrans.localPosition = localPosition;
this.mTrans.localScale = localScale;
}
}
b
localScale
localPosition
}

private void SetAlpha(float val)
{ 
int i = 0;
int num = this.mWidgets.Length;
while (i < num)
{ 
UIWidget uIWidget = this.mWidgets[i];
Color color = uIWidget.color;
color.a = val;
uIWidget.color = color;
i++;
}
i
num
uIWidget
color
}

private void SetText(string tooltipText)
{ 
if (this.text != null && !string.IsNullOrEmpty(tooltipText))
{ 
this.mTarget = 1f;
if (this.text != null)
{ 
this.text.text = tooltipText;
}
this.mPos = Input.mousePosition;
if (this.background != null)
{ 
Transform transform = this.background.transform;
Transform transform2 = this.text.transform;
Vector3 localPosition = transform2.localPosition;
Vector3 localScale = transform2.localScale;
this.mSize = this.text.relativeSize;
this.mSize.x = this.mSize.x * localScale.x;
this.mSize.y = this.mSize.y * localScale.y;
this.mSize.x = this.mSize.x + (this.background.border.x + this.background.border.z + (localPosition.x - this.background.border.x) * 2f);
this.mSize.y = this.mSize.y + (this.background.border.y + this.background.border.w + (-localPosition.y - this.background.border.y) * 2f);
this.mSize.z = 1f;
transform.localScale = this.mSize;
}
if (this.uiCamera != null)
{ 
this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
float num2 = (float)Screen.height * 0.5f / num;
Vector2 vector = new Vector2(num2 * this.mSize.x / (float)Screen.width, num2 * this.mSize.y / (float)Screen.height);
this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
this.mPos = this.mTrans.localPosition;
this.mPos.x = Mathf.Round(this.mPos.x);
this.mPos.y = Mathf.Round(this.mPos.y);
this.mTrans.localPosition = this.mPos;
}
else
{ 
if (this.mPos.x + this.mSize.x > (float)Screen.width)
{ 
this.mPos.x = (float)Screen.width - this.mSize.x;
}
if (this.mPos.y - this.mSize.y < 0f)
{ 
this.mPos.y = this.mSize.y;
}
this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
}
}
else
{ 
this.mTarget = 0f;
}
transform
transform2
localPosition
localScale
num
num2
vector
}

public static void ShowText(string tooltipText)
{ 
if (UITooltip.mInstance != null)
{ 
UITooltip.mInstance.SetText(tooltipText);
}
}
}
