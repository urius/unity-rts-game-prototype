using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACSMoveAnimationAdapter : MoveAnimationAdapterBase
{
    public override void Idle()
    {
        animator.SetBool("ACS_Idle", true);
    }

    public override void MoveBack()
    {
        animator.SetBool("ACS_WalkBack", true);
    }

    public override void MoveForward()
    {
        animator.SetBool("ACS_WalkForwad", true);
    }

    public override void TurnLeft()
    {
        animator.SetBool("ACS_TurnLeft", true);
    }

    public override void TurnRight()
    {
        animator.SetBool("ACS_TurnRight", true);
    }

    public override void Dead()
    {
        animator.SetBool("ACS_Dead3", true);
    }
}
