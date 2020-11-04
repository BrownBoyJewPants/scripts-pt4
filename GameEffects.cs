using System;
using System.Collections;
using System.Diagnostics;
using TileEngine;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Gui
{ 
public class GameEffects
{ 
private VoxelResourceManager resourceManager;

private VoxelResource selectionBox;

private VoxelResource selectionArrow;

private Material selectionBoxMaterial;

private MonoBehaviour gameBehaviour;

private PooledPrefab explosions;

private PooledPrefab muzzleFlash;

public GameEffects(GameBehaviour gameBehaviour, VoxelResourceManager resourceManager, Material selectionBoxMaterial)
{ 
this.gameBehaviour = gameBehaviour;
this.resourceManager = resourceManager;
this.selectionBoxMaterial = selectionBoxMaterial;
this.selectionBox = resourceManager.GetResource("selectionbox", TileFacing.None);
this.selectionArrow = resourceManager.GetResource("selectionarrow", TileFacing.None);
this.muzzleFlash = new PooledPrefab(Resources.Load<GameObject>("prefabs/muzzleflash"));
if (gameBehaviour.Explosion != null)
{ 
this.explosions = new PooledPrefab(gameBehaviour.Explosion);
}
}

public void DrawSelection(GameObject model)
{ 
if (model == null)
{ 
return;
}
Quaternion rotation = Quaternion.EulerRotation(0f, Time.time, 0f);
Graphics.DrawMesh(this.selectionArrow.mesh, model.transform.position + new Vector3(0f, 1.5f + Mathf.Sin(Time.time * 3f) * 0.1f, 0f), rotation, this.selectionBoxMaterial, 0, Camera.main);
rotation
}

public void DrawSelectedCell(MapCell cell)
{ 
if (cell == null)
{ 
return;
}
Graphics.DrawMesh(this.selectionBox.mesh, cell.worldposition + new Vector3(0.5f, 0.75f, 0.5f), Quaternion.identity, this.selectionBoxMaterial, 0, Camera.main);
}

public void DrawMuzzleFlash(Vector3 location, float time)
{ 
GameObject gameObject = this.muzzleFlash.Instantiate();
gameObject.transform.position = location;
this.gameBehaviour.StartCoroutine(this.DestroyObject(this.muzzleFlash, gameObject, time));
gameObject
}

public void DrawExplosion(Vector3 location, float size)
{ 
if (this.explosions == null)
{ 
return;
}
GameObject gameObject = this.explosions.Instantiate();
gameObject.transform.position = location;
ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
float num = size / 60f;
component.startSpeed *= num;
component.startSize *= num;
this.gameBehaviour.StartCoroutine(this.DestroyObject(this.explosions, gameObject, 3f));
gameObject
component
num
}

[DebuggerHidden]
private IEnumerator DestroyObject(PooledPrefab pool, GameObject obj, float time)
{ 
GameEffects.<DestroyObject>c__Iterator8 <DestroyObject>c__Iterator = new GameEffects.<DestroyObject>c__Iterator8();
<DestroyObject>c__Iterator.time = time;
<DestroyObject>c__Iterator.pool = pool;
<DestroyObject>c__Iterator.obj = obj;
<DestroyObject>c__Iterator.<$>time = time;
<DestroyObject>c__Iterator.<$>pool = pool;
<DestroyObject>c__Iterator.<$>obj = obj;
return <DestroyObject>c__Iterator;
<DestroyObject>c__Iterator
}
}
}
