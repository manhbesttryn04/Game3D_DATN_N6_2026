using UnityEngine;

public class HitBox : MonoBehaviour
{
    public PlayerHit owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner.gameObject)
            return;

        if (other.CompareTag("Player2"))
        {
          PlayerHit p = other.GetComponent<PlayerHit>();
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