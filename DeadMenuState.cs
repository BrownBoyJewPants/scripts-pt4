using Assets.Scripts.Achievements;
using Game.Commands;
using Game.Gui;
using Game.States;
using System;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Zombiescape.States
{ 
public class DeadMenuState : MenuState
{ 
public DeadMenuState(GameObject menuroot) : base(menuroot)
{ 
this.ismodal = true;
this.doblur = true;
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
this.game.Services.Get<GuiBindings>().UpdateValue("deadmessage", scoreboard.Username + ", You are dead...");
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
if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.touchCount >= 3)
{ 
this.game.Services.Get<IScoreboard>().Dispose();
this.game.Services.Dispose();
this.game.Map.Dispose();
ResourceUpdater.Dispose();
Application.LoadLevel(Application.loadedLevel);
GC.Collect();
}
base.Update();
}
}
}
