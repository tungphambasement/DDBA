using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private readonly Queue<T> _currentStock;
    private readonly Func<T> _factoryMethod;
    private readonly bool _isDynamic;
    private readonly Action<T> _turnOnCallback;
    private readonly Action<T> _turnOffCallback;

    public ObjectPool(Func<T> factoryMethod, Action<T> turnOnCallback , Action<T> turnOffCallback, int initialStock = 1, bool isDynamic = true)
    {
        _factoryMethod = factoryMethod;
        _isDynamic = isDynamic;

        _turnOffCallback = turnOffCallback;
        _turnOnCallback = turnOnCallback;

        _currentStock = new Queue<T>();

        for (var i = 0; i < initialStock; i++)
        {
            var o = _factoryMethod();
            _turnOffCallback(o);
            _currentStock.Enqueue(o);
        }
    }

    public ObjectPool(Func<T> factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, Queue<T> initialStock, bool isDynamic = true)
    {
        _factoryMethod = factoryMethod;
        _isDynamic = isDynamic;

        _turnOffCallback = turnOffCallback;
        _turnOnCallback = turnOnCallback;

        _currentStock = initialStock;
    }

    public T GetObject()
    {
        var result = default(T);
        if (_currentStock.Count > 0)
        {
            result = _currentStock.Dequeue();
        }
        else if (_isDynamic)
            result = _factoryMethod();
        _turnOnCallback(result);
        return result;
    }

    public void ReturnObject(T o)
    {
        _turnOffCallback(o);
        _currentStock.Enqueue(o);
    }
}