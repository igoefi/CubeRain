using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _fadePerTime;
    [SerializeField] private float _minDeathTime = 2;
    [SerializeField] private float _maxDeathTime = 5;
    
    [Header("Настройки взрыва")]
    [SerializeField] private float _explosionForse;
    [SerializeField] private float _explosionRadius;
    
    private ObjectPool<Bomb> _pool;
    private MeshRenderer _meshRenderer;
    private Rigidbody _body;
    private WaitForSeconds _fadeWait;
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _body = GetComponent<Rigidbody>();
        _fadeWait = new WaitForSeconds(_fadePerTime);
    }

    public void SetPool(ObjectPool<Bomb> pool) =>
        _pool = pool;
    
    public void StartFading()
    {
        Color color = _meshRenderer.material.color;
        _meshRenderer.material.color = 
            new Color(color.r, color.g, color.b, 1);
        StartCoroutine(FadeUntilExplosion(Random.Range(_minDeathTime, _maxDeathTime)));
    }

    private void ExplosionAndRelease()
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

        _pool.Release(this);
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
        
        ExplosionAndRelease();
    }
}
