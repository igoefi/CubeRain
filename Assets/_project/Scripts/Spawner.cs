using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour, ISpawner
where T : MonoBehaviour
{
    public int CountSpawnObjects { get; set; }
    public int CountCreateObjects { get; set; }
    public int CountActiveObjects  { get; }

    [SerializeField] protected T Prefab;
    [SerializeField] protected int MaxPoolSize;
    
    protected ObjectPool<T> Pool;
    
    public event Action CountChanged;

    protected void CreatePool()
    {
        Pool = new(
            Create,
            OnGetFromPool,
            OnRealesedToPool,
            OnDestroyObject,
            false, 2, MaxPoolSize);
    }
    
    protected virtual void ReleaseObject(T obj)
    {
        Pool.Release(obj);
        CountChanged?.Invoke();
    }
    
    protected abstract T Create();
    protected abstract void OnGetFromPool(T obj);
    protected abstract void OnRealesedToPool(T obj);
    protected abstract void OnDestroyObject(T obj);
    
    int ISpawner.CountSpawnObjects
    {
        get => CountSpawnObjects;
        set
        {
            CountSpawnObjects = value; 
            CountChanged?.Invoke();
        }
    }
    int ISpawner.CountCreateObjects
    {
        get => CountCreateObjects;
        set
        {
            CountCreateObjects = value; 
            CountChanged?.Invoke();
        }
    }
    int ISpawner.CountActiveObjects
    {
        get => Pool.CountActive;
    }
}
