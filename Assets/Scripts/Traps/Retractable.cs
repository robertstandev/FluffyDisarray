using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retractable : MonoBehaviour
{
    [SerializeField]private float retractedPosition;
    [SerializeField]private float extendedPosition;
    [SerializeField]private float incrementBy = 0.1f;
    [SerializeField]private float executionSpeed = 0.25f;
    [SerializeField]private float executionDelay = 2f;
    private bool isRetracting = true;
    private Vector3 newGameObjectLocalPosition;
    private void OnEnable()
    {
        StartCoroutine(retractableTimer());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator retractableTimer()
    {
        WaitForSeconds timerExecutionSpeed = new WaitForSeconds(this.executionSpeed);
        WaitForSeconds timerExecutionDelay = new WaitForSeconds(this.executionDelay);

        this.newGameObjectLocalPosition = this.transform.localPosition;

        while(true)
        {
            yield return timerExecutionSpeed;

            this.newGameObjectLocalPosition.y += this.isRetracting ? -this.incrementBy : this.incrementBy;
            this.gameObject.transform.localPosition = this.newGameObjectLocalPosition;
            


            if(this.newGameObjectLocalPosition.y <= this.retractedPosition && this.isRetracting)
            {
                this.isRetracting = false;
                yield return timerExecutionDelay;
            }
            else if(this.newGameObjectLocalPosition.y >= this.extendedPosition && !this.isRetracting)
            {
                this.isRetracting = true;
                yield return timerExecutionDelay;
            }
        }
    }
}
