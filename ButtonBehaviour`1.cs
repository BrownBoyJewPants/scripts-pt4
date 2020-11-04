using Game.Commands;
using System;
using System.Linq;
using UnityEngine;


public abstract class ButtonBehaviour<TEnum> : MonoBehaviour where TEnum : struct, IConvertible, IFormattable, IComparable
{ 
protected IGameBehaviour gameBehaviour;

public TEnum[] actions;

public int value;

private IGameBehaviour FindGame()
{ 
return UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().OfType<IGameBehaviour>().FirstOrDefault<IGameBehaviour>();
}

private void Reset()
{ 
this.gameBehaviour = this.FindGame();
}

private void OnEnable()
{ 
if (this.gameBehaviour == null)
{ 
this.gameBehaviour = this.FindGame();
}
}

private void OnClick()
{ 
for (int i = 0; i < this.actions.Length; i++)
{ 
this.gameBehaviour.Game.State.SendCommand(new ButtonCommand<TEnum>(this.actions[i], this.value, this));
}
i
}
}
