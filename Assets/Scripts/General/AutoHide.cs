using UnityEngine;
using System.Collections;

public class AutoHide : MonoBehaviour
{
	[SerializeField]private float hideAfterInterval = 0.8f;
	private void OnEnable() { Invoke("OnDisable", hideAfterInterval); }
	private void OnDisable() { this.gameObject.SetActive(false); }
}
