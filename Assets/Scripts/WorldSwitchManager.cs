using UnityEngine;

public class WorldSwitchManager : MonoBehaviour
{
    // Etat courant du monde : true = noir, false = blanc
    public static bool isBlackWorld = false;

    // Delegate : toute fonction abonnée prend un booléan (le monde noir ou blanc)
    public delegate void WorldSwitchAction(bool isBlackWorld);
    public static event WorldSwitchAction OnWorldSwitch;

    void Update()
    {
        // Touche pour switcher le monde
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchWorld();
        }
    }

    public static void SwitchWorld()
    {
        // Toggle la variable : elle inverse entre true et false
        isBlackWorld = !isBlackWorld;
        // Préviens tous les abonnés (plateformes, pièges, etc.)
        // Déclenche un evenement pour les abonnés
        if (OnWorldSwitch != null)
            OnWorldSwitch(isBlackWorld);
    }
}
