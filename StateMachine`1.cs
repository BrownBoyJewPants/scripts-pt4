using Game.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Game.States
{ 
public class StateMachine<T> : IStateMachine where T : State
{ 
private List<T> states;

private bool statesChanged;

State IStateMachine.Current
{ 
get
{ 
return this.Current;
}
}

public bool IsModal
{ 
get
{ 
return this.Current != null && this.Current.ismodal;
}
}

public T Current
{ 
get
{ 
if (this.states.Count == 0)
{ 
return (T)((object)null);
}
return this.states[this.states.Count - 1];
}
}

public StateMachine()
{ 
this.states = new List<T>();
}

void IStateMachine.PopTo(State state)
{ 
while (this.Current != null && this.Current != state && this.states.Contains((T)((object)state)))
{ 
this.Pop();
}
}

void IStateMachine.Push(State state)
{ 
this.Push((T)((object)state));
}

State IStateMachine.Pop()
{ 
return this.Pop();
}

public virtual void SendButton<TEnum>(TEnum button, int value = 0) where TEnum : struct, IConvertible, IFormattable, IComparable
{ 
this.SendCommand(new ButtonCommand<TEnum>(button, value, null));
}

public virtual void SendCommand(Command command)
{ 
command.handled = false;
int num = this.states.Count - 1;
while (!command.handled && num >= 0)
{ 
T t = this.states[num];
t.HandleCommand(command);
if (t.ismodal)
{ 
break;
}
num--;
}
num
t
}

public virtual void Push(T state)
{ 
this.statesChanged = true;
T current = this.Current;
if (current != null)
{ 
current.EnterBackground();
current.isactive = false;
}
this.states.Add(state);
state.Init(this);
state.Start();
state.EnterForeground();
state.isactive = true;
current
}

public virtual T Pop()
{ 
T current = this.Current;
if (current != null)
{ 
this.states.RemoveAt(this.states.Count - 1);
current.isactive = false;
current.EnterBackground();
current.End();
}
this.PopInactive();
if (this.Current != null && !this.Current.stopped)
{ 
this.Current.isactive = true;
T current2 = this.Current;
current2.EnterForeground();
}
this.statesChanged = true;
return this.Current;
current
current2
}

private void PopInactive()
{ 
while (this.states.Count > 0 && this.states[this.states.Count - 1].stopped)
{ 
T t = this.states[this.states.Count - 1];
this.states.RemoveAt(this.states.Count - 1);
t.isactive = false;
t.EnterBackground();
t.End();
}
t
}

public virtual void Update()
{ 
this.UpdatePriv(0);
}

private void UpdatePriv(int loopCount = 0)
{ 
this.PopInactive();
this.statesChanged = false;
if (loopCount > 5)
{ 
Debug.Log(string.Concat(new object[]
{ 
"Getting stuck in loop ",
loopCount,
": ",
this
}));
if (loopCount > 15)
{ 
throw new Exception("Really stuck now");
}
}
for (int i = this.states.Count - 1; i >= 0; i--)
{ 
if (!this.states[i].stopped)
{ 
T t = this.states[i];
t.Update();
}
if (this.statesChanged)
{ 
this.UpdatePriv(++loopCount);
return;
}
}
i
t
}

public virtual void AIUpdate()
{ 
this.PopInactive();
this.statesChanged = false;
if (this.states.Count > 0)
{ 
T t = this.states[this.states.Count - 1];
t.AIUpdate();
if (this.statesChanged)
{ 
this.AIUpdate();
}
}
t
}

public virtual void FixedUpdate()
{ 
this.PopInactive();
this.statesChanged = false;
for (int i = this.states.Count - 1; i >= 0; i--)
{ 
if (!this.states[i].stopped)
{ 
T t = this.states[i];
t.FixedUpdate();
}
if (this.statesChanged)
{ 
this.FixedUpdate();
return;
}
}
i
t
}

public override string ToString()
{ 
if (this.Current == null)
{ 
return "<No state>";
}
T current = this.Current;
return current.ToString();
current
}
}
}
