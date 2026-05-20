using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    // =========================
    // REFERENCES
    // =========================

    public Player player;

    // =========================
    // HITBOXES
    // =========================

    public BoxCollider leftHandHitBox;
    public BoxCollider rightHandHitBox;

    // =========================
    // UNITY METHODS
    // =========================

    void Start()
    {
        leftHandHitBox.enabled = false;
        rightHandHitBox.enabled = false;
    }

    // =========================
    // LEFT HAND
    // =========================

    public void EnableLeftHand()
    {
        rightHandHitBox.enabled = true;
    }

    public void DisableLeftHand()
    {
        rightHandHitBox.enabled = false;
    }

    // =========================
    // BOTH HANDS
    // =========================

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