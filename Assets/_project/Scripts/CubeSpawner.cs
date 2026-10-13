using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<FallenCube>
{
    [SerializeField] private Transform _firstSpawnPointBorder;
    [SerializeField] private Transform _secondSpawnPointBorder;
    [SerializeField] private float _spawnPerTime;

    private WaitForSeconds _waitSpawn;
    
    public event Action<Transform> CubeReleased;
    
    private void Awake()
    {
        CreatePool();
        _waitSpawn = new WaitForSeconds(_spawnPerTime);
        StartCoroutine(Spawn());
    }

    protected override FallenCube Create()
    { 
        CountCreateObjects++;
        FallenCube cube = Instantiate(Prefab);
        return cube;
    }

    protected override void OnGetFromPool(FallenCube obj)
    {
        CountSpawnObjects++;
        var position = new Vector3(
            Random.Range(_firstSpawnPointBorder.position.x, _secondSpawnPointBorder.position.x),
            _firstSpawnPointBorder.position.y,
            _firstSpawnPointBorder.position.z);
        obj.transform.position = position;
        obj.CubeDeaded += ReleaseObject;
        obj.gameObject.SetActive(true);
    }

    protected override void OnRealesedToPool(FallenCube obj)
    {
        CubeReleased?.Invoke(obj.transform);
        obj.CubeDeaded -= ReleaseObject;
        obj.gameObject.SetActive(false);
    }

    protected override void OnDestroyObject(FallenCube obj)
    {
        Destroy(obj.gameObject);
    }

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            Pool.Get();
            yield return _waitSpawn;
        }
    }
}
