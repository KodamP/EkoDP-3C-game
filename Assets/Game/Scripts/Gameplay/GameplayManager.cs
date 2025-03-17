using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
	private void Start()
	{
		InputEventManager.OnMainMenuInput += BackToMainMenu;
	}

	private void OnDestroy()
	{
		InputEventManager.OnMainMenuInput -= BackToMainMenu;
	}

	private void BackToMainMenu()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		SceneManager.LoadScene("MainMenu");
	}
}
