using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    public float comboResetTime = 1f;
    private bool canCombo = false;

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

        // Handle attack input
        if (Input.GetMouseButtonDown(0))
        {
            if (comboStep == 0 || canCombo)
            {
                lastAttackTime = Time.time;
                comboStep++;

                switch (comboStep)
                {
                    case 1:
                        anim.SetTrigger("attack1");
                        break;
                    case 2:
                        anim.SetTrigger("attack2");
                        break;
                    case 3:
                        anim.SetTrigger("attack3");
                        break;
                    case 4:
                        anim.SetTrigger("attack4");
                        break;
                    default:
                        comboStep = 0;
                        break;
                }

                canCombo = false; // Reset combo window until animation allows it again
            }
        }
    }

    // Called by animation event at the right time to allow chaining
    public void EnableCombo()
    {
        canCombo = true;
    }

    public void ResetCombo()
    { 
        comboStep = 0;
        canCombo = false;
    }
}
