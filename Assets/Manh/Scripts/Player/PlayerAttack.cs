using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAttack : MonoBehaviour
{
    public Player player;
    public bool isAttacking = false;
    public int hitCount;
    public int attackComboCount = 0;
    public bool canCombo;
    public bool canAttack = true;
    public bool hasKnock = false;
    Coroutine comboCoroutine;


    public void Update()
    {
        Attack();
        Block();
    }
    public void Attack()
    {
        if (!player.playerType.isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.J))
                HandleAttackInput();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                HandleAttackInput();
        }
    }
    public void Block()
    {
        if (!player.playerType.isPlayer2)
        {
            if (Input.GetKey(KeyCode.S))
            {
                HandelBlock(true);
            } else HandelBlock(false);
        }
        else
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                HandelBlock(true);
            }else HandelBlock(false);
        }
    }
    public void BlockHit()
    {
        player.playerAnimator.playerAni.SetTrigger("BlockHit");
    }
    void HandelBlock(bool value)
    {
        player.playerAnimator.playerAni.SetBool("Block", value);
        player.playerDebuff.canHit = value;
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

        player.playerAnimator.playerAni.SetTrigger("Attack1");

       player.playerBuff.DashForward(0.4f);

        StartComboWindow();
    }

    void NextCombo()
    {
        canCombo = false;
        attackComboCount++;

        if (attackComboCount == 2)
        {
            player.playerAnimator.playerAni.SetTrigger("Attack2");
            StartComboWindow();
        }
        else if (attackComboCount == 3)
        {
            player.playerAnimator.playerAni.SetTrigger("Attack3");
            EndComboFinisher();
        }
    }

    void StartComboWindow()
    {
        if (comboCoroutine != null)
            StopCoroutine(comboCoroutine);

        comboCoroutine = StartCoroutine(ComboRoutine());
    }

    IEnumerator ComboRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        canCombo = true;

        yield return new WaitForSeconds(0.2f);
        canCombo = false;

        if (attackComboCount == 1 || attackComboCount == 2)
            EndAttack();
    }

    void EndAttack()
    {
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

   
}
