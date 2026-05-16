using UnityEngine;

public class HitBoxAttack : MonoBehaviour
{
    public BoxCollider leftHandHitBox;
    public BoxCollider rightHandHitBox;

    void Start()
    {
        leftHandHitBox.enabled = false;
        rightHandHitBox.enabled = false;
    }

    // =====================
    // ATTACK 1
    // =====================

    public void EnableLeftHand()
    {
        rightHandHitBox.enabled = true;
    }

    public void DisableLeftHand()
    {
        rightHandHitBox.enabled = false;
    }

    // =====================
    // ATTACK 2
    // =====================

    public void EnableBothHands()
    {
        leftHandHitBox.enabled = true;
        rightHandHitBox.enabled = true;
    }

    public void DisableBothHands()
    {
        leftHandHitBox.enabled = false;
        rightHandHitBox.enabled = false;
    }
}