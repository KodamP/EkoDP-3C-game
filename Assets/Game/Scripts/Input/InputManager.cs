using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public Action<Vector2> OnMoveInput;
	public Action<bool> OnSprintInput;
	public Action OnJumpInput;
	public Action OnClimbInput;
	public Action OnCancelClimb;
	
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
		if (OnMoveInput != null)
		{
			OnMoveInput(inputAxis);
		}
	}
	
	private void CheckSprintInput()
	{
		bool isHoldSprintInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		if (isHoldSprintInput)
		{
			if (OnSprintInput != null)
			{
				OnSprintInput(true);
				Debug.Log("Sprint");
			}
		}
		else
		{
			if (OnSprintInput != null)
			{
				OnSprintInput(false);
			}
		}
	}
	
	private void CheckJumpInput()
	{
		bool isPressJumpInput = Input.GetKeyDown(KeyCode.Space);
		if (isPressJumpInput)
		{
			if (OnJumpInput != null)
			{
				OnJumpInput();
			}
		}
	}
	
	private void CheckCrouchInput()
	{
		bool isPressCrouchInput = Input.GetKeyDown(KeyCode.LeftControl);
		if (isPressCrouchInput)
		{
			Debug.Log("Crouch");
		}
	}
	
	private void CheckChangePOVInput()
	{
		bool isPressChangePOVInput = Input.GetKeyDown(KeyCode.Q);
		if (isPressChangePOVInput)
		{
			Debug.Log("Change POV");
		}
	}
	
	private void CheckClimbInput()
	{
		bool isPressClimbInput = Input.GetKeyDown(KeyCode.E);
		if (isPressClimbInput)
		{
			OnClimbInput();
		}
	}
	
	private void CheckGlideInput()
	{
		bool isPressGlideInput = Input.GetKeyDown(KeyCode.G);
		if (isPressGlideInput)
		{
			Debug.Log("Glide");
		}
	}
	
	private void CheckCancelInput()
	{
		bool isPressCancelInput = Input.GetKeyDown(KeyCode.C);
		if (isPressCancelInput)
		{
			if (OnCancelClimb != null)
			{
				OnCancelClimb();
			}
		}
	}
	
	private void CheckPunchInput()
	{
		bool isPressPunchInput = Input.GetKeyDown(KeyCode.Mouse0);
		if (isPressPunchInput)
		{
			Debug.Log("Punch");
		}
	}
	
	private void CheckMainMenuInput()
	{
		bool isPressMainMenuInput = Input.GetKeyDown(KeyCode.Escape);
		if (isPressMainMenuInput)
		{
			Debug.Log("Back To Main Menu");
		}
	}
	
}
 
