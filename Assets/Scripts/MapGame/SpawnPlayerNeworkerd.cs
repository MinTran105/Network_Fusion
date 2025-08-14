using Fusion;
using UnityEngine;

public class SpawnPlayerNeworkerd : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private Color[] playerColor;
    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log($"Player joined: {player.PlayerId}");
        if (player == Runner.LocalPlayer)
        {
            int locationIndex = player.PlayerId % spawnPoint.Length;
            int colorIndex = player.PlayerId % playerColor.Length;
            var gobj = Runner.Spawn(playerPrefab, spawnPoint[locationIndex].position, Quaternion.identity, player);
          
        }
    }
   
}
    

