using Fusion;
using TMPro;
using UnityEngine;

public class KeyPoint : MonoBehaviour
{
    
    private bool hasWinner = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasWinner) return;
        if (!other.CompareTag("Player")) return;

        NetworkObject networkObject = other.GetComponent<NetworkObject>();
        if (networkObject == null) return;

        var player = networkObject.InputAuthority;
        if (player == null) return;

        GameManager.Instance.GameFinished(player.PlayerId.ToString());
    }
}
