using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]private int duration = 10;
    [SerializeField]private float power = 0.7f;
    [SerializeField]private Transform myCamera;
   
   private Vector3 startPosition;
   private int currentDuration = 10;
   private WaitForSeconds timerWait = new WaitForSeconds(0.025f);

   private void Start()
   {
        this.myCamera = this.transform;
        this.startPosition = myCamera.localPosition;
        resetCurrentDuration();
   }

    private void OnEnable() { shakeCamera(); }
    private void OnDisable()
    {
        StopAllCoroutines();
        resetCurrentDuration();
        resetPosition();
    }

   public void shakeCamera() { StartCoroutine("shakeCameraTimer"); }

    private IEnumerator shakeCameraTimer()
    {
        while(this.currentDuration > 0)
        {
            yield return timerWait;
            
            this.myCamera.localPosition = this.startPosition + Random.insideUnitSphere * this.power;
            this.currentDuration -= 1;
        }
        this.enabled = false;
    }

    private void resetCurrentDuration() { this.currentDuration = this.duration * 40; }
    private void resetPosition() { this.myCamera.localPosition = this.startPosition; }

    public void setDuration(int value) { this.duration = value; }
}
