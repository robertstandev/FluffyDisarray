using UnityEngine;
using System.Collections;

public class AutoHideTimer : MonoBehaviour
{
	[SerializeField]private float hideAfterInterval = 0.8f;
	private void OnEnable() { StartCoroutine("autoHideTimer"); }
	private void OnDisable() { StopAllCoroutines(); hideObject(); }

	private IEnumerator autoHideTimer()
	{
		yield return new WaitForSeconds(hideAfterInterval);
		hideObject();
	}
	private void hideObject() { this.gameObject.SetActive(false); }
	public void setDuration(float value) { this.hideAfterInterval = value; }
}
