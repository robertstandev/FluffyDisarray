using UnityEngine;
using System.Collections;

public class AutoHide : MonoBehaviour
{
	[SerializeField]private float hideAfterInterval = 0.8f;
	private void OnEnable() { Invoke("OnDisable", hideAfterInterval); }
	private void OnDisable() { CancelInvoke(); this.gameObject.SetActive(false); }
}
