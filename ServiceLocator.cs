using System;
using System.Collections.Generic;


namespace Game.Services
{ 
public class ServiceLocator
{ 
private Dictionary<Type, object> services = new Dictionary<Type, object>();

public T Get<T>() where T : class
{ 
object obj;
if (!this.services.TryGetValue(typeof(T), out obj))
{ 
return (T)((object)null);
}
return (T)((object)obj);
obj
}

public void Register<T>(T service)
{ 
this.services[typeof(T)] = service;
}

internal void Dispose()
{ 
this.services.Clear();
this.services = new Dictionary<Type, object>();
}
}
}
