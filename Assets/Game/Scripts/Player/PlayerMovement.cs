using System;
using System.Collections;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement")]
	[SerializeField] private float _walkSpeed;
	[SerializeField] private float _sprintSpeed;
	[SerializeField] private float _walkSprintTransition;
	[SerializeField] private float _rotationSmoothTime = 0.1f;
	
	[Header("Jump")]
	[SerializeField] private float _jumpForce;
	[SerializeField] private Transform _groundDetector;
	[SerializeField] private float _detectorRadius;
	[SerializeField] private LayerMask _groundLayer;
	
	[Header("Crouch")]
	[SerializeField] private float _crouchSpeed;
	
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
	
	[Header("Glide")]
	[SerializeField] private float _glideSpeed;
	[SerializeField] private float _airDrag;
	[SerializeField] private Vector3 _glideRotationSpeed;
	[SerializeField] private float _minGlideRotationX;
	[SerializeField] private float _maxGlideRotationX;
	
	[Header("Punch")]
	[SerializeField] private float _resetComboInterval;
	[SerializeField] private Transform _hitDetector;
	[SerializeField] private float _hitDetectorRadius;
	[SerializeField] private LayerMask _hitLayer;

	[Header("Camera")]
	[SerializeField] private Transform _cameraTransform;
	[SerializeField] private CameraManager _cameraManager;
	
	//Movement
	private float _speed;
	private float _rotationSmoothVelocity;
	private Rigidbody _rigidbody;
	//Jump
	private bool _isGrounded;
	//Climb
	private PlayerStance _playerStance;
	//Punch
	private bool _isPunching;
	private int _combo = 0;
	private Coroutine _resetCombo;
	//Collider
	private CapsuleCollider _collider;
	//Animator
	private Animator _animator;

	#region Unity Functions

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_speed = _walkSpeed;
		_playerStance = PlayerStance.Stand;
		_animator = GetComponent<Animator>();
		_collider = GetComponent<CapsuleCollider>();
		
	}

	private void Start()
	{
		InputEventManager.OnMoveInput += Move;
		InputEventManager.OnSprintInput += Sprint;
		InputEventManager.OnJumpInput += Jump;
		InputEventManager.OnClimbInput += StartClimb;
		InputEventManager.OnCancelClimb += CancelClimb;
		InputEventManager.OnCrouchInput += Crouch;
		InputEventManager.OnGlideInput += StartGlide;
		InputEventManager.OnCancelGlide += CancelGlide;
		InputEventManager.OnPunchInput += Punch;
	}

	private void Update()
	{
		CheckIsGrounded();
		CheckStep();
		DropFromClimbableWall();
		Glide();
	}
	
	private void OnDestroy()
	{
		InputEventManager.OnMoveInput -= Move;
		InputEventManager.OnSprintInput -= Sprint;
		InputEventManager.OnJumpInput -= Jump;
		InputEventManager.OnClimbInput -= StartClimb;
		InputEventManager.OnCancelClimb -= CancelClimb;
		InputEventManager.OnCrouchInput -= Crouch;
		InputEventManager.OnGlideInput -= StartGlide;
		InputEventManager.OnCancelGlide -= CancelGlide;
		InputEventManager.OnPunchInput -= Punch;
	}
	
	#endregion
	
	private void Move(Vector2 axisDirection)
	{
		Vector3 movementDirection;
		bool isPlayerStanding = _playerStance == PlayerStance.Stand;
		bool isPlayerClimbing = _playerStance == PlayerStance.Climb;
		bool isPlayerCrouching = _playerStance == PlayerStance.Crouch;
		bool isPlayerGliding = _playerStance == PlayerStance.Glide;
		if ((isPlayerStanding || isPlayerCrouching) && !_isPunching)
		{
			switch (_cameraManager.CameraState)
			{
				case CameraState.ThirdPerson:
					if (axisDirection.magnitude >= 0.1)
					{
						float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg + 
							_cameraTransform.eulerAngles.y;
						float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle,
							ref _rotationSmoothVelocity, _rotationSmoothTime);
						transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
						movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
						_rigidbody.AddForce(movementDirection * (Time.deltaTime * _speed));
					}
					break;
				case CameraState.FirstPerson:
					transform.rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);
					Vector3 verticalDirection = axisDirection.y * transform.forward;
					Vector3 horizontalDirection = axisDirection.x * transform.right;
					movementDirection = verticalDirection + horizontalDirection;
					_rigidbody.AddForce(movementDirection * (Time.deltaTime * _speed));
					break;
			}
			PlayerEventManager.FireOnAnimationMove(axisDirection);
		}
		else if (isPlayerClimbing)
		{
			Vector3 horizontal = axisDirection.x * transform.right;
			Vector3 vertical = axisDirection.y * transform.up;
			movementDirection = horizontal + vertical;
			_rigidbody.AddForce(movementDirection * (Time.deltaTime * _climbSpeed));
			PlayerEventManager.FireOnAnimationClimb(axisDirection);
		}
		else if (isPlayerGliding)
		{
			Vector3 rotationDegree = transform.rotation.eulerAngles;
			rotationDegree.x += _glideRotationSpeed.x * axisDirection.y * Time.deltaTime;
			rotationDegree.x = Mathf.Clamp(rotationDegree.x, _minGlideRotationX, _maxGlideRotationX);
			rotationDegree.y += _glideRotationSpeed.y * axisDirection.x * Time.deltaTime;
			rotationDegree.z += _glideRotationSpeed.z * axisDirection.x * Time.deltaTime;
			transform.rotation = Quaternion.Euler(rotationDegree);
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
			_rigidbody.AddForce(jumpDirection * (_jumpForce * _jumpForce * Time.deltaTime));
			PlayerEventManager.FireOnAnimationJump();
		}
	}
	
	private void CheckIsGrounded()
	{
		_isGrounded = Physics.CheckSphere(_groundDetector.position, _detectorRadius, _groundLayer);
		PlayerEventManager.FireOnSetGrounded(_isGrounded);
		if (_isGrounded)
		{
			CancelGlide();
		}
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
			//Supaya player tetap sejajar dengan permukaan "Climbable Wall" ketika memanjat
			transform.rotation = Quaternion.LookRotation(-hit.normal);
			_playerStance = PlayerStance.Climb;
			_rigidbody.useGravity = false;
			_cameraManager.SetFPSClampedCamera(true, transform.rotation.eulerAngles);
			_cameraManager.SetTPSFieldOfView(70);
			PlayerEventManager.FireOnSetClimbing(true);
			_collider.center = Vector3.up * 1.3f;
		}
	}

	private void CancelClimb() 
	{
		if (_playerStance == PlayerStance.Climb)
		{
			_playerStance = PlayerStance.Stand;
			_rigidbody.useGravity = true;
			transform.position -= transform.forward * 1f;
			_cameraManager.SetFPSClampedCamera(false, transform.rotation.eulerAngles);
			_cameraManager.SetTPSFieldOfView(40);
			PlayerEventManager.FireOnSetClimbing(false);
			_collider.center = Vector3.up * 0.9f;
		}
	}

	//Buat cek apabila player keluar dari climbing wall, maka otomatis CancelClimb.
	private void DropFromClimbableWall()
	{
		if (_playerStance == PlayerStance.Climb)
		{
			bool isInFrontOfClimbingWall = Physics.Raycast(_climbDetector.position, transform.forward, 
				out RaycastHit _hit, _climbCheckDistance, _climbableLayer);
			if (isInFrontOfClimbingWall != true)
			{
				CancelClimb();
			}
		}
	}

	private void Crouch()
	{
		if (_playerStance == PlayerStance.Stand)
		{
			_playerStance = PlayerStance.Crouch;
			PlayerEventManager.FireOnSetCrouch(true);
			_speed = _crouchSpeed;
			_collider.height = 1.3f;
			_collider.center = Vector3.up * 0.66f;
		}
		else
		{
			_playerStance = PlayerStance.Stand;
			PlayerEventManager.FireOnSetCrouch(false);
			_speed = _walkSpeed;
			_collider.height = 1.8f;
			_collider.center = Vector3.up * 0.9f;
		}
	}

	private void StartGlide()
	{
		if (_playerStance != PlayerStance.Glide && !_isGrounded)
		{
			_playerStance = PlayerStance.Glide;
			PlayerEventManager.FireOnSetGlide(true);
			_cameraManager.SetFPSClampedCamera(true, transform.rotation.eulerAngles);
		}
	}

	private void CancelGlide()
	{
		if (_playerStance == PlayerStance.Glide)
		{
			_playerStance = PlayerStance.Stand;
			PlayerEventManager.FireOnSetGlide(false);
			_cameraManager.SetFPSClampedCamera(false, transform.rotation.eulerAngles);
		}
	}

	private void Glide()
	{
		if (_playerStance == PlayerStance.Glide)
		{
			Vector3 playerRotation = transform.rotation.eulerAngles;
			float lift = playerRotation.x;
			Vector3 upForce = transform.up * (lift + _airDrag);
			Vector3 forwardForce = transform.forward * _glideSpeed;
			Vector3 totalForce = upForce + forwardForce;
			_rigidbody.AddForce(totalForce * Time.deltaTime);
		}
	}
	
	private void Punch()
	{
		if (!_isPunching && _playerStance == PlayerStance.Stand)
		{
			_isPunching = true;
			if (_combo < 3)
			{
				_combo += 1;
			}
			else
			{ 
				_combo = 1;
			}
			PlayerEventManager.FireOnAnimationPunch(_combo);
		}
	}

	private void EndPunch()
	{
			_isPunching = false;
			if (_resetCombo != null)
			{
				StopCoroutine(_resetCombo);
			}
			_resetCombo = StartCoroutine(ResetCombo());
	}

	private IEnumerator ResetCombo()
	{
		yield return new WaitForSeconds(_resetComboInterval);
		_combo = 0;
	}

	private void Hit()
	{
		Collider[] hitObjects = Physics.OverlapSphere(_hitDetector.position, _hitDetectorRadius, _hitLayer);
		for (int i = 0; i < hitObjects.Length; i++)
		{
			if (hitObjects[i] != null)
			{
				Destroy(hitObjects[i].gameObject);
			}
		}
	}
}