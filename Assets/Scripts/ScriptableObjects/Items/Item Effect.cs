using UnityEngine;

[CreateAssetMenu(fileName = "New Item Effect", menuName = "Inventory/Item Effect")]
public class ItemEffect : ScriptableObject
{
    public enum EffectType
    {
        InstantHeal,
        DamageBoost,
        MoveSpeedBoost,
        HealthRegen
    }

    [Header("Core Settings")]
    public EffectType type;
    public float amount; // Base effect magnitude
    public float duration = 0f; // For timed effects (0 = instant)

    public virtual void ExecuteEffect(GameObject target)
    {
        Debug.Log($"Attempting to apply {type} effect to {target.name}");

        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError($"No PlayerStats found on {target.name}!");
            return;
        }

        switch (type)
        {
            case EffectType.InstantHeal:
                Debug.Log($"Applying instant heal: {amount} HP");
                stats.Heal(amount);
                break;

            case EffectType.DamageBoost:
                Debug.Log($"Applying damage boost: {amount}x for {duration}s");
                stats.ApplyDamageBoost(amount, duration);
                break;

            case EffectType.MoveSpeedBoost:
                Debug.Log($"Applying speed boost: {amount}x for {duration}s");
                stats.ApplyMoveSpeedBoost(amount, duration);
                break;

            case EffectType.HealthRegen:
                Debug.Log($"Starting health regen: {amount}/s for {duration}s");
                stats.StartHealthRegen(amount, duration);
                break;
        }
    }
}