using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class FallenCube : MonoBehaviour
{
    [SerializeField] private float _minDeathTime = 2;
    [SerializeField] private float _maxDeathTime = 5;

    private ObjectPool<FallenCube> _pool;
    private MeshRenderer _mesh;
    private bool _isFirstCollision = true;

    private void Start() =>
        _mesh = GetComponent<MeshRenderer>();

    public void SetPool(ObjectPool<FallenCube> pool) =>
        _pool = pool;

    private void OnCollisionEnter(Collision collision)
    {
        if (_isFirstCollision)
        {
            _isFirstCollision = false;
            StartCoroutine(DeathDelay());
        }
    }

    private IEnumerator DeathDelay()
    {
        _mesh.material.color = new Color(Random.value, Random.value, Random.value);
        var time = Random.Range(_minDeathTime, _maxDeathTime);

        yield return new WaitForSeconds(time);

        _mesh.material.color = Color.white;
        _isFirstCollision = true;
        _pool.Release(this);
    }
}
