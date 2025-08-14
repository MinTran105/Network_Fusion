using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RollDice : MonoBehaviour
{
    public Action<int> OnDiceStopped;

    [SerializeField] private Transform[] _diceSides;
    private Vector3 _diceTransform;
    private Quaternion _diceRotation;
    private Rigidbody _rigidbody;
    [SerializeField] private float _force = 5f;
    [SerializeField] private float _torque = 5f;
    private bool _isRolling;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _diceTransform = this.transform.position;
        _diceRotation = this.transform.rotation;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_rigidbody.IsSleeping() && _isRolling)
        {
            int result = GetSideFacingUP();
            
            OnDiceStopped?.Invoke(result);
        }
    }
    public void ResetDice()
    {
        _isRolling = false;
        this.transform.position = _diceTransform;
        this.transform.rotation = _diceRotation;
    }
    public void DiceRolling()
    {
        if (_isRolling) return;

        Vector3 force = new Vector3(0f, _force, 0f);
        Vector3 torque = new Vector3(GetPlusMinusOne(), GetPlusMinusOne(), GetPlusMinusOne()) * _torque;

        _rigidbody.AddForce(force, ForceMode.Impulse);
        _rigidbody.AddTorque(torque, ForceMode.Impulse);
        _isRolling = true;
    }
    private int GetSideFacingUP()
    {
        Transform upSide = null;
        float maxDot = -1;
        foreach (Transform side in _diceSides)
        {
            float dot = Vector3.Dot(side.up, Vector3.up);
            if (!(dot > maxDot)) continue;
            maxDot = dot;
            upSide = side;

        }
        
        _isRolling = false;
        if (!upSide)
            return 0;
        return int.Parse(upSide.name);
    }
    int GetPlusMinusOne()
    {
        return UnityEngine.Random.value < 0.5f ? -1 : 1;
    }
}
