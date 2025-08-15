using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private int comboStep = 0;                 // Current combo index (1..4)
    private float lastAttackTime = 0f;         // Time of the last attack press
    public float comboResetTime = 1f;          // Time window to continue the combo
    private bool canCombo = false;             // Set by animation event to allow next step
    private PlayerInventory playerInventory;   // Present in original; left unchanged even if unused

    [Header("Attack Settings")]
    public Transform attackPoint;              // Position to check for enemy hits
    public float attackRange = 0.7f;           // Radius for hit detection
    public int attackDamage = 1;               // Damage dealt per hit
    public LayerMask enemyLayer;               // Which layers count as enemies

    // --- Unity lifecycle: single line; kept inline per your rule ---
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // --- Unity lifecycle: delegate multi-line logic to a helper ---
    void Update()
    {
        HandleUpdate(); // combo timing, input gating, and trigger selection
    }

    // Enable next combo step (invoked via animation event)
    public void EnableCombo() => canCombo = true;

    // Reset combo state explicitly
    public void ResetCombo() { comboStep = 0; canCombo = false; }

    // Called by Animation Event: apply damage to enemies in range
    public void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    // Editor gizmo to visualize the attack range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // --- Helper (extracted from Update; functionality unchanged) ---

    // Handles combo timing window, input gating (UI check), and triggering the correct attack
    private void HandleUpdate()
    {
        // If the time since last attack exceeds the window, reset the combo
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
            canCombo = false;
        }

        // Left mouse button pressed AND no menus open (prevents attacking in UI)
        if (Input.GetMouseButtonDown(0) && !UIManager.Instance.IsAnyMenuOpen())
        {
            // Either starting a new combo (step 0) or continuing when animation allowed it
            if (comboStep == 0 || canCombo)
            {
                lastAttackTime = Time.time; // refresh combo window
                comboStep++;

                // Trigger the appropriate animation for the current step
                switch (comboStep)
                {
                    case 1: anim.SetTrigger("attack1"); break;
                    case 2: anim.SetTrigger("attack2"); break;
                    case 3: anim.SetTrigger("attack3"); break;
                    case 4: anim.SetTrigger("attack4"); break;
                    default: comboStep = 0; break; // safety: reset if somehow exceeded
                }

                // Next step is only allowed after animation event calls EnableCombo()
                canCombo = false;
            }
        }
    }
}
