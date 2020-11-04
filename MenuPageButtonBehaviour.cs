using Game.States;
using System;
using UnityEngine;


namespace Assets.Scripts.Game.MonoBehaviours
{ 
public class MenuPageButtonBehaviour : MonoBehaviour
{ 
public UIPanel page;

public Color32 selectedColor;

public Color32 selectedHighlightColor;

private Color32 defaultColor;

private Color32 defaultHighlightColor;

private UISprite sprite;

private UIButton button;

private MenuState menu;

public void SetCurrent(bool current)
{ 
if (this.sprite != null)
{ 
this.sprite.color = ((!current) ? this.defaultColor : this.selectedColor);
}
if (this.button != null)
{ 
this.button.defaultColor = ((!current) ? this.defaultColor : this.selectedColor);
this.button.hover = ((!current) ? this.defaultHighlightColor : this.selectedHighlightColor);
}
}

private void OnClick()
{ 
if (this.menu != null && this.page != null)
{ 
this.menu.ShowPage(this.page);
this.button.UpdateColor(true, false);
}
}

internal void SetMenu(MenuState menu)
{ 
this.sprite = base.gameObject.GetComponentInChildren<UISprite>();
if (this.sprite != null)
{ 
this.defaultColor = this.sprite.color;
}
this.button = base.gameObject.GetComponent<UIButton>();
if (this.button != null)
{ 
this.button.defaultColor = this.defaultColor;
this.defaultHighlightColor = this.button.hover;
}
this.menu = menu;
}
}
}
