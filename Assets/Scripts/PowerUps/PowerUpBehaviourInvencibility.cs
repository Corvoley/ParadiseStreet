using UnityEngine;

public class PowerUpBehaviourInvencibility : PowerUpBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Activate(float duration)
    {     
        ActivateForDuration(duration);
    }

    protected override void EndBehaviour()
    {
        gameManager.canBeDamaged = true;
    }

    protected override void StartBehaviour()
    {
        gameManager.canBeDamaged = false;
    }

    protected override void UpdateBehaviour()
    {
        
    }
}
