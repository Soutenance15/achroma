using UnityEngine;

public class WorldSwitchManager : MonoBehaviour
{
    // Etat du monde : false = blanc (normal), true = noir (alterné)
    public static bool isBlackWorld = false;

    // Delegate : toutes fonctions abonnées prennent un booléen
    public delegate void WorldSwitchAction(bool isBlackWorld);
    public static event WorldSwitchAction OnWorldSwitch;

    void Awake()
    {
        // Synchronisation tous les objets dès le démarrage
        if (OnWorldSwitch != null)
            OnWorldSwitch(isBlackWorld);
    }

    void Update()
    {
        // Si on MAINTIENT la touche espace, monde alterné
        // Sinon, retour au monde normal
        bool shouldBeBlackWorld = Input.GetKey(KeyCode.Space);

        // On ne diffuse l'événement que si l'état change
        if (isBlackWorld != shouldBeBlackWorld)
        {
            isBlackWorld = shouldBeBlackWorld;
            if (OnWorldSwitch != null)
                OnWorldSwitch(isBlackWorld);
        }
    }

    // Option : méthode pour forcer la synchro depuis ailleurs dans le code
    public static void SetWorldState(bool toBlack)
    {
        if (isBlackWorld != toBlack)
        {
            isBlackWorld = toBlack;
            if (OnWorldSwitch != null)
                OnWorldSwitch(isBlackWorld);
        }
    }
    
}
