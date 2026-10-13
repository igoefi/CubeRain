using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer))]
public class FallenCube : MonoBehaviour
{
    [SerializeField] private float _minDeathTime = 2;
    [SerializeField] private float _maxDeathTime = 5;
    [SerializeField] private LayerMask _ignoreLayers;
    
    private MeshRenderer _mesh;
    private bool _isFirstCollision = true;

    public event Action<FallenCube> CubeDeaded;

    private void Start() =>
        _mesh = GetComponent<MeshRenderer>();

    private void OnCollisionEnter(Collision collision)
    {
        if((_ignoreLayers.value & (1 << collision.gameObject.layer)) != 0)
            return;
        
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
        CubeDeaded?.Invoke(this);
    }
}
