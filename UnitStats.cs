using System;


namespace Game
{ 
public class UnitStats
{ 
public float timeUnits;

public float stamina;

public int health;

public float sightrange;

public float shield;

public float accuracy;

public float bravery;

public int strength;

public int intelligence;

public int dexterity;

public float BaseMovementCost
{ 
get
{ 
return 4f;
}
}

public float MovementSpeed
{ 
get
{ 
return 4f;
}
}

public void CopyTo(UnitStats stats)
{ 
stats.timeUnits = this.timeUnits;
stats.stamina = this.stamina;
stats.health = this.health;
stats.sightrange = this.sightrange;
stats.shield = this.shield;
stats.accuracy = this.accuracy;
stats.strength = this.strength;
stats.bravery = this.bravery;
stats.intelligence = this.intelligence;
stats.dexterity = this.dexterity;
}
}
}
