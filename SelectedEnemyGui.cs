using System;


namespace Game.Gui
{ 
public class SelectedEnemyGui : SelectedUnitGui
{ 
public SelectedEnemyGui(GameGui gui) : base(gui)
{ 
}

public override void Draw(Unit unit)
{ 
this.gui.bindings.UpdateValue("debug.ai", string.Format("state: {0}\r\nvisible units: {1}", unit.State, unit.Visibility.units.Length));
base.Draw(unit);
}
}
}
