using Fusion;
using TMPro;
using UnityEngine;

public class KeyPoint : NetworkBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"KeyPoint Triggered by: {other.name}");
        if (!other.CompareTag("Player")) return;
       
        var networkObject = other.GetComponent<PlayerManager>();
        if (networkObject == null) return;

        var player = networkObject.networkObject.InputAuthority;
        if (player == null) return;
        Debug.Log($"Player {player.PlayerId} reached the key point!");
        GameManager.Instance.GameFinished(player.PlayerId.ToString());
    }
}
