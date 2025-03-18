using UnityEngine;

public class PowerUpBehaviourHeal : PowerUpBehaviour
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
        
    }

    protected override void StartBehaviour()
    {
       gameManager.IncreaseHealth();
    }

    protected override void UpdateBehaviour()
    {
       
    }
}
