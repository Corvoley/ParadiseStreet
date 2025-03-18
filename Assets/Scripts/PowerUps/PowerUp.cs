using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp: MonoBehaviour
{
    [SerializeField] protected float powerUpTime = 10;
    protected float PowerUpTime => powerUpTime;
    private void Start()
    {
        GameManager.Instance.OnHealthDepleted += GameManager_OnHealthDepleted;
    }
    public abstract void ActivatePowerUpBehaviour(PlayerController player);

    private void GameManager_OnHealthDepleted()
    {
        Destroy(gameObject);
    }
}
