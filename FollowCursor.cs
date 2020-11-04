using Game.Data;
using System;
using UnityEngine;


public class FollowCursor : MonoBehaviour
{ 
public MonoBehaviour gameBehaviour;

private IGameInfo game;

private void Start()
{ 
IGameBehaviour gameBehaviour = this.gameBehaviour as IGameBehaviour;
if (gameBehaviour != null)
{ 
this.game = gameBehaviour.Game;
}
gameBehaviour
}

private void Update()
{ 
if (this.game == null)
{ 
return;
}
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
float num;
Vector3 vector;
if (this.game.Map.IntersectRay(ref ray, 100f, out num, out vector, true, false))
{ 
base.transform.position = ray.GetPoint(num - 0.5f);
}
ray
num
vector
}
}
