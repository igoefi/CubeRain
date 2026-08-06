using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private FallenCube _cubePrefab;
    [SerializeField] private Transform _firstSpawnPointBorder;
    [SerializeField] private Transform _secondSpawnPointBorder;

    private ObjectPool<FallenCube> _pool;

    private void Start()
    {
        _pool = new(
            CreateCube,
            OnGetFromPool,
            OnRealesedToPool,
            (FallenCube cube) => Destroy(cube.gameObject),
            false, 2, 2);

        _pool.Get();
    }

    private FallenCube CreateCube()
    {
        var cube = Instantiate(_cubePrefab);
        cube.SetPool(_pool);
        return cube;
    }

    private void OnGetFromPool(FallenCube cube)
    {
        var position = new Vector3(
            Random.Range(_firstSpawnPointBorder.position.x, _secondSpawnPointBorder.position.x),
            _firstSpawnPointBorder.position.y,
            _firstSpawnPointBorder.position.z);
        cube.transform.position = position;
        cube.gameObject.SetActive(true);
    }

    private void OnRealesedToPool(FallenCube cube)
    {
        cube.gameObject.SetActive(false);
        _pool.Get();
    }
}
