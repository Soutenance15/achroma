using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStartManager : MonoBehaviour
{
    GameObject quitDialog;
    Button showDialogQuitButton;
    Button cancelQuitButton;
    Button confirmQuitButton;
    Button menuLevelButton;

    private GameObject menuLevelInstance = null;

    void Awake()
    {
        menuLevelButton = GameObject.Find("MenuLevel").GetComponent<Button>();

        // Trouver les objets dans la hiérarchie par nom
        quitDialog = GameObject.Find("QuitDialog");
        showDialogQuitButton = GameObject.Find("ShowDialogQuitButton").GetComponent<Button>();
        confirmQuitButton = GameObject.Find("ConfirmQuitButton").GetComponent<Button>();
        cancelQuitButton = GameObject.Find("CancelQuitButton").GetComponent<Button>();

        // S'assurer que le panneau n'est pas visible par défaut
        quitDialog.SetActive(false);

        // Ajouter les listeners par code

        menuLevelButton.onClick.AddListener(ShowMenuLevel);

        showDialogQuitButton.onClick.AddListener(ShowQuitDialog);
        confirmQuitButton.onClick.AddListener(QuitApp);
        cancelQuitButton.onClick.AddListener(HideQuitDialog);
    }

    void ShowMenuLevel()
    {
        if (menuLevelInstance == null)
        {
            // Charge et instancie le prefab la première fois
            GameObject prefab = Resources.Load<GameObject>("Prefabs/MenuLevel");
            if (prefab != null)
            {
                menuLevelInstance = Instantiate(prefab);
            }
            else
                Debug.LogError("menuLevel prefab non trouvé dans Resources/Prefabs !");
        }
        else
        {
            // Si déjà instancié (cache/réaffiche selon besoin, par ex avec SetActive)
            menuLevelInstance.SetActive(true);
        }
    }

    void ShowQuitDialog()
    {
        quitDialog.SetActive(true);
    }

    void HideQuitDialog()
    {
        quitDialog.SetActive(false);
    }

    void QuitApp()
    {
        Application.Quit();
    }
}
