using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _catsSpeed = 10f;
	[SerializeField] private Animator _animator;
    [SerializeField] private float highOfJump = 10f;
	[SerializeField] private float gravityScale = 10;
	[SerializeField] private float fallGravityScale = 30;


	public Transform GroundCheckPoint;


	private BoxCollider2D _groundCheckColl;
	private float _deltaX;
	private Rigidbody2D _catRB;
	private float buttonPressedTime;
	private bool jumping;
	private bool faceRight = true;
	private bool onGround;
	private bool isJumping;

	private void Start()
    {
        _catRB = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_groundCheckColl = GetComponentInChildren<BoxCollider2D>();
    }

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space) && onGround)
		{
			isJumping = true;
		}

		_animator.SetBool("OnGround", onGround);
		_animator.SetFloat("VelocityOfRB", _catRB.velocity.y);
	}

    private void FixedUpdate()
    {
		Debug.Log($"{onGround} : {_catRB.velocity.y}");

		Moving();

		Jumping();

		OnGroundCheck();

		Reflect();
	}

	private void Reflect()
	{
		if((_deltaX > 0 && !faceRight) || (_deltaX < 0 && faceRight))
		{
			transform.localScale *= new Vector2(-1, 1);
			faceRight = !faceRight;
		}
	}
	
	private void Jumping()
	{
		if (isJumping)
		{
			_catRB.gravityScale = gravityScale;
			_catRB.AddForce(Vector2.up * highOfJump, ForceMode2D.Force);
			Debug.Log($"Space was pressed \nonGround State - {onGround}");
			isJumping = false;
		}

		if (_catRB.velocity.y > 0)
		{
			_catRB.gravityScale = gravityScale;
		}
		else
		{
			_catRB.gravityScale = fallGravityScale;
		}
	}

	private void Moving()
	{
		_deltaX = Input.GetAxis("Horizontal") * _catsSpeed;

		_catRB.velocity = Vector2.right * _deltaX * _catsSpeed * Time.fixedDeltaTime;

		_animator.SetFloat("Move", Mathf.Abs(_deltaX));
	}

	private void OnGroundCheck()
	{
		Vector2 checkGroundCollCorner1 = new Vector2(_groundCheckColl.bounds.max.x, _groundCheckColl.bounds.min.y);
		Vector2 checkGroundCollCorner2 = new Vector2(_groundCheckColl.bounds.min.x, _groundCheckColl.bounds.min.y);

		onGround = Physics2D.OverlapArea(checkGroundCollCorner1, checkGroundCollCorner2, LayerMask.GetMask("Ground"));
	}
}
