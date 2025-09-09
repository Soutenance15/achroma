using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = Config.CHARACTER_MOVE_SPEED;

    // TODO
    // remettre ca en public après
    private float jumpForce = Config.CHARACTER_JUMP_FORCE;
    private Transform groundCheckFirst;
    private Transform groundCheckSecond;
    private float groundCheckRadius = 0.08f;
    private Transform wallCheck;
    private float wallCheckRadius = 0.08f;
    private LayerMask platformLayerMask;
    private Rigidbody2D rb;

    private bool isGrounded;
    private bool hasJumped;

    private float lastFlipTime = -1f;
    private float flipCooldown = 0.3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheckFirst = transform.Find("GroundCheckFirst");
        groundCheckSecond = transform.Find("GroundCheckSecond");
        if (groundCheckFirst == null || groundCheckSecond == null)
            Debug.LogError(
                "Ajoute deux GameObject enfants 'GroundCheckFirst' et 'GroundCheckSecond' sous le personnage!"
            );

        wallCheck = transform.Find("WallCheck");
        if (wallCheck == null)
            Debug.LogError("Ajoute un GameObject 'WallCheck' sur le côté du perso !");

        platformLayerMask = LayerMask.GetMask("GroundLayer");
        if (!IsGrounded())
        {
            hasJumped = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = IsGrounded();
        bool atEdge = IsOnEdge();
        bool wallIsTouched = IsTouchingWall();

        // Mouvement auto au sol
        if (isGrounded && !atEdge)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            hasJumped = false;
        }
        else if (atEdge && !hasJumped)
        {
            // Saut automatique au bord (pas lors de disparition du sol)
            DoJump();
            hasJumped = true;
        }
        // Si le mur est touché ET que le délai (cooldown)
        //  depuis le dernier flip est passé :
        if (wallIsTouched && Time.time - lastFlipTime > flipCooldown)
        {
            FlipDirection(); // Inverse la direction du perso et le sprite
            lastFlipTime = Time.time; // Sauvegarde l'heure du dernier flip pour le prochain délai
        }
    }

    bool IsGrounded()
    {
        return (
                groundCheckFirst != null
                && Physics2D.OverlapCircle(
                    groundCheckFirst.position,
                    groundCheckRadius,
                    platformLayerMask
                )
            )
            || (
                groundCheckSecond != null
                && Physics2D.OverlapCircle(
                    groundCheckSecond.position,
                    groundCheckRadius,
                    platformLayerMask
                )
            );
    }

    bool IsOnEdge()
    {
        bool left =
            groundCheckFirst != null
            && Physics2D.OverlapCircle(
                groundCheckFirst.position,
                groundCheckRadius,
                platformLayerMask
            );
        bool right =
            groundCheckSecond != null
            && Physics2D.OverlapCircle(
                groundCheckSecond.position,
                groundCheckRadius,
                platformLayerMask
            );
        // Sur le bord si une seule des deux touches
        return (left && !right) || (!left && right);
    }

    bool IsTouchingWall()
    {
        return wallCheck != null
            && Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, platformLayerMask);
    }

    void FlipDirection()
    {
        moveSpeed = -moveSpeed;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        // Déplace aussi le WallCheck de l'autre côté si besoin :
        wallCheck.localPosition = new Vector3(
            -wallCheck.localPosition.x,
            wallCheck.localPosition.y,
            wallCheck.localPosition.z
        );
    }

    void DoJump()
    {
        if (rb != null)
        {
            // La combinaison des 2 fait un mouvement fluide
            // Déplace x sans y
            rb.linearVelocity = new Vector2(2.25f * moveSpeed, 0f);
            // Pousse uniquement sur y
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // TODO
            // Ancienne application du push autant sur x que y
            // pour une poussé diagonéle mais mouvement brute
            // rb.AddForce(new Vector2(1.35f, 1.45f) * jumpForce, ForceMode2D.Impulse);
        }
    }

    // Exemple : gestion des collisions piège
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            RespawnAtLastCheckpoint();
        }
    }

    void RespawnAtLastCheckpoint()
    {
        // TODO
        // à valider l'utilité de cette fonction avec les autres
        // car il y aura peut etre un reset complet du niveau à la place
    }
}
