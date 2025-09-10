using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        // Le Switch de monde se fait directement dans WorldSwitchManager

        if (Input.GetKeyDown(KeyCode.M))
            GameManager.Instance.ShowMenu();

        // Pause (ex: touche P ou Escape)
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            GameManager.Instance.TogglePause();

        // Reset scène (ex: touche R)
        if (Input.GetKeyDown(KeyCode.R))
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
    }
}
