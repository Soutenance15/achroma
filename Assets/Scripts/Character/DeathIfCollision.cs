using UnityEngine;

public class DeathIfCollision : MonoBehaviour
{
    // Tag de la plateforme ou surface mortelle (à adapter, genre "Platform")
    public string lethalTag = "Platform";

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Variante : Mort si l'objet touché est une plateforme (ex : le DeathIfCollision touche une plateforme)
        if (collider.gameObject.CompareTag(lethalTag))
        {
            // Cherche le CharacterController2D dans le parent
            CharacterController2D character = GetComponentInParent<CharacterController2D>();
            if (character != null)
            {
                // character.Die();
            }
        }
    }
}
