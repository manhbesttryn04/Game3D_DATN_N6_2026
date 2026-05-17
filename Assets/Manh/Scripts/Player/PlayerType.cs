using UnityEngine;

public class PlayerType : MonoBehaviour
{
    public Player player;
    public bool isPlayer2 = false;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        SetPlayerType();
    }
    public void SetPlayerType()
    {
        gameObject.tag = isPlayer2 ? "Player2" : "Player1";
    }
}
