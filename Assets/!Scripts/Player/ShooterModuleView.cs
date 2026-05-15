using UnityEngine;

public class ShooterModuleView
{
    Animator animator;
    int hash_shootAnimation;
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
        var particle =  particlePooler.Get();
        particle.transform.position = worldPosition + normal * 0.15f;
        particle.gameObject.SetActive(true);
    }

}