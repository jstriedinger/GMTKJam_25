using System;
using UnityEngine;

public class EnemyChaseAI : EnemyBaseAI
{

    public bool isDead;
    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }
    
    public void OnHitByLinedraw()
    {
        // Handle hit logic, e.g., play animation, sound, etc.
        isDead = true;
        Destroy(gameObject);
        Debug.Log("line hit this enemy, die!");
        
    }
}
