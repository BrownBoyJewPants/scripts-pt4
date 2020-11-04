using Game.Data.Items;
using Game.Data.Items.Weapons;
using Game.Gui;
using Game.Zombiescape.Commands;
using Game.Zombiescape.Data;
using System;
using UnityEngine;


namespace Game.Zombiescape.States.PlayerStates
{ 
public class PlayerSpecialState : PlayerState
{ 
private GuiBindings bindings;

private SpecialWeapon special;

private Weapon normalPrimaryWeapon;

private Weapon normalSecondaryWeapon;

protected override void Initialize()
{ 
base.Initialize();
GameObject model = Resources.Load<GameObject>("prefabs/AC130MainGunAmmo");
this.special = new SpecialWeapon("AC-130", new AC130MainGun(new AC130MainGunAmmo(this.player.game, model)), KeyCode.Alpha1, 50, 50, false);
base.RegisterHandler<UnitKilledCommand>(new Action<UnitKilledCommand>(this.UnitKilled));
this.normalPrimaryWeapon = this.player.primaryWeapon;
this.normalSecondaryWeapon = this.player.secondaryWeapon;
model
}

public override void EnterForeground()
{ 
this.player.primaryWeapon = this.normalPrimaryWeapon;
this.player.secondaryWeapon = this.normalSecondaryWeapon;
this.ResetStatus();
}

private void ResetStatus()
{ 
this.special.kills = 0;
this.bindings.UpdateValue("special.statustext", string.Empty);
this.bindings.UpdateValue("special.status", 0f, 1f, true);
}

private void UnitKilled(UnitKilledCommand cmd)
{ 
cmd.handled = false;
if (!this.isactive)
{ 
return;
}
this.special.kills++;
this.bindings.UpdateValue("special.status", (float)this.special.kills / (float)this.special.killsRequired, 1f, false);
if (this.special.kills == this.special.killsRequired)
{ 
this.Push(new PlayerSpecialWeaponReady(this.special));
}
}

public override void Start()
{ 
base.Start();
this.bindings = this.player.game.Services.Get<GuiBindings>();
this.ResetStatus();
}
}
}
