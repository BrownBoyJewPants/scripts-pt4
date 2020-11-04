using Game;
using Game.States;
using System;
using UnityEngine;


namespace Assets.Scripts.Game.States.MenuStates
{ 
public class SoldierStatsMenuState : MenuState
{ 
private Unit soldier;

private new BattlescapeInfo game;

public SoldierStatsMenuState(GameObject root, Unit soldier) : base(root)
{ 
this.doblur = true;
this.ismodal = true;
this.soldier = soldier;
}

protected override void Initialize()
{ 
base.Initialize();
this.game = (BattlescapeInfo)this.game;
base.RegisterHandler<SoldierStatsButton>(SoldierStatsButton.Close, new Action(base.CloseMenu));
base.RegisterHandler<SoldierStatsButton>(SoldierStatsButton.NextSoldier, new Action(this.NextSoldier));
base.RegisterHandler<SoldierStatsButton>(SoldierStatsButton.PreviousSoldier, new Action(this.PreviousSoldier));
}

private void PreviousSoldier()
{ 
this.soldier = this.game.units.Friendlies.GetPrevious(this.soldier);
this.SetFieldValues();
}

private void NextSoldier()
{ 
this.soldier = this.game.units.Friendlies.GetNext(this.soldier);
this.SetFieldValues();
}

public override void Start()
{ 
this.SetFieldValues();
base.Start();
}

private void SetFieldValues()
{ 
this.game.gui.bindings.UpdateValue("stats.name", this.soldier.name);
this.game.gui.bindings.UpdateValue("stats.health", (float)this.soldier.maxStats.health, 120f, false);
this.game.gui.bindings.UpdateValue("stats.stamina", this.soldier.maxStats.stamina, 120f, false);
this.game.gui.bindings.UpdateValue("stats.strength", (float)this.soldier.maxStats.strength, 120f, false);
this.game.gui.bindings.UpdateValue("stats.timeunits", this.soldier.maxStats.timeUnits, 120f, false);
this.game.gui.bindings.UpdateValue("stats.accuracy", (float)((int)(this.soldier.maxStats.accuracy * 100f)), 120f, false);
this.game.gui.bindings.UpdateValue("stats.bravery", this.soldier.maxStats.bravery, 120f, false);
this.game.gui.bindings.UpdateValue("stats.dexterity", (float)this.soldier.maxStats.dexterity, 120f, false);
this.game.gui.bindings.UpdateValue("stats.intelligence", (float)this.soldier.maxStats.intelligence, 120f, false);
}
}
}
