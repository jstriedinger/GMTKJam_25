using UnityEngine;

public class EnemyRangeAttackAI : EnemyBaseAI
{
    override protected void Update()
    {
        base.Update();
    }

    override protected void IdleBehavior()
    {
        if (player != null)
        {
            SwitchState(EnemyAIBehavior.Shoot);
        }
    }

    public override void SetPlayer(GameObject player)
    {
        if (player != null)
        {
            this.player = player;
        }
    }
}
