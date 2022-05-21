using UnityEngine;

public interface IPooledObject
{
    GameObject GameObject { get; }

    void OnSpawn();
    void OnDespawn();
}
