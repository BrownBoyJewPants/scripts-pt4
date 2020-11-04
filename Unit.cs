using Game.Commands;
using Game.Data;
using Game.Data.Items;
using Game.States;
using Game.UnitStates;
using System;
using TileEngine.Pathfinding;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine.Models;


namespace Game
{ 
public class Unit : IHasVisibilityInfo, IUnit
{ 
private static int uniqueId;

public int index;

public readonly Inventory inventory;

public readonly string name;

public readonly UnitStats maxStats;

public readonly UnitStats stats;

public bool selected;

public bool isFriendly;

private Animator animator;

public GameObject weaponModel;

private MapCell cell;

public Color32 bloodColor = new Color32(64, 0, 0, 128);

public UnitArmor armor;

public PathField pathField;

public bool busy;

IStateMachine IUnit.State
{ 
get
{ 
return this.State;
}
}

Weapon IUnit.MainWeapon
{ 
get
{ 
return this.inventory.mainWeapon.item as Weapon;
}
}

public int Id
{ 
get;
private set;
}

public bool IsDisabled
{ 
get;
set;
}

public VoxelModel Model
{ 
get;
private set;
}

public UnitStateMachine State
{ 
get;
private set;
}

public BattlescapeUnitManager Manager
{ 
get;
private set;
}

public VisibilityInfo Visibility
{ 
get;
private set;
}

public Vector3 Velocity
{ 
get
{ 
return Vector3.zero;
}
}

public MapCell Cell
{ 
get
{ 
return this.cell;
}
}

public bool IsAlive
{ 
get
{ 
return this.stats.health > 0;
}
}

public Color32 BloodColor
{ 
get
{ 
return this.bloodColor;
}
}

public float Speed
{ 
get
{ 
return 6f;
}
}

public bool IsFriendly
{ 
get
{ 
return this.isFriendly;
}
}

public Unit(string name, UnitStats stats)
{ 
this.Id = Unit.uniqueId++;
this.name = name;
this.maxStats = stats;
this.stats = new UnitStats();
this.inventory = new Inventory();
this.armor = default(UnitArmor);
this.maxStats.CopyTo(this.stats);
}

public void Init(BattlescapeUnitManager manager)
{ 
this.Manager = manager;
this.Visibility = new VisibilityInfo(this);
this.State = new UnitStateMachine(manager.Game, this);
this.State.Push(new NullUnitState());
}

public void Disable()
{ 
}

public void AIUpdate()
{ 
this.State.AIUpdate();
this.Manager.Game.gui.unitGui.Draw(this);
}

public void Update()
{ 
this.State.Update();
}

public void FixedUpdate()
{ 
this.State.FixedUpdate();
}

public PathField GetPathField()
{ 
if (this.pathField == null)
{ 
this.pathField = new PathField(this.cell, 50, new Func<MapCell, bool>(this.CheckCellOpen), null);
}
return this.pathField;
}

private bool CheckCellOpen(MapCell cell)
{ 
Unit unitAt = this.Manager.GetUnitAt(cell);
return unitAt == null || unitAt == this;
unitAt
}

public void ResetRoundStats()
{ 
this.stats.timeUnits = this.maxStats.timeUnits;
}

public void SetPosition(MapCell cell)
{ 
if (this.Manager != null)
{ 
this.Manager.RemoveUnitAtPosition(this);
}
this.cell = cell;
if (this.Manager != null)
{ 
this.Manager.AddUnitAtPosition(this);
this.Manager.isDirty = true;
}
}

public override string ToString()
{ 
return this.name;
}

public override int GetHashCode()
{ 
return this.Id;
}

public void TakeDamage(DamageType type, float amount, Vector3 direction)
{ 
if (!this.IsAlive)
{ 
return;
}
int front = this.armor.front;
int num = (int)((amount - (float)front) * UnityEngine.Random.Range(0.5f, 1.5f));
if (num < 0)
{ 
return;
}
this.stats.health = Mathf.Max(0, this.stats.health - num);
if (!this.IsAlive)
{ 
this.Manager.Destroy(this);
this.Manager.Game.state.SendCommand(new UnitKilledCommand(this));
this.TriggerAnimation("Die");
this.stats.timeUnits = 0f;
this.stats.shield = 0f;
}
this.UpdateGui();
front
num
}

public bool DoAction(float timeUnitCost, float staminaCost, Action action)
{ 
if (timeUnitCost > this.stats.timeUnits)
{ 
return false;
}
if (staminaCost > this.stats.stamina)
{ 
return false;
}
this.stats.timeUnits -= timeUnitCost;
this.stats.stamina -= staminaCost;
action();
this.UpdateGui();
return true;
}

public void SetModelPosition(Vector3 position)
{ 
this.Model.transform.position = position;
MapCell mapCell = this.Manager.Game.map[position];
mapCell
}

public bool LookAt(MapCell cell, bool isFree = false)
{ 
return cell == null || cell == this.cell || this.DoAction((!isFree) ? 2f : 0f, 0f, delegate
{ 
Vector3 forward = cell.worldposition - this.cell.worldposition;
forward.y = 0f;
this.Model.transform.localRotation = Quaternion.LookRotation(forward);
this.Visibility.isdirty = true;
if (this.isFriendly)
{ 
FogMap.needUpdate = true;
}
forward
});
<LookAt>c__AnonStorey1E
}

internal float GetAccuracy(Weapon weapon)
{ 
return this.stats.accuracy * weapon.accuracy;
}

internal bool CanFire(Weapon weapon, Unit target)
{ 
return weapon != null && (float)this.NeededTimeUnits(weapon) <= this.stats.timeUnits;
}

public int NeededTimeUnits(Weapon weapon)
{ 
return (int)(weapon.attackCost * this.maxStats.timeUnits);
}

internal Vector3 GetFiringPosition(Weapon weapon)
{ 
return this.Model.transform.position + new Vector3(0f, 0.75f, 0f);
}

internal float GetInitiative()
{ 
return (float)this.stats.dexterity / 100f * (this.stats.timeUnits / this.maxStats.timeUnits);
}

internal bool CanSee(Unit unit)
{ 
return this.Visibility.IsVisible(unit);
}

internal void TriggerAnimation(string trigger)
{ 
if (this.Model.animator == null)
{ 
return;
}
this.Model.animator.SetTrigger(trigger);
}

public void UpdateGui()
{ 
if (!this.isFriendly)
{ 
return;
}
string str = "unit." + this.index + ".";
this.Manager.Game.gui.bindings.UpdateValue(str + "health", (float)this.stats.health, (float)this.maxStats.health, true);
this.Manager.Game.gui.bindings.UpdateValue(str + "shield", this.stats.shield, this.maxStats.shield, true);
this.Manager.Game.gui.bindings.UpdateValue(str + "timeunits", this.stats.timeUnits, this.maxStats.timeUnits, true);
this.Manager.Game.gui.bindings.SetColor(str + "selected", (!this.selected) ? new Color32(122, 122, 122, 255) : new Color32(255, 255, 255, 255));
str
}

public Ray GetFiringRay(Weapon weapon, ref Vector3 target)
{ 
Vector3 firingPosition = this.GetFiringPosition(weapon);
return HelperFunctions.GetFiringRay(ref firingPosition, ref target, this.GetAccuracy(weapon));
firingPosition
}
}
}
