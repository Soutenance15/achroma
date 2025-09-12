using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Instance globale accessible partout (singleton)
    public static GameManager Instance;

    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver,
        TransitionLevel,
    }

    Button retryButton;
    Button quitButton;
    GameObject victoryDialog;

    // Position du dernier checkpoint
    private Vector3 lastCheckpointPos;

    // Propriété qui donne l'état courant du jeu
    // get : lecture partout
    // private set : modification possible uniquement depuis GameManager
    public GameState CurrentState { get; private set; } = GameState.Playing;

    void Awake()
    {
        // Si aucune instance, celle-ci devient l'unique instance
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Permet de garder le manager entre les scènes
        }
        else
        {
            Destroy(gameObject); // Détruit les doublons si jamais la scène recharge
        }

        retryButton = GameObject.Find("RetryButton").GetComponent<Button>();
        quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        victoryDialog = GameObject.Find("VictoryDialog");
        victoryDialog.SetActive(false);

        retryButton.onClick.AddListener(Reload);
        quitButton.onClick.AddListener(GoMenuStart);
    }

    void Start()
    {
        // TODO
        // Pour le moment necessaire apres une victoire et que le joueur revient pour jouer
        // a faire evoluer apres
        Instance.SetState(GameState.Playing);
    }

    void GoMenuStart()
    {
        // Le nom de scène doit correspondre dans Build Settings!
        SceneManager.LoadScene("Menu");
    }

    void Reload()
    {
        Debug.Log("Victoire");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void VictoryExit()
    {
        Time.timeScale = 0f;
        victoryDialog.SetActive(true);
    }

    // Méthode pour sauvegarder la position du checkpoint (à appeler depuis le perso)
    public void SaveCheckpoint(Vector3 pos)
    {
        lastCheckpointPos = pos;
        Debug.LogWarning("SAVE SAVE");
    }

    // Méthode pour replacer le personnage au dernier checkpoint (à appeler lors du respawn)
    public void RespawnCharacter()
    {
        CharacterController2D character = FindFirstObjectByType<CharacterController2D>();
        if (character != null)
        {
            // Replacer à la bonne position
            if (null != lastCheckpointPos && lastCheckpointPos != new Vector3(0, 0, 0))
                character.transform.position = lastCheckpointPos;
            else
                character.transform.position = character.defaultSpawnPosition;

            // AJOUT : remettre sa velocity à zéro
            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }

    // Méthode pour activer/désactiver la pause
    public void TogglePause()
    {
        // Si on est déjà en pause, on relance, sinon on fige
        if (CurrentState == GameState.Paused)
        {
            SetState(GameState.Playing); // Reprend le jeu
        }
        else if (CurrentState == GameState.Playing)
        {
            SetState(GameState.Paused); // Met en pause
        }
    }

    // Méthode pour Mettre le menu
    public void SetActiveMenuPause(GameObject menuPause)
    {
        if (CurrentState == GameState.Paused)
        {
            menuPause.SetActive(true);
        }
        else if (CurrentState == GameState.Playing)
        {
            menuPause.SetActive(false);
        }
    }

    // Dans SetState, fais gérer le timeScale et l’affichage de l’UI pause.
    public void SetState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.Paused:
                Time.timeScale = 0f;
                // TODO : afficher le panneau pause (UI)
                Debug.Log("PAUSE");
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                Debug.Log("PLAY");
                // TODO : masquer le panneau pause (UI)
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                Debug.Log("GAMEOVER");
                // TODO : afficher écran de fin
                break;
            case GameState.TransitionLevel:
                Debug.Log("TRANSITION NIVEAU");
                // TODO : Afficher écran loading
                break;
        }
    }

    // Méthode pour charger la scène suivante (niveau suivant)
    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        // Vérifie si cette scène existe dans la build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextSceneIndex);
        // Option : sinon, tu peux revenir au menu ou afficher "fin du jeu"
    }
}
