using UnityEngine;

public class PowerUpBehaviourScoreMultiplier : PowerUpBehaviour
{
    [SerializeField] private GameManager gameManager;
    private int scoreMultiplier;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void Activate(int multiplier, float duration)
    {
        Debug.Log("Ativando power up behaviour");
        scoreMultiplier = multiplier;
        ActivateForDuration(duration);
    }
    protected override void EndBehaviour()
    {
        gameManager.temporaryScoreMultiplier = 1;
    }

    protected override void StartBehaviour()
    {
        gameManager.temporaryScoreMultiplier = scoreMultiplier;
    }

    protected override void UpdateBehaviour()
    {

    }
}
