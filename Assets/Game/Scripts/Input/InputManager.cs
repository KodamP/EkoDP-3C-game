using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	private void Update()
	{
		CheckMovementInput();
		CheckSprintInput();
		CheckJumpInput();
		CheckCrouchInput();
		CheckChangePOVInput();
		CheckClimbInput();
		CheckGlideInput();
		CheckCancelInput();
		CheckPunchInput();
		CheckMainMenuInput();
	}
	
	private void CheckMovementInput()
	{
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		//Debug.Log("Vertical Axis: " + verticalAxis);
		//Debug.Log("Horizontal Axis: " + horizontalAxis);
		Vector2 inputAxis = new Vector2(horizontalAxis, verticalAxis);
		InputEventManager.FireOnMoveInput(inputAxis);
	}
	
	private void CheckSprintInput()
	{
		bool isHoldSprintInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		if (isHoldSprintInput)
		{
			InputEventManager.FireOnSprintInput(true);
		}
		else
		{
			InputEventManager.FireOnSprintInput(false);
		}
	}
	
	private void CheckJumpInput()
	{
		bool isPressJumpInput = Input.GetKeyDown(KeyCode.Space);
		if (isPressJumpInput)
		{
			InputEventManager.FireOnJumpInput();
		}
	}
	
	private void CheckCrouchInput()
	{
		bool isPressCrouchInput = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
		if (isPressCrouchInput)
		{
			InputEventManager.FireOnCrouchInput();
		}
	}
	
	private void CheckChangePOVInput()
	{
		bool isPressChangePOVInput = Input.GetKeyDown(KeyCode.Q);
		if (isPressChangePOVInput)
		{
			InputEventManager.FireOnChangePOV();
		}
	}
	
	private void CheckClimbInput()
	{
		bool isPressClimbInput = Input.GetKeyDown(KeyCode.E);
		if (isPressClimbInput)
		{
			InputEventManager.FireOnClimbInput();
		}
	}
	
	private void CheckGlideInput()
	{
		bool isPressGlideInput = Input.GetKeyDown(KeyCode.G);
		if (isPressGlideInput)
		{
				InputEventManager.FireOnGlideInput();
		}
	}
	
	private void CheckCancelInput()
	{
		bool isPressCancelInput = Input.GetKeyDown(KeyCode.C);
		if (isPressCancelInput)
		{
			InputEventManager.FireOnCancelClimb();
			InputEventManager.FireOnCancelGlide();
		}
	}
	
	private void CheckPunchInput()
	{
		bool isPressPunchInput = Input.GetKeyDown(KeyCode.Mouse0);
		if (isPressPunchInput)
		{
			InputEventManager.FireOnPunchInput();
		}
	}
	
	private void CheckMainMenuInput()
	{
		bool isPressMainMenuInput = Input.GetKeyDown(KeyCode.Escape);
		if (isPressMainMenuInput)
		{
			//Debug.Log("Back To Main Menu");
		}
	}
	
}
 
