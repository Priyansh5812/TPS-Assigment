using UnityEngine;

public class ParticleTimeout : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystem;
    ParticlePooler pooler;
    public void InitializePoolerReference(ParticlePooler pooler)
    { 
        this.pooler = pooler;
    }

    private void OnEnable()
    {
        Invoke(nameof(PoolSelf), particleSystem.main.duration + 0.25f);
    }

    void PoolSelf()
    {
        pooler.ReturnToPool(this);
    }
}
