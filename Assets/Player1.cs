using UnityEngine;

public class Player1 : MonoBehaviour
{
    public Rigidbody rb;

    public bool isPlayer2 = false;

    public float speed = 5f;
    public float jumpForce = 5f;
    public bool isGround = true;
    public bool isAttacking = false;
    public bool isMove = true;
    public int attackComboCount = 0;
    public bool yRotationRight = true;

    public Animator animator;

    void Update()
    {
        RotatePlayer();
        Move();
        Jump();
        Attack();

    }

    void Move()
    {
        float walk = 0f;
        float walkBack = 0f;

        // PLAYER 1
        if (!isPlayer2)
        {
            // A
            if (Input.GetKey(KeyCode.A) && isMove)
            {
                // Nếu nhìn phải
                if (yRotationRight)
                {
                    transform.Translate(0, 0, -speed * Time.deltaTime);
                    walkBack = 1f;
                }
                // Nếu nhìn trái
                else
                {
                    transform.Translate(0, 0, speed * Time.deltaTime);
                    walk = 1f;
                }
            }

            // D
            if (Input.GetKey(KeyCode.D) && isMove)
            {
                // Nếu nhìn phải
                if (yRotationRight)
                {
                    transform.Translate(0, 0, speed * Time.deltaTime);
                    walk = 1f;
                }
                // Nếu nhìn trái
                else
                {
                    transform.Translate(0, 0, -speed * Time.deltaTime);
                    walkBack = 1f;
                }
            }
        }

        // PLAYER 2
        else
        {
            // ←
            if (Input.GetKey(KeyCode.LeftArrow) && isMove)
            {
                if (yRotationRight)
                {
                    transform.Translate(0, 0, -speed * Time.deltaTime);
                    walkBack = 1f;
                }
                else
                {
                    transform.Translate(0, 0, speed * Time.deltaTime);
                    walk = 1f;
                }
            }

            // →
            if (Input.GetKey(KeyCode.RightArrow) && isMove)
            {
                if (yRotationRight)
                {
                    transform.Translate(0, 0, speed * Time.deltaTime);
                    walk = 1f;
                }
                else
                {
                    transform.Translate(0, 0, -speed * Time.deltaTime);
                    walkBack = 1f;
                }
            }
        }

        // Animator
        animator.SetFloat("Walk", walk);
        animator.SetFloat("WalkBack", walkBack);
    }
    public void RotatePlayer()
    {
        if (yRotationRight)
        {
            transform.rotation = Quaternion.Euler(0, 100, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -100, 0);
        }
    }

    void Jump()
    {
        // PLAYER 1
        if (!isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.W) && isGround)
            {
                rb.linearVelocity = new Vector3(
                    rb.linearVelocity.x,
                    jumpForce,
                    rb.linearVelocity.z
                );
                animator.SetTrigger("Jump");
                isGround = false;
            }
        }

        // PLAYER 2
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)&& isGround)
            {
                rb.linearVelocity = new Vector3(
                    rb.linearVelocity.x,
                    jumpForce,
                    rb.linearVelocity.z
                );
                animator.SetTrigger("Jump");
                isGround = false;
            }
        }
    }
    public void Attack()
    {
        // PLAYER 1
        if (!isPlayer2)
        {
            if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
            {
                isAttacking = true;
                isMove = false;

                attackComboCount++;

                // Combo 1
                if (attackComboCount == 1)
                {
                    animator.SetTrigger("Attack1");
                }

                // Combo 2
                else if (attackComboCount == 2)
                {
                    animator.SetTrigger("Attack2");
                }

                // Reset combo
                if (attackComboCount >= 2)
                {
                    attackComboCount = 0;
                }
            }
        }

        // PLAYER 2
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
            {
                isAttacking = true;
                isMove = false;

                attackComboCount++;

                // Combo 1
                if (attackComboCount == 1)
                {
                    animator.SetTrigger("Attack1");
                }

                // Combo 2
                else if (attackComboCount == 2)
                {
                    animator.SetTrigger("Attack2");
                }

                // Reset combo
                if (attackComboCount >= 2)
                {
                    attackComboCount = 0;
                }
            }
        }
    }
    public void EndAttack()
    {
        isAttacking = false;
        isMove = true;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    
}