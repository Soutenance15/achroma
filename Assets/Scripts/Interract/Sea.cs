using UnityEngine;

public class Sea : MonoBehaviour
{

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            CharacterController2D character = collision.GetComponent<CharacterController2D>();
            if (character != null)
            {
                character.Die();
            }
        }
    }
}
