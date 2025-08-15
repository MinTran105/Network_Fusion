using Fusion;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour, ISpawned
{
    [SerializeField] private GameObject _mainCamera;
    public override void Spawned() => _mainCamera.SetActive(HasStateAuthority);

}