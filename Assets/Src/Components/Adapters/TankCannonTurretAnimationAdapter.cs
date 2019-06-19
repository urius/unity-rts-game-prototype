using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCannonTurretAnimationAdapter : MonoBehaviour, ITurretAnimationAdapter
{
    // Start is called before the first frame update
    public Animator[] turretAnimators;
    public void Fire(int weaponIndex = -1)
    {
        if (weaponIndex == -1)
        {
            foreach (var turretAnimator in turretAnimators)
            {
                turretAnimator.SetBool("Fire", true);
            }
        }
        else
        {
            turretAnimators[weaponIndex].SetBool("Fire", true);
        }
    }
}
