using UnityEngine;

public abstract class SwitchableObject : MonoBehaviour
{
    public enum SwitchType
    {
        Normal,
        Inverted,
    }

    public bool myColorIsNormal = false;

    public SwitchType switchType = SwitchType.Normal;

    private Color colorForNormal = Config.COLOR_FOR_NORMAL;
    private Color colorForOther = Config.COLOR_FOR_INVERTED;

    protected Collider2D myCollider;
    protected SpriteRenderer mySprite;

    protected virtual void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        mySprite = GetComponent<SpriteRenderer>();
        UpdateOnSwitch(WorldSwitchManager.isNormalWorld);
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
    public virtual void UpdateOnSwitch(bool? isNormalWorld)
    {
        bool isActive = false;

        // Si aucun monde, désactive tout
        if (!isNormalWorld.HasValue)
        {
            Activate(false); // Désactive tous
            UpdateColorFor(false); // Optionnel : couleur neutre
            return;
        }

        // Sinon, logique normale
        if (switchType == SwitchType.Normal)
            isActive = (myColorIsNormal == isNormalWorld.Value);
        else
            isActive = (myColorIsNormal != isNormalWorld.Value);

        Activate(isActive);
        UpdateColorFor(isActive);
    }

    public virtual void Activate(bool isActive)
    {
        if (myCollider != null)
            myCollider.isTrigger = GetIsTrigger(isActive);
    }

    private bool GetIsTrigger(bool isActive)
    {
        // /!\ si il est actif allors on doit descativer le trigger
        if (isActive)
            return false;
        // si il est inactif, le trigger est vrai pour passer à travers
        return true;
    }

    public virtual void UpdateColorFor(bool isActive)
    {
        if (mySprite != null)
        {
            mySprite.color = isActive ? colorForNormal : colorForOther;
        }
    }
}
