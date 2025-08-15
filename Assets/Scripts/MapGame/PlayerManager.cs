using Fusion;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerManager : NetworkBehaviour
{
    public NetworkObject networkObject;
    private CharacterController _characterController;
    public Animator anim;
    [SerializeField] private InputActionReference moveAction;

    [Networked, OnChangedRender(nameof(UpdateAnimation))]
    public float speed { get; set; } = 5f;
    public float rotationSpeed = 10f;
    private Vector2 _lastInput;

    public override void Spawned()
    {
        networkObject = GetComponentInParent<NetworkObject>();
        _characterController = GetComponent<CharacterController>();
    }

    private void UpdateAnimation()
    {
        if (anim != null)
            anim.SetFloat("Speed", _lastInput.magnitude);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority ||
            GameManager.Instance.hasWinner || 
            !GameManager.Instance.isStarted)
            return;

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        _lastInput = input;

        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        if (moveDir.sqrMagnitude > 0.01f)
        {
            // Move
            Vector3 move = moveDir.normalized * speed * Runner.DeltaTime;
            _characterController.Move(move);

            // Rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);
        }
      
            UpdateAnimation();
    }
}
