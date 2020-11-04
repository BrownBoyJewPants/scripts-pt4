using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Gui
{ 
public class Healthbar
{ 
private class UnitHealthBar
{ 
private Unit unit;

private Healthbar parent;

private List<SpriteRenderer> elements;

private int prevHealth;

private GameObject healthbar;

public UnitHealthBar(Healthbar parent, Unit unit)
{ 
this.unit = unit;
this.parent = parent;
this.healthbar = new GameObject("healthbar");
this.healthbar.transform.parent = unit.Model.transform;
this.healthbar.transform.localPosition = new Vector3(0f, 1.5f, 0f);
this.healthbar.transform.localRotation = Camera.main.transform.rotation;
float num = 0.12f;
float num2 = 0.025f;
float num3 = num2 + num;
float num4 = (float)(-(float)unit.maxStats.health) * 0.5f * num3;
this.elements = new List<SpriteRenderer>();
for (int i = 0; i < unit.maxStats.health; i++)
{ 
SpriteRenderer spriteRenderer = new GameObject("Health" + i)
{ 
transform = 
{ 
parent = this.healthbar.transform,
localPosition = new Vector3(num4 + (float)i * num3, 0f, 0f),
localScale = new Vector3(num, num, num),
localRotation = Quaternion.identity
},
layer = 11
}.AddComponent<SpriteRenderer>();
spriteRenderer.sprite = ((i >= unit.stats.health) ? parent.barEmpty : ((!unit.isFriendly) ? parent.barRed : parent.barGreen));
this.elements.Add(spriteRenderer);
}
this.prevHealth = unit.stats.health;
num
num2
num3
num4
i
spriteRenderer
}

public void Update()
{ 
this.healthbar.transform.rotation = Camera.main.transform.rotation;
if (this.prevHealth == this.unit.stats.health)
{ 
return;
}
for (int i = 0; i < this.unit.maxStats.health; i++)
{ 
this.elements[i].sprite = ((i >= this.unit.stats.health) ? this.parent.barEmpty : ((!this.unit.isFriendly) ? this.parent.barRed : this.parent.barGreen));
}
this.prevHealth = this.unit.stats.health;
i
}
}

private Sprite barGreen;

private Sprite barRed;

private Sprite barEmpty;

private Dictionary<Unit, Healthbar.UnitHealthBar> healthbars;

public Healthbar()
{ 
this.barGreen = Resources.Load<Sprite>("sprites/barGreenMid");
this.barRed = Resources.Load<Sprite>("sprites/barRedMid");
this.barEmpty = Resources.Load<Sprite>("sprites/barBackMid");
this.healthbars = new Dictionary<Unit, Healthbar.UnitHealthBar>();
}

public void Draw(Unit unit)
{ 
}
}
}
