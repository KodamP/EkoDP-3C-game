using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerEventManager
{
	public static Action<Vector2> OnAnimationMove;
	
	#region Animation

	public static void FireOnAnimationMove(Vector2 axisDirection)
	{
		OnAnimationMove?.Invoke(axisDirection);
	}
	
	public static Action<Vector2> OnAnimationClimb;
	public static void FireOnAnimationClimb(Vector2 axisDirection)
	{
		OnAnimationClimb?.Invoke(axisDirection);
	}

	public static Action OnAnimationJump;
	public static void FireOnAnimationJump()
	{
		OnAnimationJump?.Invoke();
	}

	public static Action<bool> OnSetGrounded;
	public static void FireOnSetGrounded(bool _isGrounded)
	{
		OnSetGrounded?.Invoke(_isGrounded);
	}

	public static Action<bool> OnSetClimbing;
	public static void FireOnSetClimbing(bool _isClimbing)
	{
		OnSetClimbing?.Invoke(_isClimbing);
	}

	public static Action<bool> OnSetCrouch;
	public static void FireOnSetCrouch(bool _isCrouch)
	{
		OnSetCrouch?.Invoke(_isCrouch);
	}
	
	public static Action<bool> OnSetGlide;
	public static void FireOnSetGlide(bool _isGliding)
	{
		OnSetGlide?.Invoke(_isGliding);
	}

	public static Action<int> OnAnimationPunch;
	public static void FireOnAnimationPunch(int _combo)
	{
		OnAnimationPunch?.Invoke(_combo);
	}

	public static Action OnChangePOV;
	public static void FireOnChangePOV()
	{
		OnChangePOV?.Invoke();
	}

	#endregion

	#region Audio

	public static Action<bool> OnAudioGliding;
	public static void FireOnAudioGliding(bool _glideSFX)
	{
		OnAudioGliding?.Invoke(_glideSFX);
	}
	
	#endregion
}
