using UnityEngine;

//Biar terpisah dari player movement karena cursor bukan bagian dari player movement.
//Aneh saya liatnya wkwk
//Buat misal suatu saat nanti pengen enable cursor buat alasan apapun, ga bingung carinya.
public class CursorManager : MonoBehaviour
{
	private void Awake()
	{
		HideAndLockCursor();
	}
	
	private void HideAndLockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
