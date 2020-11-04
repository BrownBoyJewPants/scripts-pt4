using System;
using UnityEngine;


namespace Game.Commands
{ 
public class ButtonCommand<TEnum> : Command, IButtonCommand where TEnum : struct, IConvertible, IFormattable, IComparable
{ 
public readonly int value;

public readonly TEnum button;

public readonly MonoBehaviour source;

int IButtonCommand.value
{ 
get
{ 
return this.value;
}
}

object IButtonCommand.button
{ 
get
{ 
return this.button;
}
}

public ButtonCommand(TEnum button, int value = 0, MonoBehaviour source = null)
{ 
this.button = button;
this.value = value;
this.source = source;
}
}
}
