using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerHit : MonoBehaviour
{   
    public Player player;
    public int hitCount;
    public bool canHit;
    public BoxCollider leftHandHitBox;
    public BoxCollider rightHandHitBox;

    void Start()
    {
        leftHandHitBox.enabled = false;
        rightHandHitBox.enabled = false;
    }

    // =====================
    // ATTACK 1
    // =====================

    public void EnableLeftHand()
    {
        rightHandHitBox.enabled = true;
    }

    public void DisableLeftHand()
    {
        rightHandHitBox.enabled = false;
    }

    // =====================
    // ATTACK 2
    // =====================

    public void EnableBothHands()
    {
        leftHandHitBox.enabled = true;
        rightHandHitBox.enabled = true;
    }

    public void DisableBothHands()
    {
        leftHandHitBox.enabled = false;
        rightHandHitBox.enabled = false;
    }

    public void TakeHit(bool hitFromRight)
    {
        if (canHit) { StartCoroutine(HitCoroutine(hitFromRight)); }
        
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
        player.playerAnimator.playerAni.SetBool("Knock", true);

        player.playerMove.isMove = false;
       player.playerAttack.isAttacking = false;

        StartCoroutine(Recover());
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(2f);
        hitCount = 0;
        player.playerMove.isMove = true;
    }


}