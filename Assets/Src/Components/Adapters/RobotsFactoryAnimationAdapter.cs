using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsFactoryAnimationAdapter : MoveAnimationAdapterBase
{
    private Animator _animator;

    // Start is called before the first frame update
    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public override void Dead()
    {
        _animator.SetBool("Destroy2", true);
    }

    public override void Idle()
    {
        _animator.SetBool("Idle", true);
    }

    public override void MoveBack()
    {
    }

    public override void MoveForward()
    {
    }

    public override void TurnLeft()
    {
    }

    public override void TurnRight()
    {
    }

}
