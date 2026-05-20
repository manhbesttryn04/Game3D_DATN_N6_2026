using System.Collections;
using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    // =========================
    // REFERENCES
    // =========================

    public Player player;

    // =========================
    // DASH STATES
    // =========================

    public bool isDash;
    public bool canDash = true;

    // =========================================================
    // DASH
    // =========================================================

   public  IEnumerator DashForward(float speed)
    {
        float timer = 0f;
        float duration = 0.2f;

        Vector3 dir =
            player.playerLook.yRotationRight ?
            Vector3.right :
            Vector3.left;

        while (timer < duration)
        {
            player.playerMove.controller.Move(
                dir * speed * Time.deltaTime
            );

            timer += Time.deltaTime;

            yield return null;
        }
    }
    public IEnumerator DashBack(float speed)
    {
        float timer = 0f;
        float duration = 0.2f;

        Vector3 dir =
            player.playerLook.yRotationRight ?
            Vector3.left :
            Vector3.right;

        while (timer < duration)
        {
            player.playerMove.controller.Move(
                dir * speed * Time.deltaTime
            );

            timer += Time.deltaTime;

            yield return null;
        }
    }

    public void DashForInput(float i, float spacing)
    {
        Vector3 dir;

        if (i == 0)
        {
            dir = Vector3.left;
        }
        else
        {
            dir = Vector3.right;
        }

        if (!isDash &&
            canDash &&
            player.playerMove.isGround)
        {
            if (player.playerType.isHighWeight)
            {
                player.playerMove.velocity.y = 4f;
            }
            else player.playerMove.velocity.y = 2f;
            StartCoroutine(DashRoutine(dir,spacing));

           player.playerAnimator.playerAni.SetFloat("Walk", 0);
            player.playerAnimator.playerAni.SetFloat("WalkBack", 0);

            isDash = true;
            canDash = false;
        }

        //StartCoroutine(ResetDash());
    }
    IEnumerator DashRoutine(Vector3 dir, float spacing)
    {
        float timer = 0f;
        float duration = 0.2f;

        while (timer < duration)
        {
            player.playerMove.controller.Move(
                dir * spacing * Time.deltaTime
            );

            timer += Time.deltaTime;

            yield return null;
        }

        StartCoroutine(ResetDash());
    }
    // =========================================================
    // RESET
    // =========================================================

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(0.3f);

        isDash = false;

        yield return new WaitForSeconds(0.2f);

        canDash = true;
    }
}