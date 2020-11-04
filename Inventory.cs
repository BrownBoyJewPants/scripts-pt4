using System;


namespace Game.Data
{ 
public class Inventory
{ 
public readonly InventorySlot mainWeapon;

public readonly InventorySlot secondaryWeapon;

public Inventory()
{ 
this.mainWeapon = new InventorySlot();
this.secondaryWeapon = new InventorySlot();
}
}
}
