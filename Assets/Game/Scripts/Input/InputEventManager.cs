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

	#endregion
	
	#region Camera

	public static Action OnChangePOV;
	public static void FireOnChangePOV()
	{
		OnChangePOV?.Invoke();
	}

	#endregion
}