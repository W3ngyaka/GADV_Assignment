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
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats == null) return;

        switch (type)
        {
            case EffectType.InstantHeal:
                stats.Heal(amount);
                break;

            case EffectType.DamageBoost:
                stats.ApplyDamageBoost(amount, duration);
                break;

            case EffectType.MoveSpeedBoost:
                stats.ApplyMoveSpeedBoost(amount, duration);
                break;

            case EffectType.HealthRegen:
                stats.StartHealthRegen(amount, duration);
                break;
        }
    }
}