using Game.Gui;
using System;
using TileEngine.TileMap;
using UnityEngine;


namespace Game.Data.Items
{ 
public class PlasmaAmmo : Ammo
{ 
public PlasmaAmmo(IGameInfo game, GameObject model) : base(game, model)
{ 
this.speed = 15f;
this.maxDistance = 50f;
this.maxTimeAlive = 2f;
this.damage = 10f;
}

public override void Start()
{ 
this.game.Services.Get<AudioGui>().PlaySound("plasma", this.model.transform.position, 1f);
base.Start();
}

public override void HitWorld(ref Vector3 normal)
{ 
this.game.Services.Get<AudioGui>().PlaySound("plasmahitwall", this.model.transform.position, 1f);
this.alive = false;
SphereShape shape = new SphereShape(this.model.transform.position, 0.2f);
this.game.Map.AddModification(shape);
shape
}
}
}
