using UnityEngine;

public class SetCheckpoint : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerMovement>().SetCheckpoint(transform);
		}
	}
}