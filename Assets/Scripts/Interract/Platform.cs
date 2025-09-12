using UnityEngine;

public class Platform : SwitchableObject
{
    public float giveJumpForce = Config.CHARACTER_JUMP_FORCE; //par défaut mais peut être

    //  différent

    // On gère à l'entrée/sortie du contact du personnage
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
    //     if (player != null)
    //     {
    //         if (switchType == SwitchType.Normal)
    //             player.SetPlatformVisual("Normal");
    //         else if (switchType == SwitchType.Inverted)
    //             player.SetPlatformVisual("Inverted");
    //     }
    // }

    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
    //     if (player != null)
    //     {
    //         player.SetPlatformVisual("None");
    //     }
    // }
}
