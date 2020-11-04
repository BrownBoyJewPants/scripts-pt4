using Game.Gui;
using Game.Zombiescape.Commands;
using Game.Zombiescape.Data;
using System;


namespace Game.Zombiescape.States.PlayerStates
{ 
public class PlayerSpecialWeaponActiveState : PlayerState
{ 
private SpecialWeapon weapon;

private GuiBindings bindings;

public PlayerSpecialWeaponActiveState(SpecialWeapon weapon)
{ 
this.weapon = weapon;
}

protected override void Initialize()
{ 
base.Initialize();
this.bindings = this.player.game.Services.Get<GuiBindings>();
base.RegisterHandler<FiredWeaponCommand>(new Action<FiredWeaponCommand>(this.FiredWeapon));
}

private void FiredWeapon(FiredWeaponCommand cmd)
{ 
if (cmd.weapon == this.weapon.weapon)
{ 
this.weapon.shotsMade++;
this.UpdateStatus();
if (this.weapon.shotsMade >= this.weapon.shots)
{ 
this.weapon.killsRequired = (int)((float)this.weapon.killsRequired * 1.5f);
this.Pop();
}
}
}

public override void Start()
{ 
base.Start();
this.weapon.shotsMade = 0;
if (this.weapon.isPrimary)
{ 
this.player.primaryWeapon = this.weapon.weapon;
}
else
{ 
this.player.secondaryWeapon = this.weapon.weapon;
}
this.UpdateStatus();
}

private void UpdateStatus()
{ 
this.bindings.UpdateValue("special.statustext", string.Concat(new object[]
{ 
this.weapon.name,
"   -   ",
this.weapon.shots - this.weapon.shotsMade,
" rounds left"
}));
this.bindings.UpdateValue("special.status", (float)(this.weapon.shots - this.weapon.shotsMade), (float)this.weapon.shots, true);
}
}
}
