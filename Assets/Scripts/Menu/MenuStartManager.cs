using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuStartManager : MonoBehaviour
{
    GameObject quitDialog;
    Button showDialogQuitButton;
    Button cancelQuitButton;
    Button confirmQuitButton;

    void Awake()
    {
        // Trouver les objets dans la hiérarchie par nom
        quitDialog = GameObject.Find("QuitDialog");
        showDialogQuitButton = GameObject.Find("ShowDialogQuitButton").GetComponent<Button>();
        confirmQuitButton = GameObject.Find("ConfirmQuitButton").GetComponent<Button>();
        cancelQuitButton = GameObject.Find("CancelQuitButton").GetComponent<Button>();

        // S'assurer que le panneau n'est pas visible par défaut
        quitDialog.SetActive(false);

        // Ajouter les listeners par code
        showDialogQuitButton.onClick.AddListener(ShowQuitDialog);
        confirmQuitButton.onClick.AddListener(QuitApp);
        cancelQuitButton.onClick.AddListener(HideQuitDialog);
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
