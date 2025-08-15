using UnityEngine;

public class ItemUse : MonoBehaviour
{
    [Header("Assign in Inspector OR Runtime")]
    [SerializeField] private ItemEffect effect; // Reference to the effect this item triggers

    private bool used = false; // Prevents multiple uses of the same item

    /// <summary>
    /// Initialize the item at runtime with a specific effect.
    /// Useful when creating items dynamically in code.
    /// </summary>
    public void Initialize(ItemEffect effect)
    {
        this.effect = effect;
    }

    /// <summary>
    /// Use this item on the given target.
    /// - Executes its assigned ItemEffect on the target.
    /// - Prevents reuse once consumed.
    /// - Destroys the item object shortly after use.
    /// </summary>
    public void Use(GameObject target)
    {
        // Safety checks: already used, no effect assigned, or no target
        if (used || effect == null || target == null) return;

        // Apply the effect to the target (e.g., heal, buff, damage)
        effect.ExecuteEffect(target);

        // Mark as consumed
        used = true;

        // Destroy this item object shortly after use (0.5s delay allows animations/SFX)
        Destroy(gameObject, 0.5f);
    }
}
