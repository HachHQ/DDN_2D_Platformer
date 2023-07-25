using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private float _speedOfPlayer = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float _fallGravityScale = 5f;
    [SerializeField] private float _normalGravityScale = 1f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _reycastGroundCheckDistance = 0.5f;

    private float _moveX = 0f;
    private BoxCollider2D _boxColl;
    private Rigidbody2D _rb;
    private bool _faceLeft = true;
    //private bool _isJumping;

    public bool InputJumpCheck;
    public bool InputMoveCheck;
    public bool OnGround;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxColl = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        InputMoving();

        InputJumping();

        OnGroundCheck();

        ReflectSprite();
    }

    private void FixedUpdate()
    {
        Moving();

        Jumping();

        ChangeGravityVariable();
    }


	#region Input Systems
	private void InputJumping()
    {
        if (Input.GetButtonDown("Jump") && OnGround) InputJumpCheck = true;
    }

    private void InputMoving()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        if (moveX != 0)
        {
            InputMoveCheck = true;
        }
    }
    #endregion

	private void Moving()
    {
        if (InputMoveCheck)
        {
            _moveX = Input.GetAxisRaw("Horizontal") * _speedOfPlayer;
            _rb.velocity = new Vector2(_moveX, _rb.velocity.y);
            if (_moveX == 0 ) InputMoveCheck = false;
        }
    }

    private void Jumping()
    {
        if (InputJumpCheck && OnGround)
        {
            //_isJumping = true;
            _rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            InputJumpCheck = false;
		}
    }

    private void ChangeGravityVariable()
    {
		if (_rb.velocity.y >= 0)
		{
			_rb.gravityScale = _normalGravityScale;
		}
		else
		{
			_rb.gravityScale = _fallGravityScale;
		}
	}

    private void OnGroundCheck()
    {
        Vector2 leftBoundOfColl = new Vector2(_boxColl.bounds.min.x + 0.1f, _rb.transform.position.y);
        Vector2 rightBoundOfColl = new Vector2(_boxColl.bounds.max.x - 0.1f, _rb.transform.position.y);

		RaycastHit2D hit1 = Physics2D.Raycast(leftBoundOfColl, -Vector2.up, _reycastGroundCheckDistance, _layerMask);
		RaycastHit2D hit2 = Physics2D.Raycast(rightBoundOfColl, -Vector2.up, _reycastGroundCheckDistance, _layerMask);
		OnGround = hit1 || hit2;
    }

	private void ReflectSprite()
	{
		if ((_moveX < 0 && !_faceLeft) || (_moveX > 0 && _faceLeft))
		{
			transform.localScale *= new Vector2(-1, 1);
			_faceLeft = !_faceLeft;
		}
	}
}
