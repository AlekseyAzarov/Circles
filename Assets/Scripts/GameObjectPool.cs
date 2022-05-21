using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] private int _objectsOnStart;

    private IPooledObject _pooledObject;
    private Queue<IPooledObject> _pool = new Queue<IPooledObject>();

    public void Init(IPooledObject pooledObject)
    {
        _pooledObject = pooledObject;
        _pool = new Queue<IPooledObject>();
        AddToPool(_objectsOnStart);
    }

    public GameObject Spawn()
    {
        if (_pool.Count == 0) AddToPool(1);

        var go = _pool.Dequeue();
        go.OnSpawn();
        go.GameObject?.SetActive(true);
        return go.GameObject;
    }

    public void Despawn(IPooledObject pooledObject)
    {
        pooledObject.OnDespawn();
        pooledObject.GameObject.SetActive(false);
        _pool.Enqueue(pooledObject);
    }

    private void AddToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var obj = Instantiate(_pooledObject.GameObject);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            _pool.Enqueue(obj.GetComponent<IPooledObject>());
        }
    }
}
