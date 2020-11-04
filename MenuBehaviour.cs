using Game.Commands;
using Game.Data;
using Game.States;
using System;
using System.Linq;
using UnityEngine;


namespace Game.Global.MonoBehaviours
{ 
public class MenuBehaviour : MonoBehaviour
{ 
public MonoBehaviour gamebehaviour;

private GameObject[] menuRoots;

private IGameInfo game;

private void Start()
{ 
this.FindMenus();
}

private void FindMenus()
{ 
if (this.menuRoots != null)
{ 
return;
}
this.menuRoots = GameObject.FindGameObjectsWithTag("Menu");
GameObject[] array = this.menuRoots;
for (int i = 0; i < array.Length; i++)
{ 
GameObject gameObject = array[i];
gameObject.SetActive(false);
}
this.game = ((IGameBehaviour)this.gamebehaviour).Game;
array
i
gameObject
}

public GameObject FindMenu(string name)
{ 
this.FindMenus();
GameObject gameObject = this.menuRoots.FirstOrDefault((GameObject r) => r.name == name);
if (gameObject == null)
{ 
Debug.Log("Menu " + name + " + not found!");
}
return gameObject;
<FindMenu>c__AnonStorey2A
gameObject
}

private void Update()
{ 
if (Input.GetKeyDown(KeyCode.Escape))
{ 
EscapeCommand escapeCommand = new EscapeCommand();
this.game.State.SendCommand(escapeCommand);
if (!escapeCommand.handled)
{ 
this.game.State.Push(new PauseMenuState(((IGameBehaviour)this.gamebehaviour).Menu.FindMenu("MainMenu")));
}
}
escapeCommand
}
}
}
