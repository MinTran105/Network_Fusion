using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : NetworkBehaviour
{
    public TextMeshProUGUI text;
    public static GameManager Instance;
    public bool hasWinner = false;
    public bool isStarted = false;
    public GameObject readyPanel;
    [Networked] public bool isFriendReady {get; set; } = false;
    [Networked]public bool isReady { get; set; } = false;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        text.gameObject.SetActive(false);
    }
    
    public void FriendReady()
    {
        NetworkObject networkObject = GetComponent<NetworkObject>();
    }
    private void Update()
    {
    }
    public void StartGame()
    {
        
        readyPanel.SetActive(false);
        Debug.Log("Start Game");
        StartCoroutine(WaitForGameStart());
    }

    IEnumerator WaitForGameStart()
    {
        text.gameObject.SetActive(true);
        for (int i = 3; i >= 0; i--)
        {
            text.text = $"Game starts in \n {i}";
            yield return new WaitForSeconds(1f);
        }
        isStarted = true;
        text.gameObject.SetActive(false);
    }
    public void GameFinished(string playerid)
    {
        if(!hasWinner)
        {
            hasWinner = true;
            Debug.Log("Game Finished");
            isStarted = false;
            isReady = false;
            isFriendReady = false;
            hasWinner = false;
            text.gameObject.SetActive(true);
            text.text = $"Player {playerid} win!";
        }
    }
}
