using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    public float comboResetTime = 1f;
    private bool canCombo = false;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.7f;
    public int attackDamage = 1;
    public LayerMask enemyLayer;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
            canCombo = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (comboStep == 0 || canCombo)
            {
                lastAttackTime = Time.time;
                comboStep++;

                switch (comboStep)
                {
                    case 1: anim.SetTrigger("attack1"); break;
                    case 2: anim.SetTrigger("attack2"); break;
                    case 3: anim.SetTrigger("attack3"); break;
                    case 4: anim.SetTrigger("attack4"); break;
                    default: comboStep = 0; break;
                }

                canCombo = false;
            }
        }
    }

    public void EnableCombo() => canCombo = true;
    public void ResetCombo() { comboStep = 0; canCombo = false; }

    // 🔥 Called by Animation Event
    public void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
