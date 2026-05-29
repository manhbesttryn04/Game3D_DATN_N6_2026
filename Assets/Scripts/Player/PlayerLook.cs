using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerLook : MonoBehaviour
{
    public Player player;
    public bool yRotationRight = true;
    public float lockXPosition;
    public bool canRotate;
    void Start()
    {
        lockXPosition = transform.position.z;
    }
    void Update()
    {
        if (canRotate)
        {
            LookToPlayer();
            RotatePlayer();
        }
    }
    void LateUpdate()
    {
        LockPosition();
    }

    public void RotatePlayer()
    {
        transform.rotation = yRotationRight ?
            Quaternion.Euler(0, 90, 0) :
            Quaternion.Euler(0, -90, 0);
    }

    public void LookToPlayer()
    {
        if (!player.playerMove.controller.isGrounded || player.playerMove.isTurning) return;

        GameObject target = player.playerType.isPlayer2 ?
            GameObject.FindGameObjectWithTag("Player1") :
            GameObject.FindGameObjectWithTag("Player2");

        if (target == null) return;

        bool shouldFaceRight = target.transform.position.x > transform.position.x;

        if (shouldFaceRight != yRotationRight)
            StartCoroutine(TurnCharacter(shouldFaceRight));
    }

    IEnumerator TurnCharacter(bool lookRight)
    {
        player.playerMove.isTurning = true;
        yield return new WaitForSeconds(0.2f);

        yRotationRight = lookRight;

        player.playerMove.isTurning = false;
    }

    // =========================================================
    // UTIL
    // =========================================================
    public void LockPosition()
    {
        Vector3 pos = transform.position;
        pos.z = lockXPosition;
        transform.position = pos;
    }
}
