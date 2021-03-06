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

    private void OnEnable()
    {
        this.swingingLeftDirection = true;
        StartCoroutine(swingMovementTimer());
    }
    private void OnDisable() { StopAllCoroutines(); }

    private IEnumerator swingMovementTimer()
    {
        WaitForSeconds timerWait = new WaitForSeconds(this.swingMovementDelay);
        Vector3 gameObjectRotation = this.gameObject.transform.localEulerAngles;
        this.currentRotation = gameObjectRotation.z;

        while(true)
        {
            yield return timerWait;

            this.currentRotation += this.swingingLeftDirection ? -1f : 1f;
            gameObjectRotation.z = this.currentRotation;
            this.gameObject.transform.localEulerAngles = gameObjectRotation;
            this.swingingLeftDirection = this.currentRotation <= this.minRotation ? false : this.currentRotation >= this.maxRotation ? true : this.swingingLeftDirection;
        }
    } 
}
