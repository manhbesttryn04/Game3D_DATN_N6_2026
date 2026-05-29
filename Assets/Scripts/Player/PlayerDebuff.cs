using System.Collections;
using UnityEngine;

public class PlayerDebuff : MonoBehaviour
{
    // =========================
    // REFERENCES
    // =========================

    public Player player;

    // =========================
    // HIT SYSTEM
    // =========================

    public int hitCount;
    public bool canHit;
    Coroutine resetHitCoroutine;

    // =========================
    // VISUAL
    // =========================

    public SkinnedMeshRenderer[] meshes;

    // =========================
    // TAKE HIT
    // =========================

    public void TakeHit(bool hitFromRight)
    {
        if (!canHit && !player.playerAttack.hasKnock)
        {
            StartCoroutine(HitCoroutine(hitFromRight));
        }
        else if (canHit && !player.playerAttack.hasKnock)
        {
            player.playerAttack.BlockHit();
        }
    }

    IEnumerator HitCoroutine(bool hitFromRight)
    {
        hitCount++;

        player.playerMove.isMove = false;
        player.playerAttack.isAttacking = true;

        if (resetHitCoroutine != null)
        {
            StopCoroutine(resetHitCoroutine);
        }

       // resetHitCoroutine = StartCoroutine(ResetHitCount());

        float hitPush = 0.5f;

        player.playerMove.controller.Move(
            (hitFromRight ? Vector3.left : Vector3.right) * hitPush
        );

        if (hitCount >= 3)
        {
            KnockDown();
            hitCount = 0;
        }
        else
        {
            PlayHitReaction();
        }

        yield return new WaitForSeconds(0.4f);

        player.playerMove.isMove = true;
        player.playerAttack.isAttacking = false;
    }

    // =========================
    // HIT REACTION
    // =========================

    void PlayHitReaction()
    {
        if (player.playerType.isLowWeight)
        {
            int ran = Random.Range(0, 3);

            if (ran == 0)
                player.playerAnimator.playerAni.SetTrigger("Hit");
            else if (ran == 1)
                player.playerAnimator.playerAni.SetTrigger("HitLeft");
            else
                player.playerAnimator.playerAni.SetTrigger("HitRight");
        }
        else
        {
            player.playerAnimator.playerAni.SetTrigger("Hit");
        }
    }

    // =========================
    // KNOCKDOWN
    // =========================

    void KnockDown()
    {
        if (!player.playerAttack.hasKnock)
        {
            player.playerAnimator.playerAni.SetBool("Knock", true);
            player.playerAttack.hasKnock = true;
        }

        canHit = true;

        player.playerMove.isMove = false;
        player.playerAttack.isAttacking = false;
        player.playerAttack.canAttack = false;
        player.playerLook.canRotate = false;

        StartCoroutine(Recover());
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(2f);

        hitCount = 0;

        player.playerAnimator.playerAni.SetBool("Knock", false);

        StartCoroutine(Blink());

        yield return new WaitForSeconds(2f);

        canHit = false;

        player.playerAttack.hasKnock = false;
        player.playerAttack.canAttack = true;
        player.playerMove.isMove = true;
        player.playerLook.canRotate = true;
    }
    IEnumerator ResetHitCount()
    {
        yield return new WaitForSeconds(1f);

        hitCount = 0;
    }

    // =========================
    // BLINK EFFECT
    // =========================

    IEnumerator Blink()
    {
        for (int i = 0; i < 10; i++)
        {
            foreach (Renderer mesh in meshes)
            {
                mesh.enabled = false;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (Renderer mesh in meshes)
            {
                mesh.enabled = true;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}