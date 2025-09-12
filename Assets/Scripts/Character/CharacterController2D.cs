using System.Collections;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private GameObject spriteNormalChild;
    private GameObject spriteInvertedChild;
    private SpriteRenderer mainSprite;

    public float moveSpeed = Config.CHARACTER_MOVE_SPEED;
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

    public Vector2 defaultSpawnPosition;

    // Pour éviter les flip répeter on va mettre un colldown avant de refaire un calcul
    private float lastFlipTime = -1f;
    private float flipCooldown = 0.3f;

    private Collider2D edgeCollider;

    protected void OnEnable()
    {
        WorldSwitchManager.OnWorldSwitch += UpdateVisualOnWorldSwitch;
    }

    protected void OnDisable()
    {
        WorldSwitchManager.OnWorldSwitch -= UpdateVisualOnWorldSwitch;
    }

    private void UpdateVisualOnWorldSwitch(bool? isNormalWorld)
    {
        // // Détecte la plateforme active (code OverlapCircle avec LayerMask)
        // Collider2D platCol = Physics2D.OverlapCircle(
        //     (Vector2)transform.position + Vector2.down * 0.14f,
        //     0.18f,
        //     LayerMask.GetMask("PlatformNormalLayer", "InvertedPlaformLayer")
        // );
        // SwitchableObject.SwitchType? detectedType = null;
        // if (platCol != null)
        // {
        //     var sw = platCol.GetComponent<SwitchableObject>();
        //     if (sw != null)
        //         detectedType = sw.switchType;
        // }

        // Logique de visualisation combinée : état du monde + type plateforme
        if (!isNormalWorld.HasValue)
        {
            // Monde neutre, tout désactive/neutre
            mainSprite.color = Color.black;
            if (spriteNormalChild)
                spriteNormalChild.SetActive(false);
            if (spriteInvertedChild)
                spriteInvertedChild.SetActive(false);
            return;
        }
        if (isNormalWorld.Value)
        {
            mainSprite.color = Color.black;
            if (spriteNormalChild)
                spriteNormalChild.SetActive(true);
            if (spriteInvertedChild)
                spriteInvertedChild.SetActive(false);
        }
        else if (!isNormalWorld.Value)
        {
            mainSprite.color = Color.white;
            if (spriteNormalChild)
                spriteNormalChild.SetActive(false);
            if (spriteInvertedChild)
                spriteInvertedChild.SetActive(true);
        }
        else
        {
            // Plateforme qui ne correspond pas au monde actif : visuellement neutre
            mainSprite.color = Color.black;
            if (spriteNormalChild)
                spriteNormalChild.SetActive(false);
            if (spriteInvertedChild)
                spriteInvertedChild.SetActive(false);
        }
    }

    void Awake()
    {
        mainSprite = GetComponent<SpriteRenderer>();
        spriteNormalChild = transform.Find("Normal")?.gameObject;
        spriteInvertedChild = transform.Find("Inverted")?.gameObject;
        // Désactive tout au départ
        if (spriteNormalChild)
            spriteNormalChild.SetActive(false);
        if (spriteInvertedChild)
            spriteInvertedChild.SetActive(false);

        defaultSpawnPosition = transform.position;
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
        Collider2D colliderPlatformNormal = AtThisEdge(plaformNormalLayer);
        bool wallNormalIsTouched = IsTouchingWall(plaformNormalLayer);

        isGroundedInverted = IsGrounded(invertedPlaformLayer);
        Collider2D colliderInvertedPlatform = AtThisEdge(invertedPlaformLayer);
        bool wallInvertedTouched = IsTouchingWall(invertedPlaformLayer);

        // Mouvement auto sur toute plateforme tangible, sauf au bord
        if (
            (isGroundedNormal || isGroundedInverted)
            && !(colliderPlatformNormal != null || colliderInvertedPlatform != null)
        )
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            hasJumped = false;
        }
        // Saut uniquement si au bord, sans superposition
        else if (
            (
                (colliderPlatformNormal != null && isGroundedNormal && !isGroundedInverted)
                || (colliderInvertedPlatform != null && isGroundedInverted && !isGroundedNormal)
            ) && !hasJumped
        )
        {
            Platform platform = null;
            if (colliderPlatformNormal != null)
            {
                platform = colliderPlatformNormal.gameObject.GetComponent<Platform>();
            }
            else if (colliderInvertedPlatform != null)
            {
                platform = colliderInvertedPlatform.gameObject.GetComponent<Platform>();
            }
            if (platform != null)
            {
                DoJump(platform);
            }
            hasJumped = true;
        }

        // Flip au mur avec cooldown
        if ((wallNormalIsTouched || wallInvertedTouched) && Time.time - lastFlipTime > flipCooldown)
        {
            FlipDirection();
            lastFlipTime = Time.time;
        }
    }

    public void Die()
    {
        GameManager.Instance.RespawnCharacter();

        // Replace le personnage
        // GameManager.Instance.SetState(GameManager.GameState.Paused); // Met en pause et affiche l'UI
        // StartCoroutine(RespawnDelayed(1f));
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

    Collider2D AtThisEdge(LayerMask layer)
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

        if (firstSolide && !secondSolide)
        {
            edgeCollider = firstCollider2D;
            return firstCollider2D;
        }
        if (!firstSolide && secondSolide)
        {
            edgeCollider = secondCollider2D;
            return secondCollider2D;
        }
        return null;
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
        Debug.Log("FlipDirection");
        moveSpeed = -moveSpeed;
        Vector3 scale = transform.localScale;
        // scale.x *= -1;
        transform.localScale = scale;
        // Déplace aussi le WallCheck de l'autre côté si besoin :
        wallCheck.localPosition = new Vector3(
            -wallCheck.localPosition.x,
            wallCheck.localPosition.y,
            wallCheck.localPosition.z
        );
    }

    void DoJump(Platform platform)
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(Config.COEFF_MOVE_SPEED_FOR_JUMP * moveSpeed, 0f);
            // Pousse uniquement sur y
            rb.AddForce(Vector2.up * platform.giveJumpForce, ForceMode2D.Impulse);
        }
    }

    // public void SetPlatformVisual(string type)
    // {
    //     // Affiche/cache selon la demande
    //     if (type == "Normal")
    //     {
    //         if (spriteNormalChild)
    //             spriteNormalChild.SetActive(true);
    //         if (spriteInvertedChild)
    //             spriteInvertedChild.SetActive(false);
    //         mainSprite.color = Color.black;
    //     }
    //     else if (type == "Inverted")
    //     {
    //         if (spriteNormalChild)
    //             spriteNormalChild.SetActive(false);
    //         if (spriteInvertedChild)
    //             spriteInvertedChild.SetActive(true);
    //         mainSprite.color = Color.white;
    //     }
    //     else // "None" ou toute autre valeur
    //     {
    //         if (spriteNormalChild)
    //             spriteNormalChild.SetActive(false);
    //         if (spriteInvertedChild)
    //             spriteInvertedChild.SetActive(false);
    //         mainSprite.color = Color.black; // ou une couleur neutre
    //     }
    // }
}
