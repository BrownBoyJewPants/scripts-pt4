using Assets.Scripts.Achievements;
using Game.Data.Items;
using Game.Global;
using Game.Gui;
using Game.Zombiescape.Commands;
using Game.Zombiescape.Monobehaviours;
using System;
using TileEngine.Pathfinding;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Zombiescape.States.PlayerStates
{ 
public class PlayerControlState : PlayerState
{ 
private bool moving;

private bool moved;

private int kills;

private bool firing;

private float lastFireTime;

private AmmoManager ammo;

private Material material;

private PathFieldGui fieldGui;

private GuiBindings bindings;

private Map map;

private MapCell lastCell;

private ZombieUnitManager units;

private GravityBehaviour gravity;

private AudioGui audio;

private SpecialEffects effects;

private bool scoreSaved;

protected override void Initialize()
{ 
base.RegisterHandler<PlayerMovementCommand>(new Action<PlayerMovementCommand>(this.HandleMovement));
base.RegisterHandler<PlayerInputCommand>(new Action<PlayerInputCommand>(this.HandleInput));
base.RegisterHandler<PlayerFireWeaponCommand>(new Action<PlayerFireWeaponCommand>(this.FireWeaponCommand));
base.RegisterHandler<UnitKilledCommand>(new Action<UnitKilledCommand>(this.UnitKilled));
base.RegisterHandler<TookDamageCommand>(new Action<TookDamageCommand>(this.TookDamage));
this.audio = this.player.game.Services.Get<AudioGui>();
this.ammo = this.player.game.Services.Get<AmmoManager>();
this.effects = this.player.game.Services.Get<SpecialEffects>();
this.units = (ZombieUnitManager)this.player.game.Units;
this.map = this.player.game.Map;
this.bindings = this.player.game.Services.Get<GuiBindings>();
this.material = Resources.Load<Material>("materials/whitematerial");
this.fieldGui = new PathFieldGui(this.material);
this.subState = new PlayerStateMachine(this.player);
this.subState.Push(new PlayerSpecialState());
base.Initialize();
}

public override void Start()
{ 
this.player.Model.animator.SetBool("HoldingRifle", true);
}

private void FireWeaponCommand(PlayerFireWeaponCommand cmd)
{ 
if (!this.firing)
{ 
this.player.Model.animator.SetBool("FiringRifle", true);
this.firing = true;
}
Vector3 forward = cmd.target - this.player.primaryWeapon.FirePoint.position;
forward.y = 0f;
Vector3 vector = this.player.primaryWeapon.FirePoint.position - this.player.Model.transform.position;
vector.y = 0f;
if (vector.magnitude > forward.magnitude)
{ 
forward = cmd.target - this.player.Model.transform.position;
forward.y = 0f;
}
forward.Normalize();
this.player.Model.transform.forward = forward;
this.ammo.FireWeapon(this.player, cmd.weapon, cmd.target, true);
this.lastFireTime = Time.realtimeSinceStartup;
forward
vector
}

private void TookDamage(TookDamageCommand obj)
{ 
this.effects.DrawFloatingText(this.player.Model.transform.position + new Vector3(0f, 1.5f, 0f), new Color32(250, 100, 100, 255), 2f, (-obj.amount).ToString(), 100f);
}

private void UnitKilled(UnitKilledCommand cmd)
{ 
this.kills++;
this.bindings.UpdateValue("player.kills", this.kills);
this.audio.PlaySound("kill", Camera.main.transform.position, 1f);
this.effects.DrawFloatingText(cmd.unit.Model.transform.position + new Vector3(0f, 1.5f, 0f), Color.yellow, 1f, this.kills + string.Empty, 100f);
cmd.handled = false;
}

private void HandleInput(PlayerInputCommand cmd)
{ 
InputType type = cmd.type;
if (type != InputType.FirePrimary)
{ 
if (type == InputType.FireSecondary)
{ 
this.FireWeapon(this.player.secondaryWeapon, cmd.forceAimAtGround);
}
}
else
{ 
this.FireWeapon(this.player.primaryWeapon, cmd.forceAimAtGround);
}
type
}

private void FireWeapon(Weapon weapon, bool forceAimAtGround)
{ 
if (!this.firing)
{ 
this.player.Model.animator.SetBool("FiringRifle", true);
this.firing = true;
}
bool firingAtGround = weapon.aimAtGround || forceAimAtGround;
Vector3 target = this.player.GetTarget(ref firingAtGround);
weapon.firingAtGround = firingAtGround;
Vector3 forward = target - this.player.primaryWeapon.FirePoint.position;
forward.y = 0f;
Vector3 vector = this.player.primaryWeapon.FirePoint.position - this.player.Model.transform.position;
vector.y = 0f;
if (vector.magnitude > forward.magnitude)
{ 
forward = target - this.player.Model.transform.position;
forward.y = 0f;
}
forward.Normalize();
this.player.Model.transform.forward = forward;
if (this.ammo.FireWeapon(this.player, weapon, target, true))
{ 
this.subState.SendCommand(new FiredWeaponCommand(weapon));
}
this.lastFireTime = Time.realtimeSinceStartup;
firingAtGround
target
forward
vector
}

private void HandleMovement(PlayerMovementCommand cmd)
{ 
Vector3 delta = cmd.delta;
Map map = this.player.game.Map;
Bounds bounds = new Bounds(this.player.Model.collider.bounds.center, new Vector3(0.6f, this.player.Model.collider.bounds.size.y, 0.6f));
map.AdjustMovementToAllowed(ref delta, bounds, 0.14f);
this.player.Model.transform.position += delta;
this.player.Velocity = delta / Time.deltaTime;
if (delta.sqrMagnitude > 0f)
{ 
delta.y = 0f;
delta.Normalize();
if (!this.firing)
{ 
this.player.Model.transform.localRotation = Quaternion.LookRotation(delta);
this.player.Model.animator.SetBool("Strafing", false);
}
else
{ 
delta.Normalize();
float num = Mathf.Abs(Vector3.Dot(this.player.Model.transform.forward, delta));
this.player.Model.animator.SetBool("Strafing", true);
this.player.Model.animator.SetLayerWeight(3, 1f - num);
}
}
this.moved = true;
delta
map
bounds
num
}

public override void Update()
{ 
if (!this.isactive)
{ 
return;
}
base.Update();
this.subState.Update();
this.bindings.UpdateValue("player.health", this.player.life, this.player.maxlife, true);
if (!this.player.IsAlive)
{ 
if (!this.scoreSaved)
{ 
this.player.game.Services.Get<IScoreboard>().AddHighscore((uint)this.kills);
this.scoreSaved = true;
}
this.Push(new PlayerDeadState());
return;
}
if (!this.moved)
{ 
this.player.Velocity = Vector3.zero;
}
if (this.moved ^ this.moving)
{ 
this.player.Model.animator.SetBool("Running", this.moved);
if (!this.moved)
{ 
this.player.Model.animator.SetBool("Strafing", false);
}
}
if (this.firing && Time.realtimeSinceStartup - this.lastFireTime > 0.1f)
{ 
this.player.Model.animator.SetBool("FiringRifle", false);
this.firing = false;
}
this.moving = this.moved;
this.moved = false;
if (this.lastCell != this.player.Cell)
{ 
if (this.lastCell != null)
{ 
this.units.RemoveUnitAt(this.lastCell, this.player);
}
this.lastCell = this.player.Cell;
this.units.AddUnitAt(this.lastCell, this.player);
if (this.player.pathField == null)
{ 
this.player.pathField = new PathField(this.lastCell, 20, null, null);
}
else
{ 
this.player.pathField.Update(this.lastCell);
}
this.units.isDirty = false;
}
}
}
}
