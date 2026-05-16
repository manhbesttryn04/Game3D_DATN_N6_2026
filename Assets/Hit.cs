using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Player1 owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner.gameObject)
            return;

        if (other.CompareTag("Player2"))
        {
           Player1 p = other.GetComponent<Player1>();
            if (p.yRotationRight)
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