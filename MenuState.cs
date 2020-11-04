using Assets.Scripts.Game.MonoBehaviours;
using Game.Commands;
using Game.GameStates;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


namespace Game.States
{ 
public class MenuState : GameState
{ 
private Animator animator;

private GameObject menuRoot;

private bool closing;

private UIPanel[] pages;

private MenuPageButtonBehaviour[] buttons;

private UIPanel current;

protected bool doblur;

public MenuState(GameObject menuRoot)
{ 
this.menuRoot = menuRoot;
}

public override void Start()
{ 
this.menuRoot.SetActive(true);
this.InitializeButtons();
this.InitializePages();
this.animator = this.menuRoot.GetComponent<Animator>();
if (this.pages.Length > 0)
{ 
this.ShowPage(this.pages[0]);
}
if (this.doblur)
{ 
Blur component = Camera.main.GetComponent<Blur>();
if (component != null)
{ 
component.enabled = true;
}
}
base.Start();
component
}

public override void End()
{ 
if (this.doblur)
{ 
Blur component = Camera.main.GetComponent<Blur>();
if (component != null)
{ 
component.enabled = false;
}
}
component
}

private void InitializeButtons()
{ 
this.buttons = this.menuRoot.GetComponentsInChildren<MenuPageButtonBehaviour>().ToArray<MenuPageButtonBehaviour>();
MenuPageButtonBehaviour[] array = this.buttons;
for (int i = 0; i < array.Length; i++)
{ 
MenuPageButtonBehaviour menuPageButtonBehaviour = array[i];
menuPageButtonBehaviour.SetMenu(this);
}
array
i
menuPageButtonBehaviour
}

private void InitializePages()
{ 
this.pages = (from p in this.menuRoot.GetComponentsInChildren<UIPanel>()
where p.gameObject.tag == "MenuPage"
orderby p.gameObject.name
select p).ToArray<UIPanel>();
}

public void ShowPage(UIPanel page)
{ 
this.current = page;
for (int i = 0; i < this.pages.Length; i++)
{ 
this.pages[i].SetAlphaRecursive((float)((!(this.pages[i] == page)) ? 0 : 1), false);
}
for (int j = 0; j < this.buttons.Length; j++)
{ 
this.buttons[j].SetCurrent(this.buttons[j].page == page);
}
i
j
}

protected override void Initialize()
{ 
this.ismodal = true;
base.RegisterHandler<EscapeCommand>(new Action(this.CloseMenu));
}

protected void HideMenu()
{ 
this.animator.SetTrigger("HideMenu");
}

protected void CloseMenu()
{ 
for (int i = 0; i < this.buttons.Length; i++)
{ 
this.buttons[i].SetCurrent(false);
}
this.HideMenu();
this.game.Behaviour.StartCoroutine(this.WaitForMenuDone());
this.Pop();
i
}

[DebuggerHidden]
private IEnumerator WaitForMenuDone()
{ 
MenuState.<WaitForMenuDone>c__Iterator9 <WaitForMenuDone>c__Iterator = new MenuState.<WaitForMenuDone>c__Iterator9();
<WaitForMenuDone>c__Iterator.<>f__this = this;
return <WaitForMenuDone>c__Iterator;
<WaitForMenuDone>c__Iterator
}

public override void EnterForeground()
{ 
this.animator.SetTrigger("ShowMenu");
}
}
}
