using System.Collections;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    // =========================
    // COMPONENTS
    // =========================

    public CharacterController controller;
    public Animator animator;

    // =========================
    // PLAYER SETTINGS
    // =========================

    public bool isPlayer2 = false;

    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    // =========================
    // STATES
    // =========================

    public bool isAttacking = false;
    public bool isMove = true;
    public bool isTurning = false;
    public bool yRotationRight = true;
    public bool isGround;

    public float lockXPosition;
    public int attackComboCount = 0;
    bool canCombo;
    Coroutine comboCoroutine;
    bool isComboFinisher;
    bool canAttack = true;

    // =========================
    // MOVEMENT
    // =========================

    Vector3 velocity;

    // =========================
    // UNITY EVENTS
    // =========================

    void Start()
    {
        lockXPosition = transform.position.z;
    }

    void Update()
    {
        SetPlayerType();
     

        LookToPlayer();
        RotatePlayer();

        Move();
        Jump();
        ApplyGravity();

        Attack();
       

    }

    // =========================
    // MOVE
    // =========================
    void LateUpdate()
    {
        LockPosition();
    }
    void Move()
    {
        float walk = 0f;
        float walkBack = 0f;

        Vector3 move = Vector3.zero;

        // PLAYER 1
        if (!isPlayer2)
        {
            if (Input.GetKey(KeyCode.A) && isMove)
            {
                if (yRotationRight)
                {
                    move = Vector3.left;
                    walkBack = 1f;
                }
                else
                {
                    move = Vector3.left;
                    walk = 1f;
                }
            }

            if (Input.GetKey(KeyCode.D) && isMove)
            {
                if (yRotationRight)
                {
                    move = Vector3.right;
                    walk = 1f;
                }
                else
                {
                    move = Vector3.right;
                    walkBack = 1f;
                }
            }
        }

        // PLAYER 2
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow) && isMove)
            {
                if (yRotationRight)
                {
                    move = Vector3.left;
                    walkBack = 1f;
                }
                else
                {
                    move = Vector3.right;
                    walk = 1f;
                }
            }

            if (Input.GetKey(KeyCode.RightArrow) && isMove)
            {
                if (yRotationRight)
                {
                    move = Vector3.right;
                    walk = 1f;
                }
                else
                {
                    move = Vector3.left;
                    walkBack = 1f;
                }
            }
        }

        controller.Move(move * speed * Time.deltaTime);

        animator.SetFloat("Walk", walk);
        animator.SetFloat("WalkBack", walkBack);
    }

    // =========================
    // JUMP
    // =========================

    void Jump()
    {
        
        if (!isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.W) && isGround)
            {
                
                JumpAction();
                isGround = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && isGround)
            {
                JumpAction();
                isGround = false;
            }
        }
    }

    void JumpAction()
    {
        animator.SetTrigger("Jump");

        speed = 1f;

        // lực nhảy lên
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // 👉 thêm lực bay tới trước
        float dir = yRotationRight ? 1f : -1f;

        velocity.x = dir * 10f; // chỉnh số này để bay xa hơn
    }

    // =========================
    // GRAVITY
    // =========================

    void ApplyGravity()
    {
        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (!controller.isGrounded)
        {
            velocity.x *= 0.98f; // giảm dần bay tới
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // =========================
    // ATTACK
    // =========================

    public void Attack()
    {
        if (!isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                HandleAttackInput();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                HandleAttackInput();
            }
        }
    }

    void HandleAttackInput()
    {
        if (!canAttack) return;
        // chưa attack → bắt đầu attack
        if (!isAttacking)
        {
            AttackStart();
            return;
        }

        // đang attack nhưng trong combo window → combo tiếp
        if (canCombo)
        {
            NextCombo();
        }
    }
    void AttackStart()
    {
        isAttacking = true;
        isMove = false;

        attackComboCount = 1;

        animator.SetTrigger("Attack1");

        DashForward();

        StartComboWindow();
    }
   
  void StartComboWindow()
    {
        if (comboCoroutine != null)
            StopCoroutine(comboCoroutine);

        comboCoroutine = StartCoroutine(ComboRoutine());
    }

    IEnumerator ComboRoutine()
    {
        yield return new WaitForSeconds(0.5f); // mở cửa combo
        canCombo = true;

        yield return new WaitForSeconds(0.2f); // đóng cửa combo
        canCombo = false;

        // nếu không combo → kết thúc attack
        if (attackComboCount == 1|| attackComboCount == 2)
        {
            EndAttack();
        }
    }
    void NextCombo()
    {
        canCombo = false;

        attackComboCount++;

       // DashForward();

        if (attackComboCount == 2)
        {
            animator.SetTrigger("Attack2");
            StartComboWindow(); // mở window cho Attack3
        }
        else if (attackComboCount == 3)
        {
            animator.SetTrigger("Attack3");

            EndComboFinisher();
        }
    }
    void EndComboFinisher()
    {
        isAttacking = false;
        isMove = true;

        attackComboCount = 0;

        StartCoroutine(ComboCooldown());
    }
    IEnumerator ComboCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(1f);

        canAttack = true;
    }
    void DashForward()
    {
        Vector3 dir = yRotationRight ? Vector3.right : Vector3.left;

        controller.Move(dir * 0.5f);
    }

    public void EndAttack()
    {
        isAttacking = false;
        isMove = true;
        canCombo = false;
        attackComboCount = 0;
    }
    // =========================
    // ROTATION
    // =========================

    public void RotatePlayer()
    {
        if (yRotationRight)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    // =========================
    // LOOK TO PLAYER
    // =========================

    public void LookToPlayer()
    {
        if (!controller.isGrounded || isTurning)
        {
            return;
        }

        GameObject player;

        if (!isPlayer2)
        {
            player = GameObject.FindGameObjectWithTag("Player2");
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player1");
        }

        if (player.transform.position.x > transform.position.x)
        {
            if (!yRotationRight)
            {
                StartCoroutine(TurnCharacter(true));
            }
        }
        else
        {
            if (yRotationRight)
            {
                StartCoroutine(TurnCharacter(false));
            }
        }
    }

    IEnumerator TurnCharacter(bool lookRight)
    {
        isTurning = true;

        yield return new WaitForSeconds(0.2f);

        yRotationRight = lookRight;

        isTurning = false;
    }

    // =========================
    // TAKE HIT
    // =========================

    public void TakeHit(bool hitFromRight)
    {
        StartCoroutine(HitCoroutine(hitFromRight));
    }

    IEnumerator HitCoroutine(bool hitFromRight)
    {
        isMove = false;
        isAttacking = true;
        var ran = Random.Range(0, 3);
        float hitPush = 0.5f;

        if (hitFromRight)
        {
            controller.Move(Vector3.right * hitPush);
        }
        else
        {
            controller.Move(Vector3.left * hitPush);
        }
        if (ran == 0)
        {
            animator.SetTrigger("Hit");
        } else if (ran == 1) { animator.SetTrigger("HitLeft"); }
        else animator.SetTrigger("HitRight");



            yield return new WaitForSeconds(0.4f);

        isMove = true;
        isAttacking = false;
    }

    // =========================
    // LOCK POSITION
    // =========================

    public void LockPosition()
    {
        Vector3 pos = transform.position;
        pos.z = lockXPosition;
        transform.position = pos;
    }

    // =========================
    // PLAYER TYPE
    // =========================

    public void SetPlayerType()
    {
        if (!isPlayer2)
        {
            gameObject.tag = "Player1";
        }
        else
        {
            gameObject.tag = "Player2";
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground")){
            isGround = true;
            speed = 5f;
        }
        if (hit.gameObject.CompareTag("Hit"))
        {
            Debug.Log("Hit");
        }
        if (hit.gameObject.CompareTag("Player2")|| hit.gameObject.CompareTag("Player1"))
        {
            // chặn lại khi chạm player khác
            Vector3 pushBack = transform.position - hit.transform.position;
            pushBack.y = 0;

            controller.Move(pushBack * 0.1f);
        }
    }
}