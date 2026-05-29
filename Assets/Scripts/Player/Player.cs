using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerMove playerMove;
    public PlayerType playerType;
    public PlayerLook playerLook;
    public PlayerAttack playerAttack;
    public PlayerHit playerHit;
    public PlayerAnimator playerAnimator;
    public PlayerDebuff playerDebuff;
    public PlayerBuff playerBuff;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerType = GetComponent<PlayerType>();
        playerLook = GetComponent<PlayerLook>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHit = GetComponent<PlayerHit>();
        playerAnimator = GetComponent<PlayerAnimator>();
        playerDebuff = GetComponent<PlayerDebuff>();
        playerBuff = GetComponent<PlayerBuff>();
    }
}