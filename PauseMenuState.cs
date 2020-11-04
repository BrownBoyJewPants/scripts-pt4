using System;
using UnityEngine;


namespace Game.States
{ 
public class PauseMenuState : MenuState
{ 
public PauseMenuState(GameObject root) : base(root)
{ 
this.doblur = true;
}

protected override void Initialize()
{ 
base.Initialize();
base.RegisterHandler<PauseMenuButton>(PauseMenuButton.ShowOptionsMenu, new Action(this.ShowOptions));
base.RegisterHandler<PauseMenuButton>(PauseMenuButton.QuitToDesktop, new Action(this.QuitToDesktop));
base.RegisterHandler<PauseMenuButton>(PauseMenuButton.ReturnToGame, new Action(base.CloseMenu));
}

private void ShowOptions()
{ 
base.HideMenu();
this.Push(new OptionsMenuState(this.game.Behaviour.Menu.FindMenu("OptionsMenu")));
}

private void QuitToDesktop()
{ 
Application.Quit();
}
}
}
