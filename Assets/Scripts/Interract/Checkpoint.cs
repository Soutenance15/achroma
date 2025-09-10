using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            GameManager.Instance.SaveCheckpoint(transform.position);
            // Todo : effet, identification visuelle ou sonore, etc.
        }
    }
}
