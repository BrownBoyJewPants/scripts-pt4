using Game.Commands;
using Game.States;
using System;
using UnityEngine;


namespace Zombiescape.States.GameStates
{ 
public class InvalidDomainMenuState : MenuState
{ 
public InvalidDomainMenuState(GameObject menuRoot) : base(menuRoot)
{ 
this.ismodal = true;
}

public override void HandleCommand(Command command)
{ 
command.handled = true;
}

public override void Update()
{ 
base.Update();
if (Input.GetKeyDown(KeyCode.Space))
{ 
Application.ExternalEval("document.location='http://devoga.itch.io/critical-mass';");
}
}
}
}
