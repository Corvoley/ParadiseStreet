using UnityEngine;

public class PowerUpHeal : PowerUp
{
    public override void ActivatePowerUpBehaviour(PlayerController player)
    {
        player.gameObject.GetComponentInChildren<PowerUpBehaviourHeal>().Activate(PowerUpTime);
    }

  
}
