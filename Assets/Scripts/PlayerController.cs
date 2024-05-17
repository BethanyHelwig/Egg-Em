using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rigidbody;
    //public float speed = 10f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue value)
    {
        _rigidbody.velocity = value.Get<Vector2>() * GameManager.instance.playerSpeed;
    }

}
