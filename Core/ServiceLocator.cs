using System;
using System.Collections.Generic;

namespace AsteroidsClone.Core;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
    
    public static void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }
    
    public static T Get<T>()
    {
        if (_services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        throw new InvalidOperationException($"Service of type {typeof(T)} not registered.");
    }
    
    public static bool TryGet<T>(out T service)
    {
        if (_services.TryGetValue(typeof(T), out var obj))
        {
            service = (T)obj;
            return true;
        }
        service = default(T);
        return false;
    }
    
    public static void Clear()
    {
        _services.Clear();
    }
}


