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

    void OnCollisionEnter2D(Collision2D other)
    {
        CharacterController2D character = other.gameObject.GetComponent<CharacterController2D>();
        if (character != null)
        {
            character.Die();
        }
    }
}
