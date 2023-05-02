using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	[SerializeField]private GameObject objectToFollow;
	[SerializeField]private float followSpeed = 0.3f;

	public void LateUpdate()
	{
		if(!this.objectToFollow) { return; }
		transform.position = Vector3.Lerp(transform.position, this.objectToFollow.transform.position, this.followSpeed) + new Vector3(0, 0, -12);
	}

	public GameObject getObjectToFollow() { return this.objectToFollow; }
	public void setObjectToFollow(GameObject setGameObject) { this.objectToFollow = setGameObject; }
}
