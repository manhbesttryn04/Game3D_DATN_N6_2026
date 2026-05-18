using UnityEngine;

public class HitBox : MonoBehaviour
{
    public PlayerDebuff owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner.gameObject)
            return;

        if (other.CompareTag("Player2"))
        {
            PlayerDebuff p = other.GetComponent<PlayerDebuff>();
            if (p.player.playerLook.yRotationRight)
            {
                p.TakeHit(true);
            }else p.TakeHit(false);

            // TODO:
            // - damage
            // - knockback
            // - hitstop
        }
    }
}