using UnityEngine;

public class ReviveCheckpoint : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().ResetPositionToCheckpoint();
        }
	}
}
