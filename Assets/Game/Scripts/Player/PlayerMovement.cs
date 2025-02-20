using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private InputManager _input;
	[SerializeField] private float _walkSpeed;
	[SerializeField] private float _sprintSpeed;
	[SerializeField] private float _walkSprintTransition;
	[SerializeField] private float _rotationSmoothTime = 0.1f;
	
	[Header("Jump")]
	[SerializeField] private float _jumpForce;
	[SerializeField] private Transform _groundDetector;
	[SerializeField] private float _detectorRadius;
	[SerializeField] private LayerMask _groundLayer;
	
	[Header("Stair Climb")]
	[SerializeField] private Vector3 _upperStepOffset;
	[SerializeField] private float _stepCheckerDistance;
	[SerializeField] private float _stepForce;
	
	[Header("Wall Climb")]
	[SerializeField] private Transform _climbDetector;
	[SerializeField] private float _climbCheckDistance;
	[SerializeField] private LayerMask _climbableLayer;
	[SerializeField] private Vector3 _climbOffset;
	[SerializeField] private float _climbSpeed;
	
	//Movement
	private float _speed;
	private float _rotationSmoothVelocity;
	private Rigidbody _rigidbody;
	//Jump
	private bool _isGrounded;
	//Climb
	private PlayerStance _playerStance;
	
	#region Unity Functions

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_speed = _walkSpeed;
		_playerStance = PlayerStance.Stand;
	}
	
	private void Start()
	{
		_input.OnMoveInput += Move;
		_input.OnSprintInput += Sprint;
		_input.OnJumpInput += Jump;
		_input.OnClimbInput += StartClimb;
		_input.OnCancelClimb += CancelClimb;
	}

	private void Update()
	{
		CheckIsGrounded();
		CheckStep();
		DropFromClimbableWall();
	}
	
	private void OnDestroy()
	{
		_input.OnMoveInput -= Move;
		_input.OnSprintInput -= Sprint;
		_input.OnJumpInput -= Jump;
		_input.OnClimbInput += StartClimb;
		_input.OnCancelClimb -= CancelClimb;
	}
	
	#endregion
	
	private void Move(Vector2 axisDirection)
	{
		Vector3 movementDirection = Vector3.zero;
		bool isPlayerStanding = _playerStance == PlayerStance.Stand;
		bool isPlayerClimbing = _playerStance == PlayerStance.Climb;
		if (isPlayerStanding)
		{
			if (axisDirection.magnitude >= 0.1)
			{
				float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg;
				float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle,
					ref _rotationSmoothVelocity, _rotationSmoothTime);
				transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
				movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
				//Di course ditulis untuk seperti berikut:
				//_rigidbody.AddForce(movementDirection * Time.deltaTime * _walkSpeed);
				//Hanya saja ketika di sprint tidak masuk inputnya ke movement, sehingga saya ubah menjadi seperti berikut:
				_rigidbody.AddForce(movementDirection * Time.deltaTime * _speed);
				//alasan saya adalah karena setelah saya periksa, _speed tidak direferensikan/dipakai di manapun, sedangkan
				//_sprintSpeed nyambungnya ke _speed, bukan ke _walkSpeed, begitu.
				//ya akhirnya ketahuan di course Agate yg bagian 0.1c - Control - Climb sih wkwk.
			}
		}
		else if (isPlayerClimbing)
		{
			Vector3 horizontal = axisDirection.x * transform.right;
			Vector3 vertical = axisDirection.y * transform.up;
			movementDirection = horizontal + vertical;
			_rigidbody.AddForce(movementDirection * Time.deltaTime * _climbSpeed);
		}
	}
	
	private void Sprint(bool isSprint)
	{
		if (isSprint)
		{
			if (_speed < _sprintSpeed)
			{
				_speed = _speed + _walkSprintTransition * Time.deltaTime;
			}
		}
		else
		{
			if (_speed > _walkSpeed)
			{
				_speed = _speed - _walkSprintTransition * Time.deltaTime;
			}
		}
	}

	private void Jump()
	{
		if (_isGrounded)
		{
			Vector3 jumpDirection = Vector3.up;
			_rigidbody.AddForce(jumpDirection * _jumpForce * Time.deltaTime);
		}
	}
	
	private void CheckIsGrounded()
	{
		_isGrounded = Physics.CheckSphere(_groundDetector.position, _detectorRadius, _groundLayer);
	}

	private void CheckStep()
	{
		bool isHitLowerStep = Physics.Raycast(_groundDetector.position, transform.forward, 
			_stepCheckerDistance);
		bool isHitUpperStep = Physics.Raycast(_groundDetector.position + _upperStepOffset, 
			transform.forward, _stepCheckerDistance);
		if (isHitLowerStep && !isHitUpperStep)
		{
			_rigidbody.AddForce(0, _stepForce, 0);
		}
	}

	private void StartClimb()
	{
		bool isInFrontOfClimbingWall = Physics.Raycast(_climbDetector.position, transform.forward, 
			out RaycastHit hit, _climbCheckDistance, _climbableLayer);
		bool isNotClimbing = _playerStance != PlayerStance.Climb;
		if (isInFrontOfClimbingWall && _isGrounded && isNotClimbing)
		{
			Vector3 offset = (transform.forward * _climbOffset.z) + (Vector3.up * _climbOffset.y);
			transform.position = hit.point - offset;
			_playerStance = PlayerStance.Climb;
			_rigidbody.useGravity = false;
		}
	}

	private void CancelClimb() 
	{
		if (_playerStance == PlayerStance.Climb)
		{
			_playerStance = PlayerStance.Stand;
			_rigidbody.useGravity = true;
			transform.position -= transform.forward * 1f;
		}
	}

	private void DropFromClimbableWall()
	{
		if (_playerStance == PlayerStance.Climb)
		{
			bool isInFrontOfClimbingWall = Physics.Raycast(_climbDetector.position, transform.forward, 
				out RaycastHit hit, _climbCheckDistance, _climbableLayer);
			if (isInFrontOfClimbingWall != true)
			{
				CancelClimb();
			}
		}
	}
	
}