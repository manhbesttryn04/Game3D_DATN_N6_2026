using System.Collections;
using UnityEngine;

public class PlayerDebuff : MonoBehaviour
{
    public Player player;
    public int hitCount;
    public bool canHit;
    public SkinnedMeshRenderer[] meshes;

    public void TakeHit(bool hitFromRight)
    {
        if (!canHit && !player.playerAttack.hasKnock)
        {
            StartCoroutine(HitCoroutine(hitFromRight));
        }
        else if (canHit && !player.playerAttack.hasKnock) player.playerAttack.BlockHit();

    }


    IEnumerator HitCoroutine(bool hitFromRight)
    {
        hitCount++;

        player.playerMove.isMove = false;
        player.playerAttack.isAttacking = true;

        float hitPush = 0.5f;

        player.playerMove.controller.Move((hitFromRight ? Vector3.right : Vector3.left) * hitPush);

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

    void PlayHitReaction()
    {
        int ran = Random.Range(0, 3);

        if (ran == 0) player.playerAnimator.playerAni.SetTrigger("Hit");
        else if (ran == 1) player.playerAnimator.playerAni.SetTrigger("HitLeft");
        else player.playerAnimator.playerAni.SetTrigger("HitRight");
    }

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
