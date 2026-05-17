using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // =========================
    // COMPONENTS
    // =========================
    public CharacterController controller;

    // =========================
    // PLAYER SETTINGS
    // =========================
    public Player player;
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    // =========================
    // STATES
    // =========================
    public bool isMove = true;
    public bool isTurning = false;
    public bool isGround;


    // =========================
    // MOVEMENT
    // =========================
    Vector3 velocity;

    // =========================
    // UNITY
    // =========================
   

    void Update()
    {


        Move();
        Jump();
        ApplyGravity();
    }

   

    // =========================================================
    // MOVEMENT
    // =========================================================
    void Move()
    {
        float walk = 0f;
        float walkBack = 0f;
        Vector3 move = Vector3.zero;

        if (!player.playerType.isPlayer2)
        {
            if (Input.GetKey(KeyCode.A) && isMove)
            {
                move = Vector3.left;
                walkBack = player.playerLook.yRotationRight ? 1f : 0f;
                walk = player.playerLook.yRotationRight ? 0f : 1f;
            }

            if (Input.GetKey(KeyCode.D) && isMove)
            {
                move = Vector3.right;
                walk = player.playerLook.yRotationRight ? 1f : 0f;
                walkBack = player.playerLook.yRotationRight ? 0f : 1f;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow) && isMove)
            {
                move = player.playerLook.yRotationRight ? Vector3.left : Vector3.right;
                walkBack = player.playerLook.yRotationRight ? 1f : 0f;
                walk = player.playerLook.yRotationRight ? 0f : 1f;
            }

            if (Input.GetKey(KeyCode.RightArrow) && isMove)
            {
                move = player.playerLook.yRotationRight ? Vector3.right : Vector3.left;
                walk = player.playerLook.yRotationRight ? 1f : 0f;
                walkBack = player.playerLook.yRotationRight ? 0f : 1f;
            }
        }

        controller.Move(move * speed * Time.deltaTime);

        player.playerAnimator.playerAni.SetFloat("Walk", walk);
        player.playerAnimator.playerAni.SetFloat("WalkBack", walkBack);
    }

    // =========================================================
    // JUMP
    // =========================================================
    void Jump()
    {
        if (!isGround) return;

        if (!player.playerType.isPlayer2 && Input.GetKeyDown(KeyCode.W))
        {
            JumpAction();
            isGround = false;
        }

        if (player.playerType.isPlayer2 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            JumpAction();
            isGround = false;
        }
    }

    void JumpAction()
    {
        player.playerAnimator.playerAni.SetTrigger("Jump");

        speed = 1f;

        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        float dir = player.playerLook.yRotationRight ? 1f : -1f;
        velocity.x = dir * 10f;
    }

    void ApplyGravity()
    {
        if (isGround && velocity.y < 0)
            velocity.y = -2f;

        if (!controller.isGrounded)
            velocity.x *= 0.98f;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }


    // =========================================================
    // COLLISION
    // =========================================================
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            speed = 5f;
        }

        if (hit.gameObject.CompareTag("Player1") || hit.gameObject.CompareTag("Player2"))
        {
            Vector3 pushBack = transform.position - hit.transform.position;
            pushBack.y = 0;

            controller.Move(pushBack * 0.1f);
        }
    }
}