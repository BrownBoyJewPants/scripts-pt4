using System;


namespace Game.Gui
{ 
public class SelectedFriendlyGui : SelectedUnitGui
{ 
private int prevVisible = -1;

public SelectedFriendlyGui(GameGui gui) : base(gui)
{ 
}

public override void Draw(Unit unit)
{ 
base.Draw(unit);
this.gui.bindings.UpdateValue("friendly.health", (float)unit.stats.health, (float)unit.maxStats.health, true);
this.gui.bindings.UpdateValue("friendly.timeunits", unit.stats.timeUnits, unit.maxStats.timeUnits, true);
this.gui.bindings.UpdateValue("friendly.shield", unit.stats.shield, unit.maxStats.shield, true);
this.gui.bindings.UpdateValue("friendly.name", unit.name);
int num = unit.Visibility.units.Length;
if (this.prevVisible != num)
{ 
for (int i = 1; i <= 14; i++)
{ 
this.gui.bindings.SetVisible("enemy." + i, num >= i);
}
this.prevVisible = num;
}
num
i
}
}
}
