using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSwitchRevert : MonoBehaviour
{
    private Camera cameraComponent;
    private int duration;
    private WaitForSeconds timerWait = new WaitForSeconds(1f);

    private void Awake()
    {
        this.cameraComponent = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        invertCamera();
        StartCoroutine("switchRevertTimer");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        revertCamera();
    }

    private void invertCamera() { cameraComponent.transform.localRotation = Quaternion.Euler(0f,0f,180f); }
    private void revertCamera() { cameraComponent.transform.localRotation = Quaternion.identity; }
    public void setDuration(int value) { this.duration = value; }

    private IEnumerator switchRevertTimer()
    {
        while(this.duration > 0)
        {
            yield return timerWait;
            this.duration -= 1;
        }
        this.enabled = false;
    }
    

}
