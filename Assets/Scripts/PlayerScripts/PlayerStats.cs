using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    // Cached references to other player-related components on this same GameObject
    private PlayerHealth health;       // Handles HP, damage, and death
    private PlayerMovement movement;   // Handles player movement speed & input
    private PlayerAttack attack;       // Handles attack logic and damage

    [Header("Default Values")]
    // Store the original (baseline) values at game start, so temporary boosts can be reset later
    private float baseMoveSpeed;       // Default movement speed at the start
    private int baseAttackDamage;      // Default attack damage at the start

    // --- Unity lifecycle: called before Start(), good for caching references ---
    void Awake()
    {
        Initialize();        // Cache required component references
        CacheBaseValues();   // Save baseline stats for resets after temporary buffs
    }

    // === Effect Implementation ===

    /// <summary>
    /// Heal the player by a given amount.
    /// Internally, healing is done by passing "negative damage" to PlayerHealth.
    /// Example: Heal(10) will call TakeDamage(-10), effectively increasing HP.
    /// </summary>
    public void Heal(float amount)
    {
        health?.TakeDamage(-Mathf.RoundToInt(amount)); // Negative damage = healing
    }

    /// <summary>
    /// Apply a damage boost to the player’s attacks.
    /// If duration > 0 ? temporary boost using coroutine.
    /// If duration <= 0 ? permanent boost (until changed elsewhere).
    /// </summary>
    public void ApplyDamageBoost(float multiplier, float duration)
    {
        if (duration > 0)
        {
            // Apply the boosted damage temporarily, then revert back after duration
            StartCoroutine(TimedEffectRoutine(
                () => attack.attackDamage = Mathf.RoundToInt(baseAttackDamage * multiplier), // Apply boost
                () => attack.attackDamage = baseAttackDamage,                               // Revert to base
                duration
            ));
        }
        else
        {
            // Apply a permanent change to attack damage
            attack.attackDamage = Mathf.RoundToInt(baseAttackDamage * multiplier);
        }
    }

    /// <summary>
    /// Apply a movement speed boost to the player.
    /// Works the same way as ApplyDamageBoost:
    /// - If duration > 0, temporary effect.
    /// - If duration <= 0, permanent change.
    /// </summary>
    public void ApplyMoveSpeedBoost(float multiplier, float duration)
    {
        if (duration > 0)
        {
            // Temporary boost (reverts after duration ends)
            StartCoroutine(TimedEffectRoutine(
                () => movement.moveSpeed = baseMoveSpeed * multiplier,  // Apply boost
                () => movement.moveSpeed = baseMoveSpeed,              // Revert to base
                duration
            ));
        }
        else
        {
            // Permanent change to move speed
            movement.moveSpeed = baseMoveSpeed * multiplier;
        }
    }

    /// <summary>
    /// Starts a health regeneration effect.
    /// Continuously heals the player every frame for the duration specified.
    /// </summary>
    public void StartHealthRegen(float amountPerSecond, float duration)
    {
        StartCoroutine(HealthRegenRoutine(amountPerSecond, duration));
    }

    // === Coroutines ===

    /// <summary>
    /// General-purpose timed effect routine.
    /// Runs in three steps: 
    /// 1. Apply effect immediately.
    /// 2. Wait for given duration.
    /// 3. Revert effect back to original state.
    /// </summary>
    private IEnumerator TimedEffectRoutine(System.Action applyEffect, System.Action revertEffect, float duration)
    {
        applyEffect();                              // Step 1: Apply the temporary buff
        yield return new WaitForSeconds(duration);  // Step 2: Wait for effect duration
        revertEffect();                             // Step 3: Revert to baseline value
    }

    /// <summary>
    /// Coroutine that regenerates health gradually.
    /// - Heals every frame based on Time.deltaTime until totalDuration expires.
    /// </summary>
    private IEnumerator HealthRegenRoutine(float amountPerSecond, float totalDuration)
    {
        float elapsed = 0f; // Track how long regen has been running

        while (elapsed < totalDuration)
        {
            // Heal proportional to frame time ? smooth regeneration
            Heal(amountPerSecond * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null; // Wait until next frame before continuing
        }
    }

    // === Buff Tracking ===

    /// <summary>
    /// Simple debug log of current player stats.
    /// Later, you can connect this to UI (buff icons, status bars, etc.).
    /// </summary>
    public void DisplayActiveBuffs()
    {
        Debug.Log($"Current Stats - Damage: {attack.attackDamage}, Speed: {movement.moveSpeed}");
    }

    // --- Helpers (extracted from Awake; functionality unchanged) ---

    /// <summary>
    /// Finds and caches references to player-related components.
    /// </summary>
    private void Initialize()
    {
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
    }

    /// <summary>
    /// Saves the original move speed and attack damage values,
    /// so temporary boosts can safely revert after expiration.
    /// </summary>
    private void CacheBaseValues()
    {
        baseMoveSpeed = movement.moveSpeed;
        baseAttackDamage = attack.attackDamage;
    }
}
