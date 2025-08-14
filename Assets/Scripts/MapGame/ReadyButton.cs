using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : NetworkBehaviour
{

    [Header("UI References")]
    public Button notifyButton;

    [SerializeField]
    private GameObject myReadyPanel;

    [SerializeField]
    private GameObject friendReadyPanel;

    // Method called when the button is clicked
    public void OnReadyButtonClicked()
    {
        // Only the client with Input Authority sends the RPC
        //if (!Object.HasInputAuthority)
        //    return;

        // Send RPC to all clients with sender info
        RPC_HandleReady(Runner.LocalPlayer);
    }

    // RPC to handle toggling panels depending on sender
    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_HandleReady(PlayerRef sender)
    {
        if (sender.IsNone) return;
        bool isMe = sender == Runner.LocalPlayer;

        if (isMe)
        {
            // Toggle own panel
            bool state = !myReadyPanel.activeSelf;
            myReadyPanel.SetActive(state);
            GameManager.Instance.isReady = state;
            if (myReadyPanel.activeSelf && friendReadyPanel.activeSelf)
                GameManager.Instance.StartGame();
        }
        else
        {
            // Toggle friend's panel
            bool state = !friendReadyPanel.activeSelf;
            friendReadyPanel.SetActive(state);
            GameManager.Instance.isFriendReady = state;
            if (myReadyPanel.activeSelf && friendReadyPanel.activeSelf)
                GameManager.Instance.StartGame();
        }

    }
}
