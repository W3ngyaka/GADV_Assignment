using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect", menuName = "Inventory/Item Effect")]
public class ItemEffect : ScriptableObject
{
    // === Types of supported item effects ===
    // Each ItemEffect asset created in the Inspector will be assigned one of these.
    public enum EffectType
    {
        InstantHeal,     // Immediately restore health by a fixed amount
        DamageBoost,     // Temporarily or permanently increase attack damage
        MoveSpeedBoost,  // Temporarily or permanently increase movement speed
        HealthRegen      // Restore health gradually over a period of time
    }

    [Header("Core Settings")]
    public EffectType type;   // Which effect this instance represents
    public float amount;      // Magnitude of the effect (heal amount, multiplier, regen rate, etc.)
    public float duration = 0f; // Duration in seconds (0 = instant / permanent effect)

    /// <summary>
    /// Executes the effect on the given target GameObject.
    /// - Requires the target to have a PlayerStats component.
    /// - Applies the configured effect type with the given amount and duration.
    /// </summary>
    public virtual void ExecuteEffect(GameObject target)
    {
        Debug.Log($"Attempting to apply {type} effect to {target.name}");

        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError($"No PlayerStats found on {target.name}!");
            return;
        }

        // Apply the effect based on its type
        switch (type)
        {
            case EffectType.InstantHeal:
                Debug.Log($"Applying instant heal: {amount} HP");
                stats.Heal(amount); // Heal immediately
                break;

            case EffectType.DamageBoost:
                Debug.Log($"Applying damage boost: {amount}x for {duration}s");
                stats.ApplyDamageBoost(amount, duration); // Temporary or permanent damage multiplier
                break;

            case EffectType.MoveSpeedBoost:
                Debug.Log($"Applying speed boost: {amount}x for {duration}s");
                stats.ApplyMoveSpeedBoost(amount, duration); // Temporary or permanent speed multiplier
                break;

            case EffectType.HealthRegen:
                Debug.Log($"Starting health regen: {amount}/s for {duration}s");
                stats.StartHealthRegen(amount, duration); // Gradual healing over time
                break;
        }
    }
}