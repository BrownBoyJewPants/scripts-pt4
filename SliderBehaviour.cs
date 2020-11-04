using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Game.Global.MonoBehaviours.Buttons
{ 
public class SliderBehaviour : MonoBehaviour
{ 
public Action<float> callback;

public UISlider slider;

public IEnumerable<UILabel> labels;

private void OnSliderChange()
{ 
if (this.callback != null)
{ 
this.callback(this.slider.sliderValue);
}
if (this.labels == null)
{ 
return;
}
foreach (UILabel current in this.labels)
{ 
current.text = ((int)(this.slider.sliderValue * 100f)).ToString();
}
enumerator
current
}
}
}
