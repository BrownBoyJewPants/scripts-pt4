using Assets.Scripts.Settings;
using Game.Data;
using Game.Data.Items;
using Game.Data.Items.Weapons;
using Game.Global;
using Game.Global.MonoBehaviours;
using Game.Gui;
using Game.Services;
using Game.Zombiescape.Monobehaviours;
using System;
using System.Collections;
using TileEngine.Particles;
using UnityEngine;
using VoxelEngine.Models;


namespace Game.Zombiescape
{ 
public class ZombiescapeBehaviour : MonoBehaviour, IGameBehaviour
{ 
public ZombiescapeInfo game;

public VoxelResourceManager voxelResourceManager;

public TemplateResourceManager templateResourceManager;

public MonoBehaviour tilemapBehaviour;

private ITileMapBehaviour tilemap;

private GameObject[] menuRoots;

public GameObject rifleAmmo;

public GameObject rocketAmmo;

public GameObject ac130mainAmmo;

public GameObject orbitalAmmo;

public GameObject homingAmmo;

public GameObject shell;

ITileMapBehaviour IGameBehaviour.TileMap
{ 
get
{ 
return this.tilemap;
}
}

IGameInfo IGameBehaviour.Game
{ 
get
{ 
return this.game;
}
}

public MenuBehaviour Menu
{ 
get
{ 
return base.GetComponent<MenuBehaviour>();
}
}

private void Update()
{ 
this.game.Update();
}

private void FixedUpdate()
{ 
this.game.FixedUpdate();
}

private void Awake()
{ 
this.tilemap = (this.tilemapBehaviour as ITileMapBehaviour);
if (this.tilemap == null)
{ 
Debug.LogError("Invalid tilemap");
}
int threadCount = TEParticleSystem.threadCount;
ServiceLocator serviceLocator = new ServiceLocator();
serviceLocator.Register<TEParticleSystem>(new TEParticleSystem(TEParticleSystemRenderMethod.Cube, threadCount * 1250));
serviceLocator.Register<CameraEffects>(new CameraEffects(this));
serviceLocator.Register<AudioGui>(new AudioGui());
serviceLocator.Register<Assets.Scripts.Settings.AudioSettings>(new Assets.Scripts.Settings.AudioSettings());
serviceLocator.Register<VideoSettings>(new VideoSettings(serviceLocator));
serviceLocator.Register<SpecialEffects>(new SpecialEffects(this));
this.game = new ZombiescapeInfo(serviceLocator)
{ 
map = RandomOutDoorWinterMap.Create(this.voxelResourceManager, this.templateResourceManager),
behaviour = this
};
VoxelModel model = this.CreatePlayerModel();
this.game.player = new Player(this.game, model);
this.game.player.Model.model.AddComponent<PlayerControlBehaviour>();
GravityBehaviour gravityBehaviour = this.game.player.Model.model.AddComponent<GravityBehaviour>();
gravityBehaviour.map = this.game.map;
this.tilemap.SetMap(this.game.map);
CameraFollowModel component = Camera.main.GetComponent<CameraFollowModel>();
if (component != null)
{ 
component.model = this.game.player.Model.model;
}
this.game.player.primaryWeapon = new Rifle(new RifleAmmo(this.game, this.rifleAmmo, new PooledPrefab(this.shell)));
this.game.player.secondaryWeapon = new RocketLauncher(new RocketLauncherAmmo(this.game, this.rocketAmmo));
this.game.player.Model.collider.gameObject.AddComponent<UnitBehaviour>().unit = this.game.player;
this.game.player.primaryWeapon.Model = ZombiescapeBehaviour.CreateWeaponModel(this.tilemap.Material, "VoxelData/Weapons/Rifle");
ZombiescapeBehaviour.AttachWeapon(this.game.player.Model, this.game.player.primaryWeapon.Model);
serviceLocator.Get<TEParticleSystem>().Start(this.game.map);
this.game.Initialize();
threadCount
serviceLocator
model
gravityBehaviour
component
}

internal static void AttachWeapon(VoxelModel unit, VoxelModel weapon)
{ 
Transform tag = unit.GetTag("righthand");
if (tag == null)
{ 
Debug.Log("Unit has no right hand!");
}
weapon.model.transform.parent = tag;
weapon.model.transform.localScale = Vector3.one * 0.85f;
weapon.model.transform.localRotation = Quaternion.Euler(0f, 180f, 88.5f);
weapon.model.transform.localPosition = Vector3.zero;
if (unit.animator != null)
{ 
unit.model.SetActive(false);
unit.model.SetActive(true);
}
tag
}

internal static VoxelModel CreateWeaponModel(Material material, string modelname)
{ 
VoxelAsset asset = Resources.Load<VoxelAsset>(modelname);
VoxelModel voxelModel = ModelRenderer.CreateModel(asset, material, null);
GameObject model = voxelModel.model;
model.name = "Weapon";
return voxelModel;
asset
voxelModel
model
}

private VoxelModel CreatePlayerModel()
{ 
VoxelAsset asset = Resources.Load<VoxelAsset>("VoxelData/Characters/Soldier");
VoxelModel voxelModel = ModelRenderer.CreateUnitModel(asset, this.tilemap.Material, 0.75f, "Animations/VoxelModel");
voxelModel.model.transform.position = new Vector3((float)(this.game.map.Dx / 2), 3f, (float)(this.game.map.Dz / 2));
voxelModel.rigidBody.isKinematic = true;
VoxelModel result = voxelModel.Clone();
UnityEngine.Object.Destroy(voxelModel.model);
return result;
asset
voxelModel
result
}

virtual Coroutine StartCoroutine(IEnumerator routine)
{ 
return base.StartCoroutine(routine);
}
}
}
