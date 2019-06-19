using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveStates
{
    Undefined,
    Idle,
    MovingForward,
    MovingBack,
    TurningRight,
    TurningLeft,
}
public class TankCannonMoveAnimationAdapter : MoveAnimationAdapterBase
{
    public override void Idle()
    {
        animator.SetBool("Idle", true);
    }

    public override void MoveBack()
    {
        animator.SetBool("MoveBack", true);
    }

    public override void MoveForward()
    {
        animator.SetBool("MoveForward", true);
    }

    public override void TurnLeft()
    {
        animator.SetBool("MoveForward", true);
    }

    public override void TurnRight()
    {
        animator.SetBool("MoveForward", true);
    }

    public override void Dead()
    {
        animator.SetBool("Dead1", true);
    }
}
