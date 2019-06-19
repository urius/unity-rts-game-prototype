using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveAnimationAdapterBase : MonoBehaviour, IMoveAnimationAdapter
{
    protected MoveStates currentState = MoveStates.Undefined;
    protected Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeMoveState(MoveStates moveState)
    {
        if (currentState != moveState)
        {
            currentState = moveState;
            switch (currentState)
            {
                case MoveStates.Idle:
                    Idle();
                    break;
                case MoveStates.MovingForward:
                    MoveForward();
                    break;
                case MoveStates.MovingBack:
                    MoveBack();
                    break;
                case MoveStates.TurningRight:
                    TurnRight();
                    break;
                case MoveStates.TurningLeft:
                    TurnLeft();
                    break;
            }
        }
    }

    public abstract void Dead();

    public abstract void Idle();

    public abstract void MoveBack();

    public abstract void MoveForward();

    public abstract void TurnLeft();

    public abstract void TurnRight();
}
