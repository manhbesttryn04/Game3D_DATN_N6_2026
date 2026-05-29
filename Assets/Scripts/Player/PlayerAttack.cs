using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // =========================
    // REFERENCES
    // =========================

    public Player player;

    // =========================
    // ATTACK STATES
    // =========================

    public bool isAttacking = false;
    public bool canCombo;
    public bool canAttack = true;
    public bool hasKnock = false;

    // =========================
    // COMBO
    // =========================

    public int hitCount;
    public int attackComboCount = 0;

    Coroutine comboCoroutine;

    // =========================
    // UNITY
    // =========================

    void Update()
    {
        Attack();
        Block();
    }

    // =========================================================
    // ATTACK
    // =========================================================

    public void Attack()
    {
        if (!player.playerType.isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                HandleAttackInput();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                HandleAttackInput();
            }
        }
    }

    void HandleAttackInput()
    {
        if (!canAttack) return;

        if (!isAttacking)
        {
            AttackStart();
            return;
        }

        if (canCombo)
        {
            NextCombo();
        }
    }

    void AttackStart()
    {
        isAttacking = true;

        player.playerMove.isMove = false;

        attackComboCount = 1;

        StartCoroutine(player.playerBuff.DashForward(3f));

        player.playerAnimator.playerAni.SetTrigger("Attack1");


        if (player.playerType.isHighWeight)
        {
            StartComboWindow(1.5f);
        }
        else if (player.playerType.isLowWeight)
        {
            StartComboWindow(0.5f);
        }
    }

    void NextCombo()
    {
        canCombo = false;

        attackComboCount++;

        if (attackComboCount == 2)
        {
            StartCoroutine(player.playerBuff.DashForward(3f));
            player.playerAnimator.playerAni.SetTrigger("Attack2");

            if (player.playerType.isHighWeight)
            {
                StartComboWindow(2f);
            }
            else if (player.playerType.isLowWeight)
            {
                StartComboWindow(0.5f);
            }
        }
        else if (attackComboCount == 3)
        {
            StartCoroutine(player.playerBuff.DashForward(3f));
            player.playerAnimator.playerAni.SetTrigger("Attack3");

            if (player.playerType.isHighWeight)
            {
                Invoke("EndComboFinisher", 2f);
            }
            else Invoke("EndComboFinisher", 0f);



        }
    }

    // =========================================================
    // COMBO
    // =========================================================

    void StartComboWindow(float time)
    {
        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
        }

        comboCoroutine =
            StartCoroutine(ComboRoutine(time));
    }

    IEnumerator ComboRoutine(float time)
    {
        yield return new WaitForSeconds(time);

        canCombo = true;

        yield return new WaitForSeconds(0.2f);

        canCombo = false;

        if (attackComboCount == 1 ||
            attackComboCount == 2)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        StopAllCoroutines();
        isAttacking = false;

        player.playerMove.isMove = true;

        canCombo = false;

        attackComboCount = 0;
    }

    void EndComboFinisher()
    {
        isAttacking = false;

        player.playerMove.isMove = true;

        attackComboCount = 0;

        StartCoroutine(ComboCooldown());
    }

    IEnumerator ComboCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(1f);

        canAttack = true;
    }

    // =========================================================
    // BLOCK
    // =========================================================

    public void Block()
    {
        if (!player.playerType.isPlayer2)
        {
            if (Input.GetKey(KeyCode.S))
            {
                HandelBlock(true);
            }
            else
            {
                HandelBlock(false);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                HandelBlock(true);
            }
            else
            {
                HandelBlock(false);
            }
        }
    }

    void HandelBlock(bool value)
    {
        player.playerAnimator.playerAni.SetBool("Block", value);

        player.playerDebuff.canHit = value;
    }

    public void BlockHit()
    {
        if (player.playerType.isLowWeight)
        {
            player.playerAnimator.playerAni.SetTrigger("BlockHit");
            StartCoroutine(player.playerBuff.DashBack(0.5f));
        }
        else StartCoroutine(player.playerBuff.DashBack(0.5f));
    }
}