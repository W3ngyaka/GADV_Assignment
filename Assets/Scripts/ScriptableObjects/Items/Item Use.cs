using UnityEngine;

public class ItemUse : MonoBehaviour
{
    [Header("Assign in Inspector OR Runtime")]
    [SerializeField] private ItemEffect effect; // Now visible in Inspector

    private bool used = false;

    // Still allow runtime initialization
    public void Initialize(ItemEffect effect)
    {
        this.effect = effect;
    }

    public void Use(GameObject target)
    {
        if (used || effect == null || target == null) return;
        effect.ExecuteEffect(target);
        used = true;
        Destroy(gameObject, 0.5f);
    }
}