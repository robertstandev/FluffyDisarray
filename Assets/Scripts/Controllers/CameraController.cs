using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	[SerializeField]private Transform objectToFollow;
	[SerializeField]private float followSpeed = 0.3f;

	//public void OnEnable() { GetComponent<Camera>().orthographicSize = (Screen.height / 100f) / 0.7f; }

	public void LateUpdate()
	{
		if(!objectToFollow) { return; }
		transform.position = Vector3.Lerp(transform.position, objectToFollow.position, followSpeed) + new Vector3(0, 0, -12);
	}
}
