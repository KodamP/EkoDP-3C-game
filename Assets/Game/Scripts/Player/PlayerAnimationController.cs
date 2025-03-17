using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	private Rigidbody _rigidbody;
	private Animator _animator;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		PlayerEventManager.OnAnimationMove += AnimationMove;
		PlayerEventManager.OnAnimationClimb += AnimationClimb;
		PlayerEventManager.OnAnimationJump += AnimationJump;
		PlayerEventManager.OnSetGrounded += SetGrounded;
		PlayerEventManager.OnSetClimbing += SetClimbing;
		PlayerEventManager.OnSetCrouch += SetCrouch;
		PlayerEventManager.OnSetGlide += SetGlide;
		PlayerEventManager.OnAnimationPunch += AnimationPunch;
		PlayerEventManager.OnChangePOV += ChangePOV;
	}

	private void OnDisable()
	{
		PlayerEventManager.OnAnimationMove -= AnimationMove;
		PlayerEventManager.OnAnimationClimb -= AnimationClimb;
		PlayerEventManager.OnAnimationJump -= AnimationJump;
		PlayerEventManager.OnSetGrounded -= SetGrounded;
		PlayerEventManager.OnSetClimbing -= SetClimbing;
		PlayerEventManager.OnSetCrouch -= SetCrouch;
		PlayerEventManager.OnSetGlide -= SetGlide;
		PlayerEventManager.OnAnimationPunch -= AnimationPunch;
		PlayerEventManager.OnChangePOV -= ChangePOV;
	}

	private void AnimationMove(Vector2 axisDirection)
	{
		Vector3 velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
		_animator.SetFloat("Velocity", velocity.magnitude * axisDirection.magnitude);
		_animator.SetFloat("VelocityX", velocity.magnitude * axisDirection.x);
		_animator.SetFloat("VelocityZ", velocity.magnitude * axisDirection.y);
		//Debug.Log("Velocity: " + velocity.magnitude);
	}

	private void AnimationClimb(Vector2 axisDirection)
	{
		Vector3 velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, 0);
		// _animator.SetFloat("ClimbVelocityX", velocity.magnitude * axisDirection.x);
		// _animator.SetFloat("ClimbVelocityY", velocity.magnitude * axisDirection.y);
		_animator.SetFloat("ClimbVelocityX", axisDirection.x);
		_animator.SetFloat("ClimbVelocityY", axisDirection.y);
	}
	
	private void OnAnimatorMove()
	{
		//Untuk lock z position ketika Climbing
		if (_animator.applyRootMotion)
		{
			Vector3 deltaPosition = _animator.deltaPosition;
			deltaPosition.z = 0;
			transform.position += deltaPosition;
			transform.rotation *= _animator.deltaRotation;
		}
	}

	private void AnimationJump()
	{
		//solving "phantom jump" ketika button jump di-trigger ketika animasi "landing" sedang berjalan
		if (_rigidbody.velocity.y > 0)
		{
			_animator.SetTrigger("Jump");
		}
	}

	private void SetGrounded(bool _isGrounded)
	{
		_animator.SetBool("IsGrounded", _isGrounded);
	}

	private void SetClimbing(bool _isClimbing)
	{
		_animator.SetBool("IsClimbing", _isClimbing);
	}

	private void SetCrouch(bool _isCrouch)
	{
		_animator.SetBool("IsCrouch", _isCrouch);
	}

	private void SetGlide(bool _isGliding)
	{
		_animator.SetBool("IsGliding", _isGliding);
	}

	private void AnimationPunch(int _combo)
	{
		_animator.SetInteger("Combo", _combo);
		_animator.SetTrigger("Punch");
	}

	private void ChangePOV()
	{
		_animator.SetTrigger("ChangePOV");
	}
}
