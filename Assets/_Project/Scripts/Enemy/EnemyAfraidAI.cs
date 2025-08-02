using UnityEngine;

public class EnemyAfraidAI : EnemyBaseAI
{
    override protected void Update()
    {
        base.Update();
    }

    override protected void IdleBehavior()
    {
        if (player != null)
        {
            SwitchState(EnemyAIBehavior.Retreat);
        }
    }
}
