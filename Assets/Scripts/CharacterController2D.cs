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
    private LayerMask plaformNormalLayer;
    private LayerMask invertedPlaformLayer;
    private Rigidbody2D rb;

    private bool isGroundedNormal;
    private bool isGroundedInverted;
    private bool hasJumped;

    // Pour éviter les flip répeter on va mettre un colldown avant de refaire un calcul
    private float lastFlipTime = -1f;
    private float flipCooldown = 0.3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheckFirst = transform.Find("GroundCheckFirst");
        groundCheckSecond = transform.Find("GroundCheckSecond");
        if (groundCheckFirst == null || groundCheckSecond == null)
            Debug.LogWarning(
                "Ajoute deux GameObject enfants 'GroundCheckFirst' et 'GroundCheckSecond' sous le personnage!"
            );

        wallCheck = transform.Find("WallCheck");
        if (wallCheck == null)
            Debug.LogWarning("Ajoute un GameObject 'WallCheck' sur le côté du perso !");

        plaformNormalLayer = LayerMask.GetMask("PlatformNormalLayer");
        invertedPlaformLayer = LayerMask.GetMask("InvertedPlaformLayer");

        if (!IsGrounded(plaformNormalLayer) && !IsGrounded(invertedPlaformLayer))
        {
            hasJumped = true;
        }
    }

    void FixedUpdate()
    {
        isGroundedNormal = IsGrounded(plaformNormalLayer);
        bool atEdgeNormal = IsOnEdge(plaformNormalLayer);

        isGroundedInverted = IsGrounded(invertedPlaformLayer);
        bool atEdgeInverted = IsOnEdge(invertedPlaformLayer);

        bool wallNormalIsTouched = IsTouchingWall(plaformNormalLayer);
        bool wallInvertedTouched = IsTouchingWall(invertedPlaformLayer);

        // Mouvement auto sur toute plateforme tangible, sauf au bord
        if ((isGroundedNormal || isGroundedInverted) && !(atEdgeNormal || atEdgeInverted))
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            hasJumped = false;
        }
        // Saut uniquement si au bord, sans superposition
        else if (
            (
                (atEdgeNormal && isGroundedNormal && !isGroundedInverted)
                || (atEdgeInverted && isGroundedInverted && !isGroundedNormal)
            ) && !hasJumped
        )
        {
            DoJump();
            hasJumped = true;
        }

        // Flip au mur avec cooldown
        if ((wallNormalIsTouched || wallInvertedTouched) && Time.time - lastFlipTime > flipCooldown)
        {
            FlipDirection();
            lastFlipTime = Time.time;
        }
    }

    bool IsGrounded(LayerMask layer)
    {
        return (
                groundCheckFirst != null
                && Physics2D.OverlapCircle(groundCheckFirst.position, groundCheckRadius, layer)
            )
            || (
                groundCheckSecond != null
                && Physics2D.OverlapCircle(groundCheckSecond.position, groundCheckRadius, layer)
            );
    }

    bool IsOnEdge(LayerMask layer)
    {
        Collider2D firstCollider2D = null;
        Collider2D secondCollider2D = null;

        if (groundCheckFirst != null)
            firstCollider2D = Physics2D.OverlapCircle(
                groundCheckFirst.position,
                groundCheckRadius,
                layer
            );

        if (groundCheckSecond != null)
            secondCollider2D = Physics2D.OverlapCircle(
                groundCheckSecond.position,
                groundCheckRadius,
                layer
            );

        bool firstSolide = firstCollider2D != null && !firstCollider2D.isTrigger;
        bool secondSolide = secondCollider2D != null && !secondCollider2D.isTrigger;

        // Si un des collider touche mais pas l'autre
        // ca signifie que l'un est sur un platform
        // pandant que l'autre est dans le vide
        // donc on est sur un bord

        return (firstSolide && !secondSolide) || (!firstSolide && secondSolide);
    }

    bool IsTouchingWall(LayerMask layer)
    {
        if (wallCheck == null)
            return false;

        // recuperation du collider avec lequel wallcheck interragit
        Collider2D collider2D = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, layer);
        // Retourne vrai SEULEMENT si un collider a été détecté
        // ET qu'il n'est pas un trigger car quand on traverse wall en trigger
        // On considère que on ne le touche pas
        return collider2D != null && !collider2D.isTrigger;
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
            rb.linearVelocity = new Vector2(Config.COEFF_MOVE_SPEED_FOR_JUMP * moveSpeed, 0f);
            // Pousse uniquement sur y
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // TODO
            // Ancienne application du push autant sur x que y
            // pour une poussé diagonéle mais mouvement brute
            // rb.AddForce(new Vector2(1.35f, 1.45f) * jumpForce, ForceMode2D.Impulse);
        }
    }

    // Exemple : gestion des collisions piège
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("CheckPoint"))
    //     {
    //         saveStae();
    //
    //     }
    // }

    // void RespawnAtLastCheckpoint()
    // {
    //     // TODO
    //     // à valider l'utilité de cette fonction avec les autres
    //     // car il y aura peut etre un reset complet du niveau à la place
    // }
}
