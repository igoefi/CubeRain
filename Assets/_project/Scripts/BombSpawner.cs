using System;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    private void Start()
    {
        CreatePool();
    }

    public void CreateBomb(Transform target)
    {
        Bomb bomb = _pool.Get();
        bomb.transform.position = target.position;
    }
    
    protected override Bomb Create()
    {
        Bomb bomb = Instantiate(_prefab);
        bomb.SetPool(_pool);
        bomb.gameObject.SetActive(false);
        return bomb;
    }

    protected override void OnGetFromPool(Bomb obj)
    {
        obj.gameObject.SetActive(true);
        obj.StartFading();
    }

    protected override void OnRealesedToPool(Bomb obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected override void OnDestroyObject(Bomb obj)
    {
        Destroy(obj.gameObject);
    }
}
