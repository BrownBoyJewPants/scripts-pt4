using System;


namespace Game.States
{ 
public class RootAIState : BattleScapeAIState
{ 
public override void AIUpdate()
{ 
if (base.GetVisibleEnemyCount() > 0)
{ 
this.Push(new AttackAIState());
return;
}
this.Push(new PatrolAIState());
}
}
}
