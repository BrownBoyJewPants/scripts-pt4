using System;
using System.Collections.Generic;
using TileEngine.Pathfinding;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.UnitStates
{ 
public class MovingUnitState : UnitState
{ 
private MapCell target;

private MapCell[] path;

private int prevPosition;

private float position;

private Vector3 offset = new Vector3(0.5f, 0f, 0.5f);

private HashSet<Unit> knownUnits;

private int reserveTUs;

public bool stoppedDueToReactionFire;

public MovingUnitState(MapCell target, int reserveTUs = 0)
{ 
this.ismodal = true;
this.target = target;
this.reserveTUs = reserveTUs;
}

protected override void Initialize()
{ 
base.Initialize();
this.knownUnits = new HashSet<Unit>(this.unit.Visibility.units);
}

public override void Update()
{ 
if (this.unit.isFriendly)
{ 
this.game.gui.pathfield.Draw(this.unit, this.target);
}
if (this.path == null)
{ 
PathField pathField = this.unit.GetPathField();
this.path = pathField.GetPathTo(this.target);
if (this.path == null || this.path.Length == 0)
{ 
this.StopMoving();
return;
}
this.position = 0f;
this.prevPosition = 0;
}
this.MoveStep();
pathField
}

private MapCell GetPathPosition(int idx)
{ 
if (this.path.Length == 0)
{ 
return this.unit.Cell;
}
if (idx < 0)
{ 
return this.path[0];
}
if (idx >= this.path.Length)
{ 
return this.path[this.path.Length - 1];
}
return this.path[idx];
}

private void MoveStep()
{ 
if (!this.unit.isFriendly && !this.game.units.Friendlies.CanSee(this.GetPathPosition((int)this.position)) && !this.game.units.Friendlies.CanSee(this.GetPathPosition((int)this.position + 1)))
{ 
this.position += 0.5f;
}
else
{ 
this.position += this.unit.stats.MovementSpeed * Time.deltaTime;
}
int num = (int)this.position;
int num2 = num + 1;
float t = this.position - (float)num;
bool flag = false;
if (this.prevPosition != num)
{ 
this.unit.stats.timeUnits -= this.unit.stats.BaseMovementCost;
this.prevPosition = num;
this.unit.Visibility.isdirty = true;
this.unit.Visibility.Update();
this.unit.UpdateGui();
if (this.unit.isFriendly)
{ 
FogMap.needUpdate = true;
}
Unit[] units = this.unit.Visibility.units;
for (int i = 0; i < units.Length; i++)
{ 
Unit item = units[i];
if (this.knownUnits.Add(item))
{ 
flag = true;
}
}
if (base.CheckReactionFire())
{ 
flag = true;
this.stoppedDueToReactionFire = true;
}
}
if (flag || num2 >= this.path.Length || this.unit.stats.timeUnits - (float)this.reserveTUs < this.unit.stats.BaseMovementCost)
{ 
this.UpdateUnitPosition(num);
this.unit.SetModelPosition(this.path[num].worldposition + this.offset);
this.StopMoving();
return;
}
this.unit.SetModelPosition(Vector3.Lerp(this.path[num].worldposition, this.path[num2].worldposition, t) + this.offset);
this.unit.Model.transform.localRotation = Quaternion.LookRotation(this.path[num2].worldposition - this.path[num].worldposition);
if (!this.unit.isFriendly && this.game.units.Friendlies.CanSee(this.GetPathPosition(num2)))
{ 
CameraBase.ScrollTo(this.unit.Model.transform.position, false);
}
num
num2
t
flag
units
i
item
}

public override void EnterForeground()
{ 
this.unit.TriggerAnimation("StartRunning");
base.Start();
}

public override void EnterBackground()
{ 
this.unit.TriggerAnimation("StopRunning");
base.End();
}

private void UpdateUnitPosition(int pathIndex)
{ 
this.unit.SetPosition(this.GetPathPosition(pathIndex));
}

private void StopMoving()
{ 
this.unit.pathField = null;
this.path = null;
this.Pop();
}
}
}
