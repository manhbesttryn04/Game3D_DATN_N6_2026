using UnityEngine;

public class PlayerBuff : MonoBehaviour
{
    public Player player;

    public void DashForward(float i)
    {
        Vector3 dir = player.playerLook.yRotationRight ? Vector3.right : Vector3.left;
        player.playerMove.controller.Move(dir * i);
    }
}

