using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PowerUpScoreMultiplier : PowerUp
{
    [SerializeField] private int scoreMultiplier = 2;
    public override void ActivatePowerUpBehaviour(PlayerController player)
    {       
        player.gameObject.GetComponentInChildren<PowerUpBehaviourScoreMultiplier>().Activate(scoreMultiplier, PowerUpTime);
       
    }
}
