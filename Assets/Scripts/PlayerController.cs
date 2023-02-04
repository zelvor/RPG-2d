using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5f;
    private Rigidbody2D _rigidbody;

    private Animator _animator;

    private LayerMask solidObjectsLayer;
    private LayerMask interactableLayer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        solidObjectsLayer = LayerMask.GetMask("SolidObjects");
        interactableLayer = LayerMask.GetMask("Interactable");
    }

    private void OnMove(InputValue value)
    {
        _animator.SetFloat("moveX", value.Get<Vector2>().x);
        _animator.SetFloat("moveY", value.Get<Vector2>().y);
        _animator.SetBool("isMoving", value.Get<Vector2>() != Vector2.zero);
        
        _rigidbody.velocity = value.Get<Vector2>() * moveSpeed;
    }

    private void OnInteract()
    {
        //Check facing direction and interact with object in front of player
        Collider2D collider = Physics2D.OverlapCircle(transform.position, 0.6f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    private void Update()
    {
        Vector2 lastValue = _rigidbody.velocity;
        if(lastValue != Vector2.zero)
        {
            _animator.SetFloat("lastMoveX", lastValue.x);
            _animator.SetFloat("lastMoveY", lastValue.y);
        }
        if (Physics2D.Raycast(transform.position, _rigidbody.velocity, 0.5f, solidObjectsLayer | interactableLayer))
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

}
