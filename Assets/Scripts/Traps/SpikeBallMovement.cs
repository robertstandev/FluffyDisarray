using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBallMovement : MonoBehaviour
{
    [SerializeField]private float minRotation = -20f;
    [SerializeField]private float maxRotation = 20f;
    [SerializeField]private float swingMovementDelay = 0.05f;
    private float currentRotation = 0f;
    private bool swingingLeftDirection = true;
    private Vector3 gameObjectRotation;
    private WaitForSeconds timerWait;

    private void OnEnable()
    {
        this.swingingLeftDirection = true;
        this.timerWait = new WaitForSeconds(this.swingMovementDelay);
        StartCoroutine(swingMovementTimer());
    }
    private void OnDisable() { StopAllCoroutines(); }

    private IEnumerator swingMovementTimer()
    {
        this.gameObjectRotation = this.gameObject.transform.localEulerAngles;
        this.currentRotation = this.gameObjectRotation.z;

        while(true)
        {
            yield return this.timerWait;

            this.currentRotation += this.swingingLeftDirection ? -1f : 1f;
            this.gameObjectRotation.z = this.currentRotation;
            this.gameObject.transform.localEulerAngles = this.gameObjectRotation;
            this.swingingLeftDirection = this.currentRotation <= this.minRotation ? false : this.currentRotation >= this.maxRotation ? true : this.swingingLeftDirection;
        }
    } 
}
