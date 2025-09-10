using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (null != lastCheckpointPos)
            {
                character.transform.position = lastCheckpointPos;
            }
            else
            {
                character.transform.position = character.defaultSpawn.position;
            }
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
    public void ShowMenu()
    {
        Debug.Log("MENU");
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
