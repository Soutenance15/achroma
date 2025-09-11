using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevelManager : MonoBehaviour
{
    Button backButton;

    void Start()
    {
        // Références dynamiques (si tes boutons s’appellent "ButtonLevel1", "ButtonLevel2", ...)
        for (int i = 1; i <= 1; i++) // suppose 5 niveaux
        {
            string btnName = "ButtonLevel" + i;
            Button btn = GameObject.Find(btnName).GetComponent<Button>();
            TMP_Text textMeshPro = btn.GetComponentInChildren<TMP_Text>();
            if (textMeshPro != null)
            {
                textMeshPro.text = "Niveau " + i;
            }
            int levelIndex = i; // capture la valeur correcte
            btn.onClick.AddListener(() => LoadLevel(levelIndex));
        }
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(HideDialog);
    }

    void HideDialog()
    {
        gameObject.SetActive(false);
    }

    void LoadLevel(int index)
    {
        // Les noms de scènes doivent correspondre dans Build Settings!
        SceneManager.LoadScene("Niveau" + index);
    }
}
