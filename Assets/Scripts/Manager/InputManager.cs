using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private GameObject menuPause;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tente de trouver MenuPause dans la scène
        menuPause = GameObject.Find("MenuPause");
        if (menuPause != null)
            menuPause.SetActive(false);
    }

    void Update()
    {
        // Le switch de monde se fait directement dans WorldSwitchManager

        // Pause (P ou Escape)
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPause != null)
            {
                GameManager.Instance.TogglePause();
                GameManager.Instance.SetActiveMenuPause(menuPause);
            }
            // Sinon, rien (pas d’erreur)
        }

        // Reset scène (R)
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }
    }
}
