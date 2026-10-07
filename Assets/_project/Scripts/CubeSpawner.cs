using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CubeSpawner : Spawner<FallenCube>
{
    public Action<Transform> OnReleasedCube;
    
    [SerializeField] private Transform _firstSpawnPointBorder;
    [SerializeField] private Transform _secondSpawnPointBorder;
    [SerializeField] private float _spawnPerTime;

    private WaitForSeconds _waitSpawn;
    
    private void Start()
    {
        CreatePool();
        _waitSpawn = new WaitForSeconds(_spawnPerTime);
        StartCoroutine(Spawn());
    }

    protected override FallenCube Create()
    { 
        FallenCube cube = Instantiate(_prefab);
        cube.SetPool(_pool);
        return cube;
    }

    protected override void OnGetFromPool(FallenCube cube)
    {
        var position = new Vector3(
            Random.Range(_firstSpawnPointBorder.position.x, _secondSpawnPointBorder.position.x),
            _firstSpawnPointBorder.position.y,
            _firstSpawnPointBorder.position.z);
        cube.transform.position = position;
        cube.gameObject.SetActive(true);
    }

    protected override void OnRealesedToPool(FallenCube cube)
    {
        OnReleasedCube?.Invoke(cube.transform);
        cube.gameObject.SetActive(false);
    }

    protected override void OnDestroyObject(FallenCube cube)
    {
        Destroy(cube.gameObject);
    }

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            _pool.Get();
            yield return _waitSpawn;
        }
    }
}
