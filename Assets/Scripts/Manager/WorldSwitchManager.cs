using UnityEngine;

public class WorldSwitchManager : MonoBehaviour
{
    // Etat du monde : false = blanc (normal), true = noir (alterné)
    public static bool isNormalWorld = false;

    // Delegate : toutes fonctions abonnées prennent un booléen
    public delegate void WorldSwitchAction(bool isNormalWorld);
    public static event WorldSwitchAction OnWorldSwitch;

    void Awake()
    {
        // Synchronisation tous les objets dès le démarrage
        if (OnWorldSwitch != null)
            OnWorldSwitch(isNormalWorld);
    }

    void Update()
    {
        // Si on MAINTIENT la touche espace, monde alterné
        // Sinon, retour au monde normal
        bool shouldBeNormalWorld = Input.GetKey(KeyCode.Space);

        // On ne diffuse l'événement que si l'état change
        if (isNormalWorld != shouldBeNormalWorld)
        {
            isNormalWorld = shouldBeNormalWorld;
            if (OnWorldSwitch != null)
                OnWorldSwitch(isNormalWorld);
        }
    }

    // Option : méthode pour forcer la synchro depuis ailleurs dans le code
    public static void SetWorldState(bool toNormal)
    {
        if (isNormalWorld != toNormal)
        {
            isNormalWorld = toNormal;
            if (OnWorldSwitch != null)
                OnWorldSwitch(isNormalWorld);
        }
    }
}
