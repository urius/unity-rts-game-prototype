
public class RobotsFactoryAnimationAdapter : MoveAnimationAdapterBase
{
    public override void Dead()
    {
        animator.SetBool("Destroy2", true);
    }

    public override void Idle()
    {
        animator.SetBool("Idle", true);
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
