using Game.Commands;
using Game.Data.Items;
using Game.Data.Items.Weapons;
using Game.GameStates;
using Game.Zombiescape.Commands;
using Game.Zombiescape.Data;
using Game.Zombiescape.Monobehaviours;
using Game.Zombiescape.States.PlayerStates;
using System;
using TileEngine;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine.Models;


namespace Game.Zombiescape.States
{ 
public class PlayingState : GameState
{ 
public static bool painterEdition;

private ZombieUnitManager units;

private PooledVoxelModel zombiePrefab;

private PooledVoxelModel plasmaWeaponPrefab;

private Ammo plasmaAmmo;

private float spawnsPerMinute = 30f;

private float runSpeed = 2f;

private float lastSpawn = -1000f;

public PlayingState(ZombieUnitManager units)
{ 
this.units = units;
units.Player.State.Push(new PlayerControlState());
}

protected override void Initialize()
{ 
base.RegisterHandler<Game.Zombiescape.Commands.UnitKilledCommand>(new Action<Game.Zombiescape.Commands.UnitKilledCommand>(this.UnitKilled));
base.Initialize();
}

private void UnitKilled(Game.Zombiescape.Commands.UnitKilledCommand cmd)
{ 
cmd.handled = false;
if (cmd.unit is Enemy)
{ 
Enemy enemy = (Enemy)cmd.unit;
this.plasmaWeaponPrefab.Destroy(enemy.MainWeapon.Model);
enemy.Model.SetActive(false);
this.units.Destroy(enemy);
}
enemy
}

public override void Start()
{ 
this.plasmaAmmo = new PlasmaAmmo(this.game, Resources.Load<GameObject>("prefabs/plasmaammo"));
VoxelAsset asset = Resources.Load<VoxelAsset>("voxeldata/characters/enemy");
this.zombiePrefab = new PooledVoxelModel(ModelRenderer.CreateUnitModel(asset, this.game.Behaviour.TileMap.Material, 0.75f, "Animations/VoxelModel"));
this.plasmaWeaponPrefab = new PooledVoxelModel(ZombiescapeBehaviour.CreateWeaponModel(this.game.Behaviour.TileMap.Material, "VoxelData/Weapons/plasmapistol"));
asset
}

public override void Update()
{ 
if (!this.isactive)
{ 
return;
}
if (!this.units.Player.IsAlive)
{ 
this.Push(new DeadMenuState(this.game.Behaviour.Menu.FindMenu("DeadMenu")));
return;
}
base.Update();
this.spawnsPerMinute += Time.deltaTime * 2f;
if (!this.isactive)
{ 
return;
}
if (Time.timeSinceLevelLoad - this.lastSpawn > 60f / this.spawnsPerMinute)
{ 
this.SpawnEnemy();
}
}

private void SpawnEnemy()
{ 
if (this.units.Enemies.Count > 75)
{ 
this.runSpeed += Time.deltaTime * 0.05f;
}
if (this.units.Enemies.Count > 150)
{ 
return;
}
VoxelModel voxelModel = this.zombiePrefab.Instantiate();
Enemy enemy = new Enemy((ZombiescapeInfo)this.game, voxelModel);
enemy.Speed = this.runSpeed;
if (PlayingState.painterEdition)
{ 
enemy.BloodColor = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), 255);
}
MapCell mapCell = null;
while (mapCell == null)
{ 
mapCell = this.game.Map[UnityEngine.Random.Range(0, this.game.Map.Dx), 1, UnityEngine.Random.Range(0, this.game.Map.Dz)];
if (this.units.GetUnitsAt(mapCell) != null)
{ 
mapCell = null;
}
else if ((mapCell.moveable & TilePart.All) != TilePart.All)
{ 
mapCell = null;
}
else
{ 
float magnitude = (mapCell.worldposition - this.units.Player.Model.transform.position).magnitude;
if (magnitude < 12f || magnitude > 17f)
{ 
mapCell = null;
}
}
}
enemy.Model.transform.position = mapCell.worldposition + new Vector3(0.5f, 7f, 0.5f);
UnitBehaviour unitBehaviour = enemy.Model.collider.gameObject.GetComponent<UnitBehaviour>();
if (unitBehaviour == null)
{ 
unitBehaviour = enemy.Model.collider.gameObject.AddComponent<UnitBehaviour>();
GravityBehaviour gravityBehaviour = enemy.Model.model.AddComponent<GravityBehaviour>();
gravityBehaviour.map = this.game.Map;
}
unitBehaviour.unit = enemy;
enemy.MainWeapon = new PlasmaPistol(this.plasmaAmmo);
enemy.MainWeapon.Model = this.plasmaWeaponPrefab.Instantiate();
ZombiescapeBehaviour.AttachWeapon(voxelModel, enemy.MainWeapon.Model);
this.units.Enemies.Add(enemy);
this.lastSpawn = Time.timeSinceLevelLoad;
voxelModel
enemy
mapCell
magnitude
unitBehaviour
gravityBehaviour
}

public override void HandleCommand(Command command)
{ 
this.units.Player.State.SendCommand(command);
if (!command.handled)
{ 
base.HandleCommand(command);
}
}

public override string ToString()
{ 
return base.ToString() + "->" + this.units.Player.State;
}
}
}
