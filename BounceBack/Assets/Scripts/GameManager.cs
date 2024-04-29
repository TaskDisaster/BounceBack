using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Main man himself
    public static GameManager Instance;

    [Header("Player Variables")]
    #region Player
    public GameObject playerControllerPrefab;
    public GameObject playerPawnPrefab;
    public Pawn currentPlayer;
    public Transform playerSpawn;
    #endregion

    [Header("Enemy Variables")]
    #region Enemy
    public GameObject enemyControllerPrefab;
    public GameObject enemyPawnPrefab;
    public Pawn currentEnemy;
    public Transform enemySpawn;
    #endregion

    [Header("Game States")]
    #region GameStates
    public GameObject TitleStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverStateObject;
    public GameObject CreditsStateObject;
    public GameObject PauseStateObject;
    public enum GameState { Title, MainMenu, Options, Credits, Gameplay, GameOver, Pause, Idle, Restart };
    public GameState gameState;
    public GameState previousState;
    public KeyCode transitionKey;
    public KeyCode pauseKey;
    public bool playerSpawned;
    public bool pausedGame;
    public bool playerDied;
    public bool enemySpawned;
    public bool enemyDied;
    #endregion

    [Header("Wave Variables")]
    #region Wave
    public bool beginWaves = false;
    public int wave;
    public float secondsBetween;
    public float timeTillWave;
    public PlayerUIManager uiManager;
    #endregion

    [Header("Misc Variables")]
    #region Misc
    public FollowCam cam;
    public AudioClip mainMenuTrack;
    public AudioClip gameplayTrack;
    public bool isPlaying = false;
    #endregion


    private void Awake()
    {
        // Ensure the existance of only one
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy any copies
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Spawn the player immediately if there's no TitleStateObject
        if (TitleStateObject != null)
        {
            // Sets to title screen first
            gameState = GameState.Title;
        }
        else
        {
            SpawnPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TitleStateObject != null)
        {
            MakeDecision();
        }
    }

    public void MakeDecision()
    {
        // Game State FSM
        switch (gameState)
        {
            case GameState.Title:

                ActivateTitleState();

                // Transition to Main Menu
                if (Input.GetKey(transitionKey))
                {
                    ChangeState(GameState.MainMenu);
                }
                break;

            case GameState.MainMenu:

                ActivateMainMenuState();

                // Play the MainMenu music
                if (!isPlaying)
                {
                    AudioManager.Instance.PlayMusic(mainMenuTrack);
                    isPlaying = true;
                }

                break;

            case GameState.Options:

                ActivateOptionsState();

                break;

            case GameState.Gameplay:

                ActivateGameplayState();

                // Resume Game if time is paused
                if (pausedGame == true)
                {
                    Time.timeScale = 1;

                    // Also resume soundtrack
                    AudioManager.Instance.UnPause();

                    pausedGame = false;
                }

                // Play the soundtrack
                if (isPlaying)
                {
                    AudioManager.Instance.PlayMusic(gameplayTrack);
                    isPlaying = false;
                }

                // Spawn the player
                if (playerSpawned != true)
                {
                    SpawnPlayer();
                    playerSpawned = true;
                }
                
                /*
                // Spawn the enemy
                if (enemySpawned != true)
                {
                    SpawnEnemy();
                    enemySpawned = true;
                    timeTillWave = secondsBetween
                }
                */

                // Check if waves have begun
                if (beginWaves == true)
                {
                    // Check if all enemies are dead
                    if (currentEnemy == null)
                    {
                        // Update the timer every frame
                        timeTillWave -= Time.deltaTime;

                        // Time down until the next wave
                        if (timeTillWave <= 0)
                        {
                            StartNewWave();
                        }
                    }
                }

                // Check for transitions to Pause Menu
                if (Input.GetKeyDown(pauseKey))
                {
                    ChangeState(GameState.Pause);
                }

                break;

            case GameState.GameOver:

                ActivateGameOverState();

                // Stop playing the sound track
                AudioManager.Instance.Stop();
                isPlaying = false;

                // Transition to Main Menu
                if (Input.GetKey(transitionKey))
                {
                    ChangeState(GameState.Restart);
                }
                break;

            case GameState.Credits:

                ActivateCreditsState();

                break;

            case GameState.Pause:

                ActivatePauseState();

                // Pause Game
                if (pausedGame == false)
                {
                    Time.timeScale = 0;

                    pausedGame = true;
                }

                // pause the soundtrack too
                AudioManager.Instance.Pause();

                // Check for transitions
                if (Input.GetKeyDown(pauseKey))
                {
                    ChangeState(GameState.Gameplay);
                }

                break;

            case GameState.Idle:
                // Wait for transitions
                break;

            // Case will only play once, then switch to MainMenu
            case GameState.Restart:

                // Reset Wave Variables
                wave = 1;
                UpdateUI();
                beginWaves = false;
                timeTillWave = 0;

                // Reset Player Variables and Destroy Player
                if (currentPlayer != null)
                {
                    Destroy(currentPlayer.gameObject);
                }

                if (currentPlayer.GetController() != null)
                {
                    Destroy(currentPlayer.GetController().gameObject);
                }

                currentPlayer = null;
                playerSpawned = false;

                // Rest Enemy Variables and Destroy Enemy
                if (currentEnemy != null)
                {
                    Destroy(currentEnemy.gameObject);
                }

                if (currentEnemy.GetController() != null)
                {
                    Destroy(currentEnemy.GetController().gameObject);
                }

                currentEnemy = null;
                enemySpawned = false;

                // Reset pause variables
                pausedGame = false;
                Time.timeScale = 1;

                // Reset Pool
                ObjectPoolManager.Instance.ResetPool();
                ObjectPoolManager.Instance.ResetNumObj();

                ChangeState(GameState.MainMenu);
                break;
        }
    }

    #region Spawning
    public void SpawnPlayer()
    {
        // Chcek if there is a spawn point
        if (playerSpawn == null)
        {
            Debug.LogWarning("No Player Spawn Set!");
            return;
        }

        // Instantiate player
        GameObject newControllerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject newPawnObj = Instantiate(playerPawnPrefab, playerSpawn.position, Quaternion.identity);

        // Connect the player and controller
        Controller newController = newControllerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;
        newPawn.SetController(newController);

        // Set current player
        currentPlayer = newPawn;

        // Set the camera's target to the player
        cam.target = newPawnObj.transform;
    }

    public void SpawnEnemy()
    {
        // Check if there is a spawn point
        if (enemySpawn == null)
        {
            Debug.LogWarning("No Enemy Spawn Set!");
            return;
        }

        // Then spawn the enemy
        GameObject newControllerObj = Instantiate(enemyControllerPrefab, Vector3.zero, Quaternion.identity);
        GameObject newPawnObj = Instantiate(enemyPawnPrefab, enemySpawn.position, Quaternion.identity);

        // Connect Controllers and Pawns
        Controller newController = newControllerObj.GetComponent<Controller>();
        Pawn newPawn = newPawnObj.GetComponent<Pawn>();

        newController.pawn = newPawn;
        newPawn.SetController(newController);

        // Scale the health of the enemy with the wave
        newPawn.SetMaxHealth(newPawn.GetMaxHealth() * wave);

        // Set current enemy
        currentEnemy = newPawn;

        // Cast the newController as an AIController
        EnemyController enemyController = newController as EnemyController;

        if (enemyController != null)
        {
            enemyController.SetTarget(currentPlayer.gameObject);
        }
    }
    #endregion

    #region Wave Management
    public void UpdateUI()
    {
        uiManager.waveCounter.text = wave.ToString();
    }

    public void SpawnWave()
    {
        Debug.Log("Wave: " + wave);
        SpawnEnemy();

        UpdateUI();
        IncrementWave();
    }

    public void IncrementWave()
    {
        wave += 1;
    }

    public void StartNewWave()
    {
        // Spawn Wave
        SpawnWave();
        Debug.Log("Starting new wave!");

        // Reset and Increment the amount of bouncyballs that can be in play
        ObjectPoolManager.Instance.ResetPool();
        ObjectPoolManager.Instance.IncrementPool();

        // Reset the timer for the next wave
        timeTillWave = secondsBetween;
    }


    #endregion

    #region Game States

    public void SetPreviousState(GameState newState)
    {
        // Change the previous state;
        previousState = newState;
    }

    public void ChangeState(GameState newState)
    {
        // Set the previous state
        SetPreviousState(gameState);

        // Change the current state
        gameState = newState;
    }

    #region Deactivate States

    private void DeactivateAllStates()
    {
        TitleStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverStateObject.SetActive(false);
        CreditsStateObject.SetActive(false);
        PauseStateObject.SetActive(false);
    }

    public void DeactivateTitleState()
    {
        TitleStateObject.SetActive(false);
    }

    public void DeactivateMainMenuState()
    {
        MainMenuStateObject.SetActive(false);
    }

    public void DeactivateOptionsState()
    {
        OptionsStateObject.SetActive(false);
    }

    public void DeactivateGameplayState()
    {
        GameplayStateObject.SetActive(false);
    }

    public void DeactivateGameOverState()
    {
        GameOverStateObject.SetActive(false);
    }

    public void DeactivateCreditsState()
    {
        CreditsStateObject.SetActive(false);
    }

    public void DeactivatePauseState()
    {
        PauseStateObject.SetActive(false);
    }

    #endregion

    #region Activate States

    private void ActivateAllStates()
    {
        TitleStateObject.SetActive(true);
        MainMenuStateObject.SetActive(true);
        OptionsStateObject.SetActive(true);
        GameplayStateObject.SetActive(true);
        GameOverStateObject.SetActive(true);
        CreditsStateObject.SetActive(true);
        PauseStateObject.SetActive(true);
    }

    public void ActivateTitleState()
    {
        DeactivateAllStates();

        gameState = GameState.Title;

        TitleStateObject.SetActive(true);
    }

    public void ActivateMainMenuState()
    {
        DeactivateAllStates();

        gameState = GameState.MainMenu;

        MainMenuStateObject.SetActive(true);
    }

    public void ActivateOptionsState()
    {
        DeactivateAllStates();

        gameState = GameState.Options;

        OptionsStateObject.SetActive(true);
    }

    public void ActivateGameplayState()
    {
        DeactivateAllStates();

        gameState = GameState.Gameplay;

        GameplayStateObject.SetActive(true);
    }

    public void ActivateGameOverState()
    {
        DeactivateAllStates();

        gameState = GameState.GameOver;

        GameOverStateObject.SetActive(true);
    }

    public void ActivateCreditsState()
    {
        DeactivateAllStates();

        gameState = GameState.Credits;

        CreditsStateObject.SetActive(true);
    }

    public void ActivatePauseState()
    {
        DeactivateAllStates();

        gameState = GameState.Pause;

        PauseStateObject.SetActive(true);
    }

    public void ActivateRestartState()
    {
        DeactivateAllStates();

        gameState = GameState.Restart;
    }

    #endregion

    #endregion
}
