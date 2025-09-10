using UnityEngine;

public class Trap : SwitchableObject
{
    // protected override void Awake()
    // {
    //     mySprite = GetComponent<SpriteRenderer>();
    // }

    public override void Activate(bool isActive)
    {
        if (myCollider != null)
            myCollider.enabled = isActive;
        if (mySprite != null)
            mySprite.enabled = isActive;
        // Tu peux aussi désactiver d’autres composants si besoin.
    }
}
