public interface IMoveAnimationAdapter
{
    void ChangeMoveState(MoveStates moveState);
    void Idle();
    void MoveForward();
    void MoveBack();
    void TurnRight();
    void TurnLeft();
    void Dead();
}
