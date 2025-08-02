using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    public PlayerMovement playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  

    public void OnWandAttackEnds()
    {
        playerMovement.ToggleCanMove(true);
    }
}
