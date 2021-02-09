using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    RequireComponent(typeof(Rigidbody)),
    RequireComponent(typeof(Animator))
]
public class PlayerController : MonoBehaviour
{
    // Constants
    private const string IS_WALKING = "IsWalking";
    
    // Components
    private Vector3 movement;
    private Animator _animator;
    private Rigidbody _rigidbody;
    
    // Props
    [SerializeField] private float turnSpeed = 40f;
    private Quaternion rotation = Quaternion.identity;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal"), vertical = Input.GetAxis("Vertical");
        
        movement.Set(horizontal, 0, vertical);

        movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0),
            hasVerticalInput = !Mathf.Approximately(vertical, 0),
            isWalking = hasHorizontalInput || hasVerticalInput;

        _animator.SetBool(IS_WALKING, isWalking);
        
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.fixedTime, 0f);

        rotation = Quaternion.LookRotation(desiredForward);
        
        
    }

    private void OnAnimatorMove()
    {
        _rigidbody.MovePosition(_rigidbody.position + movement * _animator.deltaPosition.magnitude);
        _rigidbody.MoveRotation(rotation);
    }
}
