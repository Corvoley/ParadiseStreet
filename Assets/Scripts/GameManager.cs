using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Action OnScoreUpdate;
    public Action OnHealthDepleted;
    public Action OnHealthChanged;
    public Action OnPlayerSpawned;

    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform enemyContainer;
    [SerializeField] private ObjPool<EnemyController> enemyControllerPool;

    [SerializeField] private GameObject[] powerUpsArray;
    [SerializeField] private bool canSpawnPowerUps = true;
    [SerializeField] private float powerUpMinCooldown;
    [SerializeField] private float powerUpMaxCooldown;
    [SerializeField] private float powerUpDestroyTimer;

    [SerializeField] private int maxNumberOfEnemies;
    [SerializeField] private int numberOfEnemies;
    [SerializeField] private float enemyIncreaseCooldown;

    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private SpriteRenderer groundSprite;
    [SerializeField] private UiAudioController uiAudioController;
    [SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] private UIManager uiManager;


    [SerializeField] private int enemyCounter;
    public static GameObject CurrentPackage { get; private set; }

    public GameObject PlayerRef { get; private set; }

    public int Score { get; private set; }
    public int Health { get; private set; }

    public int temporaryScoreMultiplier = 1;
    public bool canBeDamaged = true;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        OnHealthDepleted += GameManager_OnHealthDepleted;
        enemyControllerPool.Initialize();
    }

    private IEnumerator DifficultScaleCor()
    {
        while (numberOfEnemies < maxNumberOfEnemies)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemyIncreaseCooldown);
        }
    }
    private void Start()
    {
        musicPlayer.PlayStartMenuMusic();
    }

    private IEnumerator PowerUpSpawnCor()
    {
        if (powerUpsArray.Length > 0)
        {
            while (canSpawnPowerUps)
            {
                float randomTimer = UnityEngine.Random.Range(powerUpMinCooldown, powerUpMaxCooldown);
                int randomPowerUp = UnityEngine.Random.Range(0, powerUpsArray.Length);
                yield return new WaitForSeconds(randomTimer);
                GameObject powerUp = Instantiate(powerUpsArray[randomPowerUp], GetMapRandomSpawnPos(), Quaternion.identity, transform);
                Destroy(powerUp, powerUpDestroyTimer);
            }
        }
    }


    private void SpawnPlayer()
    {
        PlayerRef = Instantiate(playerPrefab, transform.position, Quaternion.identity, transform);
        PlayerRef.GetComponent<PlayerController>().OnPackageCollected += PlayerController_OnPackageCollected;
        PlayerRef.GetComponent<PlayerController>().OnEnemyCollision += PlayerController_OnEnemyCollision;
        PlayerRef.GetComponent<PlayerController>().OnPowerUpCollected += PlayerController_OnPowerUpCollision;
        PlayerRef.GetComponent<PlayerController>().groundSprite = groundSprite;
        playerCamera.Target.TrackingTarget = PlayerRef.transform;
        Score = 0;
        Health = 5;
        OnPlayerSpawned?.Invoke();

    }

    public void SpawnEnemy()
    {
        float posX = groundSprite.sprite.bounds.size.x / 2 * groundSprite.transform.localScale.x;
        float posY = groundSprite.sprite.bounds.size.y / 2 * groundSprite.transform.localScale.y;

        var posVer = new Vector2(GetRandomBetweenTwoNumbers(-posX, posX), UnityEngine.Random.Range(-posY, posY));

        var posHor = new Vector2(UnityEngine.Random.Range(-posX, posX), GetRandomBetweenTwoNumbers(-posY, posY));

        var enemy = enemyControllerPool.GetFromPool(UnityEngine.Random.value < 0.5f ? posVer : posHor, Quaternion.identity, enemyContainer);
        enemy.GetComponent<EnemyController>().SetupEnemy(this);
        numberOfEnemies++;

    }
    public void DespawnEnemy(EnemyController enemy)
    {
        enemyControllerPool.ReturnToPool(enemy);
        numberOfEnemies--;
    }

    public void DespawnAllEnemies()
    {
        while (enemyContainer.childCount > 0)
        {
            DespawnEnemy(enemyContainer.GetChild(0).GetComponent<EnemyController>());
        }
    }


    public void IncreasePlayerScore()
    {
        Score += 1 * temporaryScoreMultiplier;
        OnScoreUpdate?.Invoke();
    }

    public void IncreaseHealth()
    {
        Health++;
        Health = Math.Clamp(Health, 0, 5);
        OnHealthChanged?.Invoke();
    }

    public void DecreaseHealth()
    {
        if (canBeDamaged)
        {
            Health--;
            Health = Math.Clamp(Health, 0, 5);
            if (Health <= 0)
            {
                OnHealthDepleted?.Invoke();
            }
            OnHealthChanged?.Invoke();
        }

    }
    public void RestartGame()
    {
        SpawnPlayer();
        SpawnPackage();
        StartCoroutine(PowerUpSpawnCor());

        for (int i = 0; i < enemyCounter; i++)
        {
            SpawnEnemy();
        }
        StartCoroutine(DifficultScaleCor());
        musicPlayer.PlayMainTrackMusic();   
    }
    private void EndGame()
    {
        StopAllCoroutines();
        DespawnAllEnemies();
        Destroy(CurrentPackage);
        Destroy(PlayerRef);
    }

    public void PauseGame()
    {
        uiManager.TooglePauseScreen();
        if (Time.timeScale == 0)
        {           
            Time.timeScale = 1;
        }
        else
        {            
            Time.timeScale = 0;
        }
    }
    private float GetRandomBetweenTwoNumbers(float a, float b)
    {
        return UnityEngine.Random.value < 0.5f ? a : b;
    }

    #region On Events
    private void GameManager_OnHealthDepleted()
    {
        EndGame();
    }
    private void PlayerController_OnEnemyCollision()
    {
        DecreaseHealth();
        if (canBeDamaged)
        {            
            uiAudioController.PlayEnemyHitSound();
        }
    }
    private void PlayerController_OnPackageCollected()
    {
        CurrentPackage = null;
        IncreasePlayerScore();
        SpawnPackage();
        uiAudioController.PlayCoinSound();
    }

    private void PlayerController_OnPowerUpCollision()
    {
        uiAudioController.PlayPowerUpSound();
    }


    #endregion



    #region Package Controller
    private void SpawnPackage()
    {
        CurrentPackage = Instantiate(packagePrefab, GetMapRandomSpawnPos(), Quaternion.identity, transform);
    }



    private Vector2 GetMapRandomSpawnPos()
    {
        float posX = groundSprite.sprite.bounds.size.x / 2 * groundSprite.transform.localScale.x;
        float posY = groundSprite.sprite.bounds.size.y / 2 * groundSprite.transform.localScale.y;

        return new Vector2(UnityEngine.Random.Range(-posX, posX), UnityEngine.Random.Range(-posY, posY));
    }
    #endregion











}
