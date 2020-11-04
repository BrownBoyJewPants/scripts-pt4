using Game;
using Game.Commands;
using Game.Data;
using Game.Data.Items;
using Game.Data.Items.Weapons;
using Game.Global.MonoBehaviours;
using Game.Gui;
using Game.States;
using Game.UnitStates;
using System;
using System.Collections;
using TileEngine;
using TileEngine.Generators;
using TileEngine.MonoBehaviours;
using TileEngine.TileMap;
using UnityEngine;
using VoxelEngine;
using VoxelEngine.Renderers;


public class GameBehaviour : MonoBehaviour, IGameBehaviour
{ 
public int maxParticles;

public VoxelResourceManager resourceManager;

public TemplateResourceManager templateManager;

public GraphicsTileMapBehaviour tileMap;

public BattlescapeInfo game;

public Transform PlayerModel;

public Transform EnemyModel;

public GameObject OrbitalStrikeAmmo;

public GameObject PlasmaAmmo;

public GameObject MachineGunAmmo;

public GameObject RocketLauncherAmmo;

public GameObject Explosion;

public GameObject MuzzleFlash;

public GameObject LaserBomb;

public GameObject Shell;

public ParticleSystem ParticleSystem;

private CellHoverCommand hoverCommand = new CellHoverCommand(null);

private float mouseDownTime;

private Vector3 mouseDownPos;

IGameInfo IGameBehaviour.Game
{ 
get
{ 
return this.game;
}
}

ITileMapBehaviour IGameBehaviour.TileMap
{ 
get
{ 
return this.tileMap;
}
}

public MenuBehaviour Menu
{ 
get
{ 
return base.GetComponent<MenuBehaviour>();
}
}

private void Awake()
{ 
if (this.PlayerModel == null)
{ 
Debug.LogError("No player model attached to TileMap!");
return;
}
this.game = new BattlescapeInfo(this)
{ 
roundStartTime = DateTime.Now,
map = RandomOutdoorMap.Create(this.resourceManager, this.templateManager)
};
GameObject.Find("UfoCoreLights").transform.position = this.game.map.UfoCoreLocation.worldposition;
this.CreatePlayerUnits();
this.CreateFriendlyUnitObjects();
this.CreateEnemyUnits();
this.CreateEnemyUnitObjects();
this.tileMap.SetMap(this.game.map);
}

private void Start()
{ 
this.game.Start();
this.game.state.SendButton<GameButton>(GameButton.NextFriendlyUnit, 0);
this.game.state.SendButton<GameButton>(GameButton.CenterOnFriendlyUnit, 0);
}

private void Update()
{ 
this.ProcessMouseInput();
this.ProcessKeyboardInput();
OnScreenDebug.Update(this.game);
this.game.Update();
}

private void FixedUpdate()
{ 
this.game.FixedUpdate();
}

private void ProcessKeyboardInput()
{ 
if (Input.GetKeyDown(KeyCode.Alpha1))
{ 
this.game.state.SendButton<GameButton>(GameButton.FireMainWeapon, 0);
}
if (Input.GetKeyDown(KeyCode.Alpha2))
{ 
this.game.state.SendButton<GameButton>(GameButton.FireSecondaryWeapon, 0);
}
if (Input.GetKeyDown(KeyCode.Tab))
{ 
if (Input.GetKey(KeyCode.LeftShift))
{ 
this.game.state.SendButton<GameButton>(GameButton.PreviousFriendlyUnit, 0);
}
else
{ 
this.game.state.SendButton<GameButton>(GameButton.NextFriendlyUnit, 0);
}
}
}

private void ProcessMouseInput()
{ 
if (GameGui.HitGUI(Input.mousePosition))
{ 
return;
}
MapCell mapCell = this.hoverCommand.cell = this.tileMap.mouseOverCell;
this.game.state.SendCommand(this.hoverCommand);
if (Input.mousePosition != this.mouseDownPos)
{ 
this.mouseDownTime = 3.40282347E+38f;
}
if (Input.GetMouseButtonDown(0))
{ 
this.mouseDownTime = Time.realtimeSinceStartup;
this.mouseDownPos = Input.mousePosition;
if (mapCell != null)
{ 
Unit unitAt = this.game.units.GetUnitAt(mapCell);
if (unitAt != null)
{ 
if (unitAt.isFriendly)
{ 
this.game.state.SendCommand(new SelectFriendlyUnitCommand(unitAt));
}
else
{ 
this.game.state.SendCommand(new SelectEnemyUnitCommand(unitAt));
}
}
else
{ 
this.game.state.SendCommand(new CellClickedCommand(mapCell));
}
}
}
if (Input.GetMouseButtonDown(1) && mapCell != null)
{ 
this.game.state.SendCommand(new LookAtCellCommand(mapCell));
}
mapCell
unitAt
}

private void CreateEnemyUnits()
{ 
for (int i = 0; i < 15; i++)
{ 
MapCell mapCell;
do
{ 
int x = UnityEngine.Random.Range(0, this.game.map.Dx);
int z = UnityEngine.Random.Range(0, this.game.map.Dz);
mapCell = this.game.map[x, 1, z];
}
while (this.game.units.GetUnitAt(mapCell) != null || (mapCell.moveable & TilePart.Center) == TilePart.None || this.game.units.Friendlies.CanSee(mapCell));
Unit unit = new Unit("Enemy " + i, new UnitStats
{ 
health = 30,
sightrange = 20f,
stamina = 60f,
timeUnits = 54f,
accuracy = 0.52f,
dexterity = 63
})
{ 
armor = new UnitArmor
{ 
front = 4,
under = 2,
sides = 2,
back = 3
},
bloodColor = new Color32(0, 64, 0, 128)
};
unit.SetPosition(mapCell);
unit.inventory.mainWeapon.item = new PlasmaPistol(new PlasmaAmmo(this.game, this.PlasmaAmmo));
this.game.units.Enemies.Add(unit);
unit.State.Push(new RootAIState());
}
i
x
z
mapCell
unit
}

private void CreatePlayerUnits()
{ 
int num = 10;
int num2 = this.game.map.Dz / 2;
for (int i = 0; i < 4; i++)
{ 
for (int j = 0; j < 2; j++)
{ 
Unit unit = new Unit(RandomNames.GetMaleName(), new UnitStats
{ 
health = UnityEngine.Random.Range(25, 40),
sightrange = 20f,
stamina = 45f,
timeUnits = (float)UnityEngine.Random.Range(50, 60),
accuracy = UnityEngine.Random.Range(0.4f, 0.7f),
strength = UnityEngine.Random.Range(20, 40),
bravery = (float)UnityEngine.Random.Range(20, 40),
intelligence = UnityEngine.Random.Range(20, 40),
dexterity = UnityEngine.Random.Range(20, 40)
})
{ 
isFriendly = true
};
unit.inventory.mainWeapon.item = new Rifle(new RifleAmmo(this.game, this.MachineGunAmmo, new PooledPrefab(this.Shell)));
unit.inventory.secondaryWeapon.item = new RocketLauncher(new OrbitalStrike(this.game, this.OrbitalStrikeAmmo));
unit.SetPosition(this.game.map[num + i, 1, num2 + j]);
this.game.units.Friendlies.Add(unit);
unit.State.Push(UnitState.Root());
}
}
num
num2
i
j
unit
}

private void CreateFriendlyUnitObjects()
{ 
Transform transform = new GameObject("FriendlyUnits").transform;
transform.parent = base.transform;
VoxelAsset voxelAsset = Resources.Load<VoxelAsset>("VoxelData/Characters/Soldier");
GameObject gameObject = this.CreateModel(voxelAsset);
GameObject gameObject2 = new GameObject("Friendlies");
foreach (Unit current in this.game.units.Friendlies)
{ 
this.CreateUnitModel(current, gameObject, 0.75f / (float)voxelAsset.dx).transform.parent = gameObject2.transform;
current.LookAt(this.game.map[current.Cell.mapposition.x + 1, current.Cell.mapposition.y, current.Cell.mapposition.z], true);
}
UnityEngine.Object.DestroyImmediate(gameObject);
transform
voxelAsset
gameObject
gameObject2
enumerator
current
}

private void CreateEnemyUnitObjects()
{ 
Transform transform = new GameObject("FriendlyUnits").transform;
transform.parent = base.transform;
VoxelAsset voxelAsset = Resources.Load<VoxelAsset>("VoxelData/Characters/Enemy");
GameObject gameObject = this.CreateModel(voxelAsset);
GameObject gameObject2 = new GameObject("Enemies");
foreach (Unit current in this.game.units.Enemies)
{ 
this.CreateUnitModel(current, gameObject, 0.75f / (float)voxelAsset.dx).transform.parent = gameObject2.transform;
current.LookAt(this.game.map[current.Cell.mapposition.x + UnityEngine.Random.Range(-1, 2), 1, current.Cell.mapposition.z + UnityEngine.Random.Range(-1, 2)], true);
}
UnityEngine.Object.DestroyImmediate(gameObject);
transform
voxelAsset
gameObject
gameObject2
enumerator
current
}

private GameObject CreateModel(VoxelAsset asset)
{ 
SimpleRenderBuffer simpleRenderBuffer = new SimpleRenderBuffer(false, true);
GreedySurfaceExtractor greedySurfaceExtractor = new GreedySurfaceExtractor(asset.dx, asset.dy, asset.dz);
GameObject gameObject = new GameObject(asset.name);
gameObject.transform.position = new Vector3(0f, 0f, 0f);
Vector3 a = new Vector3((float)asset.dx, 0f, (float)asset.dz) * 0.5f;
Animator animator = gameObject.AddComponent<Animator>();
animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/VoxelModel");
VoxelAssetPart[] parts = asset.parts;
for (int i = 0; i < parts.Length; i++)
{ 
VoxelAssetPart voxelAssetPart = parts[i];
Vector3 b = new Vector3((float)(voxelAssetPart.bounds.x2 + voxelAssetPart.bounds.x1), (float)(voxelAssetPart.bounds.y1 + voxelAssetPart.bounds.y2), (float)(voxelAssetPart.bounds.z1 + voxelAssetPart.bounds.z2)) * 0.5f;
VoxelData voxelData = asset.ToVoxelData(voxelAssetPart, true);
greedySurfaceExtractor.ExtractSurface(simpleRenderBuffer, voxelData, true, new Vector3?(voxelAssetPart.centerOffset));
Mesh mesh = new Mesh();
simpleRenderBuffer.UpdateMesh(mesh);
GameObject gameObject2 = new GameObject(voxelAssetPart.name);
gameObject2.transform.parent = gameObject.transform;
gameObject2.transform.localPosition = -a + b + voxelAssetPart.centerOffset;
MeshFilter meshFilter = gameObject2.AddComponent<MeshFilter>();
meshFilter.mesh = mesh;
MeshRenderer meshRenderer = gameObject2.AddComponent<MeshRenderer>();
meshRenderer.material = this.tileMap.material;
}
BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
boxCollider.size = new Vector3((float)(asset.bounds.x2 - asset.bounds.x1), (float)(asset.bounds.y2 - asset.bounds.y1), (float)(asset.bounds.z2 - asset.bounds.z1));
boxCollider.center = new Vector3(0f, (float)(asset.bounds.y1 + asset.bounds.y2) * 0.5f, 0f);
return gameObject;
simpleRenderBuffer
greedySurfaceExtractor
gameObject
a
animator
parts
i
voxelAssetPart
b
voxelData
mesh
gameObject2
meshFilter
meshRenderer
boxCollider
}

private GameObject CreateUnitModel(Unit unit, GameObject model, float scale)
{ 
Vector3 vector = new Vector3(0.5f, 0f, 0.5f);
GameObject gameObject = new GameObject(model.name);
GameObject gameObject2 = new GameObject("Root");
gameObject2.transform.parent = gameObject.transform;
gameObject2.transform.localRotation = Quaternion.LookRotation(Vector3.back);
GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(model);
gameObject3.transform.parent = gameObject2.transform;
gameObject.transform.localScale = Vector3.one * scale;
throw new NotImplementedException();
vector
gameObject
gameObject2
gameObject3
}

virtual Coroutine StartCoroutine(IEnumerator routine)
{ 
return base.StartCoroutine(routine);
}
}
