using UnityEngine;

public class ShooterModuleView
{
    Animator animator;
    int hash_shootAnimation;
    public ShooterModuleView(Animator animator)
    { 
        this.animator = animator;
        InitAnimationHashes();
    }

    void InitAnimationHashes()
    {
        hash_shootAnimation = Animator.StringToHash("Firing");
    }

    public void PlayShootAnimation()
    {
        animator.Play(hash_shootAnimation, 0);
    }

}