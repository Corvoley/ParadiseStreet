using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject powerUpParticles;
    private float endTime;

    public bool IsPowerUpActive => Time.time < endTime;

    protected void ActivateForDuration(float duration)
    {
        bool wasActive = IsPowerUpActive;
        endTime = Time.time + duration;
        if (!wasActive)
        {
            Debug.Log("Active? " + gameObject.activeInHierarchy);
            StartCoroutine(UpdateBehaviourCoroutine());
        }
    }

    private IEnumerator UpdateBehaviourCoroutine()
    {
        StartBehaviour();
        if (powerUpParticles != null) powerUpParticles.gameObject.SetActive(true);

        while (IsPowerUpActive)
        {
            UpdateBehaviour();
            yield return null;
        }
        if (powerUpParticles != null) powerUpParticles.gameObject.SetActive(false);
        EndBehaviour();
    }

    protected abstract void StartBehaviour();
    protected abstract void UpdateBehaviour();
    protected abstract void EndBehaviour();
    
    
    
}
