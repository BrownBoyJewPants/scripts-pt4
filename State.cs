using Game.Commands;
using System;
using System.Collections.Generic;


namespace Game.States
{ 
public abstract class State
{ 
public bool stopped;

public bool ismodal;

public bool isactive;

public IStateMachine subState;

private IStateMachine statemachine;

private Dictionary<Type, Action<Command>> handlers;

private Dictionary<string, Action<IButtonCommand>> buttonHandlers;

public State()
{ 
this.ismodal = false;
this.handlers = new Dictionary<Type, Action<Command>>();
this.buttonHandlers = new Dictionary<string, Action<IButtonCommand>>();
this.subState = null;
}

internal void Init(IStateMachine statemachine)
{ 
this.statemachine = statemachine;
this.Initialize();
}

protected virtual void Initialize()
{ 
}

protected virtual void Set(State state)
{ 
this.statemachine.PopTo(this);
this.statemachine.Pop();
this.statemachine.Push(state);
}

protected virtual void Push(State state)
{ 
this.statemachine.PopTo(this);
this.statemachine.Push(state);
}

protected virtual void Pop()
{ 
if (this.statemachine.Current == this)
{ 
this.statemachine.Pop();
}
else
{ 
this.stopped = true;
}
}

protected void MakeActiveState()
{ 
this.statemachine.PopTo(this);
}

protected void RegisterHandler<TCommand>(Action<TCommand> handler) where TCommand : Command
{ 
this.handlers[typeof(TCommand)] = delegate(Command c)
{ 
handler((TCommand)((object)c));
};
<RegisterHandler>c__AnonStorey
}

protected void RegisterHandler<TCommand>(Action handler) where TCommand : Command
{ 
this.handlers[typeof(TCommand)] = delegate(Command c)
{ 
handler();
};
<RegisterHandler>c__AnonStorey
}

protected void RegisterHandler<TEnum>(TEnum button, Action<ButtonCommand<TEnum>> handler) where TEnum : struct, IConvertible, IFormattable, IComparable
{ 
this.buttonHandlers[this.GetButtonKey(button)] = delegate(IButtonCommand cmd)
{ 
handler((ButtonCommand<TEnum>)cmd);
};
<RegisterHandler>c__AnonStorey
}

protected void RegisterHandler<TEnum>(TEnum button, Action handler) where TEnum : struct, IConvertible, IFormattable, IComparable
{ 
this.buttonHandlers[this.GetButtonKey(button)] = delegate(IButtonCommand b)
{ 
handler();
};
<RegisterHandler>c__AnonStorey
}

private string GetButtonKey(object button)
{ 
return button.GetType().Name + "." + button;
}

public virtual void Start()
{ 
}

public virtual void EnterForeground()
{ 
}

public virtual void EnterBackground()
{ 
}

public virtual void End()
{ 
}

public virtual void Update()
{ 
}

public virtual void AIUpdate()
{ 
}

public virtual void FixedUpdate()
{ 
}

public virtual void HandleCommand(Command command)
{ 
if (this.subState != null)
{ 
this.subState.SendCommand(command);
if (command.handled)
{ 
return;
}
}
Action<Command> action;
if (this.handlers.TryGetValue(command.GetType(), out action))
{ 
command.handled = true;
action(command);
return;
}
IButtonCommand buttonCommand = command as IButtonCommand;
Action<IButtonCommand> action2;
if (buttonCommand != null && this.buttonHandlers.TryGetValue(this.GetButtonKey(buttonCommand.button), out action2))
{ 
command.handled = true;
action2(buttonCommand);
}
action
buttonCommand
action2
}

public override string ToString()
{ 
return base.GetType().Name;
}
}
}
