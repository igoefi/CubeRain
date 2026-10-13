using System;
using TMPro;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    private void Awake()
    {
        CreatePool();
    }

    public void CreateBomb(Transform target)
    {
        Bomb bomb = Pool.Get();
        bomb.transform.position = target.position;
    }
    
    protected override Bomb Create()
    {
        CountCreateObjects++;
        Bomb bomb = Instantiate(Prefab);
        bomb.gameObject.SetActive(false);
        return bomb;
    }

    protected override void OnGetFromPool(Bomb obj)
    {
        CountSpawnObjects++;
        obj.gameObject.SetActive(true);
        obj.BombExploded += ReleaseObject;
        obj.StartFading();
    }

    protected override void OnRealesedToPool(Bomb obj)
    {
        obj.gameObject.SetActive(false);
        obj.BombExploded -= ReleaseObject;
    }

    protected override void OnDestroyObject(Bomb obj)
    {
        Destroy(obj.gameObject);
    }
}
