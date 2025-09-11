using UnityEngine;

// Etat du monde : false = noir (alterné), true = blanc (normal), null = aucun (tout trigger/traversable)
public class WorldSwitchManager : MonoBehaviour
{
    // Permet trois états : null = aucun monde, sinon normal (true) ou alterné (false)
    public static bool? isNormalWorld = null;

    // Delegate : toutes fonctions abonnées prennent un booléen nullable
    public delegate void WorldSwitchAction(bool? isNormalWorld);
    public static event WorldSwitchAction OnWorldSwitch;

    void Awake()
    {
        // Synchronisation tous les objets dès le démarrage
        if (OnWorldSwitch != null)
            OnWorldSwitch(isNormalWorld);
    }

    void Update()
    {
        // Par défaut, aucun monde actif tant que rien n'est maintenu
        bool? newWorld = null;

        // Si on MAINTIENT une des flèches, on fixe le monde
        if (Input.GetKey(KeyCode.LeftArrow))
            newWorld = true; // Monde normal (blanc)
        else if (Input.GetKey(KeyCode.RightArrow))
            newWorld = false; // Monde alterné (noir)

        // Diffuse l'événement que si l'état change
        if (isNormalWorld != newWorld)
        {
            isNormalWorld = newWorld;
            if (OnWorldSwitch != null)
                OnWorldSwitch(isNormalWorld);
        }
    }

    // Option : méthode pour forcer la synchro ailleurs dans le code (prend bool? nullable)
    public static void SetWorldState(bool? toNormal)
    {
        Debug.Log("Test appuie");
        if (isNormalWorld != toNormal)
        {
            isNormalWorld = toNormal;
            if (OnWorldSwitch != null)
                OnWorldSwitch(isNormalWorld);
        }
    }
}
