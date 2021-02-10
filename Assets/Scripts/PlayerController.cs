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
    private AudioSource _audioSource;
    
    // Props
    [SerializeField] private float turnSpeed = 40f;
    private Quaternion rotation = Quaternion.identity;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
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

        if (isWalking)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void OnAnimatorMove()
    {
        _rigidbody.MovePosition(_rigidbody.position + movement * _animator.deltaPosition.magnitude);
        _rigidbody.MoveRotation(rotation);
    }
}
