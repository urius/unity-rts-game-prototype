using UnityEngine;

public class CommonTurretAnimationAdapter : ITurretAnimationAdapter
{
    public void Fire(Animator turretAnimator)
    {
        turretAnimator.SetBool("Fire", true);
    }
}
