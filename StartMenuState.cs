using Assets.Scripts;
using Assets.Scripts.Achievements;
using Game.Commands;
using Game.Gui;
using Game.States;
using Game.Zombiescape;
using Game.Zombiescape.States;
using System;
using UnityEngine;


namespace Zombiescape.States.GameStates
{ 
public class StartMenuState : MenuState
{ 
public StartMenuState(GameObject menu) : base(menu)
{ 
this.ismodal = true;
this.doblur = false;
}

protected override void Initialize()
{ 
base.RegisterHandler<EscapeCommand>(new Action<EscapeCommand>(this.EscapePressed));
}

public override void Start()
{ 
base.Start();
IScoreboard scoreboard = this.game.Services.Get<IScoreboard>();
if (scoreboard.Connected)
{ 
this.game.Services.Get<GuiBindings>().UpdateValue("startmessage", scoreboard.Username + ", get ready to start...");
}
scoreboard
}

private void EscapePressed(EscapeCommand cmd)
{ 
base.HideMenu();
cmd.handled = false;
}

public override void Update()
{ 
bool keyDown = Input.GetKeyDown(KeyCode.P);
if (keyDown || Input.GetKeyDown(KeyCode.Space) || Input.touchCount >= 2)
{ 
PlayingState.painterEdition = keyDown;
Camera.main.GetComponent<MusicPlayer>().enabled = true;
base.HideMenu();
this.Set(new PlayingState((ZombieUnitManager)this.game.Units));
}
base.Update();
keyDown
}
}
}
