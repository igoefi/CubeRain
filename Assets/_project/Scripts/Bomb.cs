using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _fadePerTime;
    [SerializeField] private float _minDeathTime = 2;
    [SerializeField] private float _maxDeathTime = 5;
    
    [Header("Настройки взрыва")]
    [SerializeField] private float _explosionForse;
    [SerializeField] private float _explosionRadius;
    
    private MeshRenderer _meshRenderer;
    private WaitForSeconds _fadeWait;

    public event Action<Bomb> BombExploded;
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _fadeWait = new WaitForSeconds(_fadePerTime);
    }
    
    public void StartFading()
    {
        Color color = _meshRenderer.material.color;
        _meshRenderer.material.color = 
            new Color(color.r, color.g, color.b, 1);
        StartCoroutine(FadeUntilExplosion(Random.Range(_minDeathTime, _maxDeathTime)));
    }

    private void Explosion()
    {
        Vector3 position = transform.position;
        RaycastHit[] casts = Physics.SphereCastAll(position,
            _explosionRadius, Vector3.one, _explosionRadius);

        foreach (var cast in casts)
        {
            var distance = Vector3.Distance(cast.transform.position, position);
            var force = (1 - distance / _explosionRadius) * _explosionForse;
            var direction = (cast.transform.position - position).normalized * force;

            if (cast.rigidbody != null)
                cast.rigidbody.AddForce(direction, ForceMode.Impulse);
        }

        BombExploded?.Invoke(this);
    }

    private IEnumerator FadeUntilExplosion(float time)
    {
        float nowTime = 0;
        
        while (nowTime < time)
        {
            yield return _fadeWait;

            nowTime += _fadePerTime;
            Color color =  _meshRenderer.material.color;
            _meshRenderer.material.color = 
                new Color(color.r, color.g, color.b, 1 - nowTime / time);
        }
        
        Explosion();
    }
}
