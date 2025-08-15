using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerManager : NetworkBehaviour
{
    private CharacterController _characterController;
    private Transform target;
    [SerializeField] private InputActionReference moveAction;
    public float speed { get; set; } = 55f;
    [Networked] public Vector3 NetworkedDestination { get; set; }

    public override void Spawned()
    {
        target = GameObject.FindWithTag("Key").transform;
        _characterController = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority)
            return;
        //if (!GameManager.Instance.isStarted)
        //    return;
        // Cập nhật đích target di chuyển
        Vector3 dir = GetDirection() * speed * Runner.DeltaTime;
        _characterController.Move(dir);
        //Vector3 dest = target.position;
        //agent.speed = speed + Random.Range(0,2f); // Tăng tốc độ ngẫu nhiên
        //if (NetworkedDestination != dest)
        //{
        //    NetworkedDestination = dest;
        //    agent.SetDestination(dest );
        //}

        //// Gửi vị trí agent về client
        //transform.position = agent.transform.position;
        //transform.rotation = agent.transform.rotation;
    }

    private Vector3 GetDirection()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y);
        return direction.normalized;
    }
}
