using Game.States;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Game.GameStates
{ 
public class EnemyTurnGameState : GameState
{ 
private new BattlescapeInfo game;

private HashSet<Unit> unitsToProcess;

private Unit currentUnit;

public override void Start()
{ 
this.game = (BattlescapeInfo)this.game;
this.unitsToProcess = new HashSet<Unit>();
foreach (Unit current in this.game.units.Enemies)
{ 
this.unitsToProcess.Add(current);
current.ResetRoundStats();
current.Visibility.isdirty = true;
current.Visibility.Update();
}
enumerator
current
}

public override void Update()
{ 
while (true)
{ 
if (this.currentUnit == null || !this.currentUnit.IsAlive)
{ 
this.currentUnit = this.GetNextUnit();
if (this.currentUnit != null)
{ 
BattleScapeAIState battleScapeAIState = this.currentUnit.State.Current as BattleScapeAIState;
battleScapeAIState.RoundStart();
}
}
if (this.currentUnit == null)
{ 
break;
}
this.currentUnit.busy = false;
this.currentUnit.AIUpdate();
if (this.currentUnit.busy || this.currentUnit.State.IsModal)
{ 
return;
}
this.unitsToProcess.Remove(this.currentUnit);
this.currentUnit = null;
}
this.Pop();
battleScapeAIState
}

private Unit GetNextUnit()
{ 
while (this.unitsToProcess.Count != 0)
{ 
Unit unit = this.unitsToProcess.FirstOrDefault<Unit>();
if (unit.IsAlive)
{ 
return unit;
}
this.unitsToProcess.Remove(unit);
}
return null;
unit
}

public override string ToString()
{ 
if (this.currentUnit != null)
{ 
return "Unit AI->" + this.currentUnit.State;
}
return "Enemy turn";
}

public override void EnterForeground()
{ 
this.game.gui.bindings.SetVisible("game.enemyturn", true);
}

public override void EnterBackground()
{ 
this.game.gui.bindings.SetVisible("game.enemyturn", false);
}
}
}
