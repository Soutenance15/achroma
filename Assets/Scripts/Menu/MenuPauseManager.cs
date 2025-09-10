using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPauseManager : MonoBehaviour
{
    // Quitter dialog
    GameObject quitDialog;
    GameObject instructionDialog;
    Button showDialogQuitButton;
    Button cancelQuitButton;
    Button confirmQuitButton;

    // Continuer Dialog

    Button continueGameButton;

    // Instruction dialog
    Button instructionButton;
    Button backMenuButton;
    Button playButton;

    void Awake()
    {
        instructionButton = GameObject.Find("InstructionButton").GetComponent<Button>();
        backMenuButton = GameObject.Find("BackMenuButton").GetComponent<Button>();
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        continueGameButton = GameObject.Find("ContinueGameButton").GetComponent<Button>();

        // Trouver les objets dans la hiérarchie par nom
        instructionDialog = GameObject.Find("InstructionDialog");

        quitDialog = GameObject.Find("QuitDialog");
        showDialogQuitButton = GameObject.Find("ShowDialogQuitButton").GetComponent<Button>();
        confirmQuitButton = GameObject.Find("ConfirmQuitButton").GetComponent<Button>();
        cancelQuitButton = GameObject.Find("CancelQuitButton").GetComponent<Button>();

        // S'assurer que le panneau n'est pas visible par défaut
        quitDialog.SetActive(false);
        instructionDialog.SetActive(false);

        // Ajouter les listeners par code
        instructionButton.onClick.AddListener(ShowInstructionDialog);
        backMenuButton.onClick.AddListener(HideInstructionDialog);
        playButton.onClick.AddListener(ResumeGame);

        continueGameButton.onClick.AddListener(ResumeGame);

        showDialogQuitButton.onClick.AddListener(ShowQuitDialog);
        confirmQuitButton.onClick.AddListener(QuitApp);
        cancelQuitButton.onClick.AddListener(HideQuitDialog);
    }

    void ShowInstructionDialog()
    {
        instructionDialog.SetActive(true);
    }

    void HideInstructionDialog()
    {
        instructionDialog.SetActive(false);
    }

    void ResumeGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameManager.GameState.Playing);
            GameManager.Instance.SetActiveMenuPause(gameObject);
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
