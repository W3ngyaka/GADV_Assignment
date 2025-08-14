using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    private PlayerHealth health;
    private PlayerMovement movement;
    private PlayerAttack attack;

    [Header("Default Values")]
    private float baseMoveSpeed;
    private int baseAttackDamage;

    void Awake()
    {
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();

        // Cache base values
        baseMoveSpeed = movement.moveSpeed;
        baseAttackDamage = attack.attackDamage;
    }

    // === Effect Implementation ===
    public void Heal(float amount)
    {
        health?.TakeDamage(-Mathf.Abs(amount)); // Negative damage = healing
    }

    public void ApplyDamageBoost(float multiplier, float duration)
    {
        if (duration > 0)
        {
            // Temporary boost
            StartCoroutine(TimedEffectRoutine(
                () => attack.attackDamage = Mathf.RoundToInt(baseAttackDamage * multiplier),
                () => attack.attackDamage = baseAttackDamage,
                duration
            ));
        }
        else
        {
            // Permanent boost
            attack.attackDamage = Mathf.RoundToInt(baseAttackDamage * multiplier);
        }
    }

    public void ApplyMoveSpeedBoost(float multiplier, float duration)
    {
        if (duration > 0)
        {
            StartCoroutine(TimedEffectRoutine(
                () => movement.moveSpeed = baseMoveSpeed * multiplier,
                () => movement.moveSpeed = baseMoveSpeed,
                duration
            ));
        }
        else
        {
            movement.moveSpeed = baseMoveSpeed * multiplier;
        }
    }

    public void StartHealthRegen(float amountPerSecond, float duration)
    {
        StartCoroutine(HealthRegenRoutine(amountPerSecond, duration));
    }

    // === Coroutines ===
    private IEnumerator TimedEffectRoutine(System.Action applyEffect, System.Action revertEffect, float duration)
    {
        applyEffect();
        yield return new WaitForSeconds(duration);
        revertEffect();
    }

    private IEnumerator HealthRegenRoutine(float amountPerSecond, float totalDuration)
    {
        float elapsed = 0f;
        while (elapsed < totalDuration)
        {
            Heal(amountPerSecond * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    // === Buff Tracking ===
    public void DisplayActiveBuffs()
    {
        // You can implement UI feedback here later
        Debug.Log($"Current Stats - Damage: {attack.attackDamage}, Speed: {movement.moveSpeed}");
    }
}