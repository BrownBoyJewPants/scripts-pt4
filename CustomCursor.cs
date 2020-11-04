using Game.Data;
using Game.Gui;
using System;
using UnityEngine;


public class CustomCursor : MonoBehaviour
{ 
public Texture2D PointerCursor;

public Texture2D CrosshairCursor;

public MonoBehaviour GameBehaviour;

private Texture2D cursorImage;

private IGameInfo game;

private void Start()
{ 
if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
{ 
this.cursorImage = this.PointerCursor;
Cursor.SetCursor(this.cursorImage, Vector2.zero, CursorMode.Auto);
}
IGameBehaviour gameBehaviour = this.GameBehaviour as IGameBehaviour;
if (gameBehaviour != null)
{ 
this.game = gameBehaviour.Game;
}
gameBehaviour
}

private void OnGUI()
{ 
if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
{ 
Texture2D x;
if ((this.game != null && this.game.State.IsModal) || GameGui.HitGUI(Input.mousePosition))
{ 
x = this.PointerCursor;
}
else
{ 
x = this.CrosshairCursor;
}
Vector2 vector = Vector2.zero;
if (x == this.CrosshairCursor)
{ 
vector += new Vector2((float)this.CrosshairCursor.width * 0.5f, (float)this.CrosshairCursor.height * 0.5f);
}
if (x != this.cursorImage)
{ 
Cursor.SetCursor(this.cursorImage = x, vector, CursorMode.Auto);
}
}
x
vector
}
}
