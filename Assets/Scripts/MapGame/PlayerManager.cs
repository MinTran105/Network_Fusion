using Fusion;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : NetworkBehaviour
{
    public NavMeshAgent agent;
    private Transform target;
    [Networked] public float speed { get; set; } = 5f;
    [Networked] public Vector3 NetworkedDestination { get; set; }

    public override void Spawned()
    {
        target = GameObject.FindWithTag("Key").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    public override void FixedUpdateNetwork()
    {
        // Chỉ host/server tính đường
        if (!Object.HasStateAuthority)
            return;
        if (!GameManager.Instance.isStarted)
            return;
        // Cập nhật đích target di chuyển
        Vector3 dest = target.position;
        agent.speed = speed + Random.Range(0,2f); // Tăng tốc độ ngẫu nhiên
        if (NetworkedDestination != dest)
        {
            NetworkedDestination = dest;
            agent.SetDestination(dest );
        }

        // Gửi vị trí agent về client
        transform.position = agent.transform.position;
        transform.rotation = agent.transform.rotation;
    }
}
