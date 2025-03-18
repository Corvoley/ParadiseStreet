using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour, IPooledObject
{
    public Action OnBorderCollision;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private float finalSpeed;
    private Coroutine changeDirectionCor;



    public void SetupEnemy(GameManager gameManager)
    {
        this.gameManager = gameManager;
        transform.up = Vector3.zero - transform.position;
        transform.up = GetDirectionWithinAngle(90);
        finalSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        changeDirectionCor = StartCoroutine(ChangeDirectionCor());
    }



    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.AddForce(transform.up * finalSpeed);
        }
    }
    private IEnumerator ChangeDirectionCor()
    {
        while (true)
        {
            float randomTime = UnityEngine.Random.Range(0.1f, 1f);
            yield return new WaitForSeconds(randomTime);
            transform.up = GetDirectionWithinAngle(90);
        }

    }

    private Vector2 GetDirectionWithinAngle(float angle)
    {
        return Quaternion.Euler(0, 0, UnityEngine.Random.Range(-angle / 2, angle / 2)) * transform.up;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyZone"))
        {
            gameManager.DespawnEnemy(this);
            gameManager.SpawnEnemy();
        }
    }

    public void OnInstantiated()
    {
        
    }

    public void OnEnabledFromPool()
    {
        
    }

    public void OnDisabledFromPool()
    {
        StopCoroutine(changeDirectionCor);
    }
}
