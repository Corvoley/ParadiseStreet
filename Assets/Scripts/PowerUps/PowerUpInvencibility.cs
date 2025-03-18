using UnityEngine;

public class PowerUpInvencibility : PowerUp
{
    public override void ActivatePowerUpBehaviour(PlayerController player)
    {
        player.gameObject.GetComponentInChildren<PowerUpBehaviourInvencibility>().Activate(PowerUpTime);
    }

 
}
