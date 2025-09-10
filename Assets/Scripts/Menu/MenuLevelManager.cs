using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLevelManager : MonoBehaviour
{
    void Start()
    {
        // Références dynamiques (si tes boutons s’appellent "ButtonLevel1", "ButtonLevel2", ...)
        for (int i = 1; i <= 2; i++) // suppose 5 niveaux
        {
            string btnName = "ButtonLevel" + i;
            Button btn = GameObject.Find(btnName).GetComponent<Button>();
            int levelIndex = i; // capture la valeur correcte
            btn.onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int index)
    {
        // Les noms de scènes doivent correspondre dans Build Settings!
        SceneManager.LoadScene("Niveau" + index);
    }
}
