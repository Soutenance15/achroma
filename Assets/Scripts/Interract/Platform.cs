using UnityEngine;

public class Platform : MonoBehaviour
{
    // Indique la couleur de la plateforme (true = noir, false = blanc)
    public bool myColorIsBlack = false;

    // Référence sur le collider à activer/désactiver
    private Collider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    // Quand l'objet devient actif, il "s'abonne" à l'événement du manager
    // Comme addEventListener en JavaScript, il "écoute"
    void OnEnable()
    {
        WorldSwitchManager.OnWorldSwitch += SetActiveState;
    }

    // Quand l'objet est désactivé/devenu inutilisé, il "arrête d'écouter"
    void OnDisable()
    {
        WorldSwitchManager.OnWorldSwitch -= SetActiveState;
    }

    // Méthode appelée automatiquement sur changement de monde
    public void SetActiveState(bool isBlackWorld)
    {
        // Si le monde est de même couleur que la plateforme => active la collision (tangible)
        if (myColorIsBlack == isBlackWorld)
            myCollider.enabled = true;
        else
            myCollider.enabled = false;
    }
}
