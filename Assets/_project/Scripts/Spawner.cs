using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour
where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _maxPoolSize;
    protected ObjectPool<T> _pool;

    protected void CreatePool()
    {
        _pool = new(
            Create,
            OnGetFromPool,
            OnRealesedToPool,
            OnDestroyObject,
            false, 2, _maxPoolSize);
    }
    
    protected abstract T Create();
    protected abstract void OnGetFromPool(T obj);
    protected abstract void OnRealesedToPool(T obj);
    protected abstract void OnDestroyObject(T obj);
}
