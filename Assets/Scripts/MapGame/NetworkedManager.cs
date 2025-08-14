using Fusion;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NetworkedManager : NetworkBehaviour
{
   public BoardManager boardManager;
    public SpawnPlayer spawnPlayer;
    private bool startGame = false;
    [Networked] public int playerTurnIndex { get; set; } = 0;
    [Networked] public int movesLeft { get; set; } = 0;
    public NetworkedManager instance { get; private set; }
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        if(StartGame())
            Debug.Log("Game is ready to start");
    }
    private bool StartGame()
    {
       if(spawnPlayer.playerIndex >= 2)
       {
            Debug.Log("Game Started");
            return true;
        }
        return false;
    }
}
