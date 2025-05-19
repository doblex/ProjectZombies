using UnityEngine;

public struct PlayerInfo
{
    public Vector3 currentPosition;
    public Vector3 forwardVector;
}

public class AIManager : MonoBehaviour
{
    public static AIManager Instance;

    public delegate void PlayerInfoDelegate(PlayerInfo info);
    public static event PlayerInfoDelegate OnPlayerInfo;

    private Player player;
    private AIController[] controller;

    private PlayerInfo playerInfo;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        if (player != null)
            UpdatePlayerInfo();
    }

    void UpdatePlayerInfo()
    {
        playerInfo.currentPosition = player.transform.position;
        playerInfo.forwardVector = player.transform.forward;

        OnPlayerInfo?.Invoke(playerInfo);
    }

}
