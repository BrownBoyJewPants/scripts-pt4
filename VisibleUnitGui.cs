using System;


namespace Game.Gui
{ 
public class VisibleUnitGui
{ 
private Healthbar healthbar;

public VisibleUnitGui(BattlescapeInfo game)
{ 
this.healthbar = new Healthbar();
}

public void Draw(Unit unit)
{ 
this.healthbar.Draw(unit);
}
}
}
