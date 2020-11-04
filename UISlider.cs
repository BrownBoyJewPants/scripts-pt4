using System;
using UnityEngine;


[AddComponentMenu("NGUI/Interaction/Slider")]
public class UISlider : IgnoreTimeScale
{ 
public enum Direction
{ 
Horizontal,
Vertical
}

public delegate void OnValueChange(float val);

public static UISlider current;

public Transform foreground;

public Transform thumb;

public UISlider.Direction direction;

public GameObject eventReceiver;

public string functionName = "OnSliderChange";

public UISlider.OnValueChange onValueChange;

public int numberOfSteps;

[HideInInspector, SerializeField]
private float rawValue = 1f;

private BoxCollider mCol;

private Transform mTrans;

private Transform mFGTrans;

private UIWidget mFGWidget;

private UISprite mFGFilled;

private bool mInitDone;

private Vector2 mSize = Vector2.zero;

private Vector2 mCenter = Vector3.zero;

public float sliderValue
{ 
get
{ 
float num = this.rawValue;
if (this.numberOfSteps > 1)
{ 
num = Mathf.Round(num * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
}
return num;
num
}
set
{ 
this.Set(value, false);
}
}

public Vector2 fullSize
{ 
get
{ 
return this.mSize;
}
set
{ 
if (this.mSize != value)
{ 
this.mSize = value;
this.ForceUpdate();
}
}
}

private void Init()
{ 
this.mInitDone = true;
if (this.foreground != null)
{ 
this.mFGWidget = this.foreground.GetComponent<UIWidget>();
this.mFGFilled = ((!(this.mFGWidget != null)) ? null : (this.mFGWidget as UISprite));
this.mFGTrans = this.foreground.transform;
if (this.mSize == Vector2.zero)
{ 
this.mSize = this.foreground.localScale;
}
if (this.mCenter == Vector2.zero)
{ 
this.mCenter = this.foreground.localPosition + this.foreground.localScale * 0.5f;
}
}
else if (this.mCol != null)
{ 
if (this.mSize == Vector2.zero)
{ 
this.mSize = this.mCol.size;
}
if (this.mCenter == Vector2.zero)
{ 
this.mCenter = this.mCol.center;
}
}
else
{ 
Debug.LogWarning("UISlider expected to find a foreground object or a box collider to work with", this);
}
}

private void Awake()
{ 
this.mTrans = base.transform;
this.mCol = (base.collider as BoxCollider);
}

private void Start()
{ 
this.Init();
if (Application.isPlaying && this.thumb != null && this.thumb.collider != null)
{ 
UIEventListener uIEventListener = UIEventListener.Get(this.thumb.gameObject);
UIEventListener expr_49 = uIEventListener;
expr_49.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_49.onPress, new UIEventListener.BoolDelegate(this.OnPressThumb));
UIEventListener expr_6B = uIEventListener;
expr_6B.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_6B.onDrag, new UIEventListener.VectorDelegate(this.OnDragThumb));
}
this.Set(this.rawValue, true);
uIEventListener
expr_49
expr_6B
}

private void OnPress(bool pressed)
{ 
if (base.enabled && pressed && UICamera.currentTouchID != -100)
{ 
this.UpdateDrag();
}
}

private void OnDrag(Vector2 delta)
{ 
if (base.enabled)
{ 
this.UpdateDrag();
}
}

private void OnPressThumb(GameObject go, bool pressed)
{ 
if (base.enabled && pressed)
{ 
this.UpdateDrag();
}
}

private void OnDragThumb(GameObject go, Vector2 delta)
{ 
if (base.enabled)
{ 
this.UpdateDrag();
}
}

private void OnKey(KeyCode key)
{ 
if (base.enabled)
{ 
float num = ((float)this.numberOfSteps <= 1f) ? 0.125f : (1f / (float)(this.numberOfSteps - 1));
if (this.direction == UISlider.Direction.Horizontal)
{ 
if (key == KeyCode.LeftArrow)
{ 
this.Set(this.rawValue - num, false);
}
else if (key == KeyCode.RightArrow)
{ 
this.Set(this.rawValue + num, false);
}
}
else if (key == KeyCode.DownArrow)
{ 
this.Set(this.rawValue - num, false);
}
else if (key == KeyCode.UpArrow)
{ 
this.Set(this.rawValue + num, false);
}
}
num
}

private void UpdateDrag()
{ 
if (this.mCol == null || UICamera.currentCamera == null || UICamera.currentTouch == null)
{ 
return;
}
UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
Plane plane = new Plane(this.mTrans.rotation * Vector3.back, this.mTrans.position);
float distance;
if (!plane.Raycast(ray, out distance))
{ 
return;
}
Vector3 b = this.mTrans.localPosition + (this.mCenter - this.mSize * 0.5f);
Vector3 b2 = this.mTrans.localPosition - b;
Vector3 a = this.mTrans.InverseTransformPoint(ray.GetPoint(distance));
Vector3 vector = a + b2;
this.Set((this.direction != UISlider.Direction.Horizontal) ? (vector.y / this.mSize.y) : (vector.x / this.mSize.x), false);
ray
plane
distance
b
b2
a
vector
}

private void Set(float input, bool force)
{ 
if (!this.mInitDone)
{ 
this.Init();
}
float num = Mathf.Clamp01(input);
if (num < 0.001f)
{ 
num = 0f;
}
float sliderValue = this.sliderValue;
this.rawValue = num;
float sliderValue2 = this.sliderValue;
if (force || sliderValue != sliderValue2)
{ 
Vector3 localScale = this.mSize;
if (this.direction == UISlider.Direction.Horizontal)
{ 
localScale.x *= sliderValue2;
}
else
{ 
localScale.y *= sliderValue2;
}
if (this.mFGFilled != null && this.mFGFilled.type == UISprite.Type.Filled)
{ 
this.mFGFilled.fillAmount = sliderValue2;
}
else if (this.foreground != null)
{ 
this.mFGTrans.localScale = localScale;
if (this.mFGWidget != null)
{ 
if (sliderValue2 > 0.001f)
{ 
this.mFGWidget.enabled = true;
this.mFGWidget.MarkAsChanged();
}
else
{ 
this.mFGWidget.enabled = false;
}
}
}
if (this.thumb != null)
{ 
Vector3 localPosition = this.thumb.localPosition;
if (this.mFGFilled != null && this.mFGFilled.type == UISprite.Type.Filled)
{ 
if (this.mFGFilled.fillDirection == UISprite.FillDirection.Horizontal)
{ 
localPosition.x = ((!this.mFGFilled.invert) ? localScale.x : (this.mSize.x - localScale.x));
}
else if (this.mFGFilled.fillDirection == UISprite.FillDirection.Vertical)
{ 
localPosition.y = ((!this.mFGFilled.invert) ? localScale.y : (this.mSize.y - localScale.y));
}
else
{ 
Debug.LogWarning("Slider thumb is only supported with Horizontal or Vertical fill direction", this);
}
}
else if (this.direction == UISlider.Direction.Horizontal)
{ 
localPosition.x = localScale.x;
}
else
{ 
localPosition.y = localScale.y;
}
this.thumb.localPosition = localPosition;
}
UISlider.current = this;
if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName) && Application.isPlaying)
{ 
this.eventReceiver.SendMessage(this.functionName, sliderValue2, SendMessageOptions.DontRequireReceiver);
}
if (this.onValueChange != null)
{ 
this.onValueChange(sliderValue2);
}
UISlider.current = null;
}
num
sliderValue
sliderValue2
localScale
localPosition
}

public void ForceUpdate()
{ 
this.Set(this.rawValue, true);
}
}
