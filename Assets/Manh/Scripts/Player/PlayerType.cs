using UnityEngine;

public class PlayerType : MonoBehaviour
{
    // =========================
    // REFERENCES
    // =========================

    public Player player;

    // =========================
    // PLAYER SIDE
    // =========================

    public bool isPlayer2 = false;

    // =========================
    // WEIGHT TYPE
    // =========================

    public bool isLowWeight;
    public bool isHighWeight;

    // =========================
    // UNITY METHODS
    // =========================

    void Start()
    {
        player = GetComponent<Player>();
    }
    public void Update()
    {
        SetPlayerType();
    }

    // =========================
    // PLAYER TYPE
    // =========================

    public void SetPlayerType()
    {
        gameObject.tag = isPlayer2 ?
            "Player2" :
            "Player1";
    }
}