using System;


namespace Game.Gui
{ 
public abstract class SelectedUnitGui
{ 
public readonly GameGui gui;

public SelectedUnitGui(GameGui gui)
{ 
this.gui = gui;
}

public virtual void Draw(Unit unit)
{ 
this.gui.effects.DrawSelection(unit.Model.model);
}
}
}
