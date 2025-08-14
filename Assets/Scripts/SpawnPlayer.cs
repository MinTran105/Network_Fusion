using UnityEngine;
using Fusion;
public class SpawnPlayer : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject[] playerPrefab;
    public BoardManager boardManager;
    [Networked] public int playerIndex { get; set; } = 0;   

    public void PlayerJoined(PlayerRef player)
    {
        print("player joined " + player.PlayerId);
        if (player == Runner.LocalPlayer)
        {
            int index = Random.Range(0, playerPrefab.Length);
            // Spawn the player object for the local player
            Runner.Spawn(playerPrefab[index],boardManager.SetNetworkStartSpawnPlayer(), Quaternion.identity, player);

            playerIndex++;
            Debug.Log("Player join: " + playerIndex);
        }
    }
}
