using UnityEngine;

public abstract class SwitchableObject : MonoBehaviour
{
    public enum SwitchType
    {
        Normal,
        Inverted,
    }

    public bool myColorIsBlack = false;

    public SwitchType switchType = SwitchType.Normal;

    public Color colorForBlack = Config.COLOR_FOR_BLACK;
    public Color colorForOther = Config.COLOR_FOR_OTHER;

    protected Collider2D myCollider;
    protected SpriteRenderer mySprite;

    protected virtual void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        mySprite = GetComponent<SpriteRenderer>();
        UpdateOnSwitch(WorldSwitchManager.isBlackWorld);
    }

    protected virtual void OnEnable()
    {
        WorldSwitchManager.OnWorldSwitch += UpdateOnSwitch;
    }

    protected virtual void OnDisable()
    {
        WorldSwitchManager.OnWorldSwitch -= UpdateOnSwitch;
    }

    // Ici toute la gestion centrale
    public virtual void UpdateOnSwitch(bool isBlackWorld)
    {
        bool isActive;
        if (switchType == SwitchType.Normal)
            isActive = (myColorIsBlack == isBlackWorld);
        else
            isActive = (myColorIsBlack != isBlackWorld);

        ActivateCollider(isActive);
        UpdateColorFor(isActive);
    }

    public virtual void ActivateCollider(bool isActive)
    {
        if (myCollider != null)
            myCollider.enabled = isActive;
    }

    public virtual void UpdateColorFor(bool isActive)
    {
        if (mySprite != null)
            mySprite.color = isActive ? colorForBlack : colorForOther;
    }

    // Ex : à spécialiser dans Trap/Platform si besoin
    protected virtual void OnCollisionEnter2D(Collision2D other) { }
}
