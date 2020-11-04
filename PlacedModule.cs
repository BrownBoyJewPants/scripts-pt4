using System;


namespace Game.Data
{ 
public class PlacedModule
{ 
public readonly BaseModule module;

public readonly IntVector2 position;

public readonly IntRect2 bounds;

public PlacedModule(BaseModule module, IntVector2 position)
{ 
this.module = module;
this.position = position;
this.bounds = new IntRect2(position, position + new IntVector2(module.template.dx - 1, module.template.dz - 1));
}

public override string ToString()
{ 
return this.module.name + " " + this.position;
}
}
}
