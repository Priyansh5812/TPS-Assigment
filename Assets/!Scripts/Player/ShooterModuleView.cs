using UnityEngine;

// Handles the visual side of shooting such as animation and impact particles
public class ShooterModuleView
{
    Animator animator;

    // Cached hash for the firing animation state
    int hash_shootAnimation;

    // Pool used to spawn hit particles without repeated allocations
    ParticlePooler particlePooler;

    public ShooterModuleView(Animator animator)
    { 
        this.animator = animator;
        InitAnimationHashes();
        particlePooler = GameObject.FindAnyObjectByType<ParticlePooler>();
    }

    void InitAnimationHashes()
    {
        hash_shootAnimation = Animator.StringToHash("Firing");
    }

    public void PlayShootAnimation()
    {
        animator.Play(hash_shootAnimation, 0);
    }

    public void PlayParticleAt(Vector3 worldPosition , Vector3 normal)
    {
        // Places an impact particle slightly above the hit surface and activates it
        var particle =  particlePooler.Get();
        particle.transform.position = worldPosition + normal * 0.15f;
        particle.gameObject.SetActive(true);
    }

}
