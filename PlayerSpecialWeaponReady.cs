using Game.Gui;
using Game.Zombiescape.Data;
using System;
using UnityEngine;


namespace Game.Zombiescape.States.PlayerStates
{ 
public class PlayerSpecialWeaponReady : PlayerState
{ 
private SpecialWeapon weapon;

public PlayerSpecialWeaponReady(SpecialWeapon weapon)
{ 
this.weapon = weapon;
}

public override void Start()
{ 
base.Start();
this.player.game.Services.Get<GuiBindings>().UpdateValue("special.statustext", this.weapon.name + " ready!");
}

public override void Update()
{ 
base.Update();
if (Input.GetKeyDown(this.weapon.keyToActivate))
{ 
this.Set(new PlayerSpecialWeaponActiveState(this.weapon));
}
}
}
}
