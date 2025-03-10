using System;
using UnityEngine;

public static class InputEventManager
{
	#region PlayerMovement
	
	public static Action<Vector2> OnMoveInput;
	public static void FireOnMoveInput(Vector2 _moveInput)
	{
		OnMoveInput?.Invoke(_moveInput);
	}
		
	public static Action<bool> OnSprintInput;
	public static void FireOnSprintInput(bool _sprintInput)
	{
		OnSprintInput?.Invoke(_sprintInput);
	}
	
	public static Action OnJumpInput;
	public static void FireOnJumpInput()
	{
		OnJumpInput?.Invoke();
	}

	public static Action OnClimbInput;
	public static void FireOnClimbInput()
	{
		OnClimbInput?.Invoke();
	}
	
	public static Action OnCancelClimb;
	public static void FireOnCancelClimb()
	{
		OnCancelClimb?.Invoke();
	}
	
	public static Action OnCrouchInput;
	public static void FireOnCrouchInput()
	{
		OnCrouchInput?.Invoke();
	}

	public static Action OnGlideInput;
	public static void FireOnGlideInput()
	{
		OnGlideInput?.Invoke();
	}

	public static Action OnCancelGlide;
	public static void FireOnCancelGlide()
	{
		OnCancelGlide?.Invoke();
	}

	public static Action OnPunchInput;
	public static void FireOnPunchInput()
	{
		OnPunchInput?.Invoke();
	}

	#endregion
	
	#region Camera

	public static Action OnChangePOV;
	public static void FireOnChangePOV()
	{
		OnChangePOV?.Invoke();
	}

	#endregion
}