using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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
    public float startSpeed;
    public float jumpHeight = 2f;
    public float gravity = -20f;
    float bufferTime = 0.2f;

    float aTimer, dTimer;

    // =========================
    // STATES
    // =========================
    public bool isMove = true;
    public bool isTurning = false;
    public bool isGround;


    // =========================
    // MOVEMENT
    // =========================
    public Vector3 velocity;

    // =========================
    // UNITY
    // =========================
    private void Start()
    {
        startSpeed = speed;
    }

    void Update()
    {

        if (!player.playerAttack.hasKnock)
        {
            if (!player.playerBuff.isDash)
            {
                Move();
                Jump();
            }
            Dash();


        }

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
                move = Vector3.left;
                walkBack = player.playerLook.yRotationRight ? 1f : 0f;
                walk = player.playerLook.yRotationRight ? 0f : 1f;

            }

            if (Input.GetKey(KeyCode.RightArrow) && isMove)
            {
                move = Vector3.right;
                walk = player.playerLook.yRotationRight ? 1f : 0f;
                walkBack = player.playerLook.yRotationRight ? 0f : 1f;

            }
        }

        controller.Move(move * speed * Time.deltaTime);

        player.playerAnimator.playerAni.SetFloat("Walk", walk);
        player.playerAnimator.playerAni.SetFloat("WalkBack", walkBack);
    }
    public void Dash()
    {
        if (!player.playerType.isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.A)) aTimer = bufferTime;
            if (Input.GetKeyDown(KeyCode.D)) dTimer = bufferTime;

            if (Input.GetKey(KeyCode.L))
            {
                if (aTimer > 0)
                {
                    player.playerBuff.DashForInput(0, 7f);
                    aTimer = 0;
                    dTimer = 0;
                }

                if (dTimer > 0)
                {
                    player.playerBuff.DashForInput(1, 7f);
                    aTimer = 0;
                    dTimer = 0;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) aTimer = bufferTime;
            if (Input.GetKeyDown(KeyCode.RightArrow)) dTimer = bufferTime;

            if (aTimer > 0 && Input.GetKey(KeyCode.Keypad3))
            {
                player.playerBuff.DashForInput(0, 7f);
                aTimer = 0;
                dTimer = 0;
            }

            if (dTimer > 0 && Input.GetKey(KeyCode.Keypad3))
            {
                player.playerBuff.DashForInput(1, 7f);
                aTimer = 0;
                dTimer = 0;
            }
        }

        aTimer -= Time.deltaTime;
        dTimer -= Time.deltaTime;
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

        StartCoroutine(StartJump());
    }

    IEnumerator StartJump()
    {
        yield return new WaitForSeconds(0.5f);
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
            speed = startSpeed;
        }

        if (hit.gameObject.CompareTag("Player1") || hit.gameObject.CompareTag("Player2"))
        {
            Vector3 pushBack = transform.position - hit.transform.position;
            pushBack.y = 0;

            controller.Move(pushBack * 0.1f);
        }
    }
}
