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
    [SerializeField]private bool disableKillEffectWhenRetracting = true;
    private bool isRetracting = true;
    private BoxCollider2D killEffectCollider;
    private float halfDistanceBetweenExtendedPositionAndRetractedPosition;
    private Vector3 newGameObjectLocalPosition;

    private void Awake()
    {
        cacheKillingCollider();
        calculateHalfDistance(this.extendedPosition , this.retractedPosition);
    }
    private void OnEnable()
    {
        this.isRetracting = true;
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

            this.killEffectCollider.enabled = this.disableKillEffectWhenRetracting && this.newGameObjectLocalPosition.y < this.halfDistanceBetweenExtendedPositionAndRetractedPosition ? false : true;
        }
    }

    private void cacheKillingCollider()
    {
        BoxCollider2D[] tempColliders = GetComponents<BoxCollider2D>();
        for(int i = 0 ; i < tempColliders.Length; i++)
        {
            this.killEffectCollider = tempColliders[i].isTrigger ? tempColliders[i] : null;
        }
    }
    private void calculateHalfDistance(float topNumber , float bottomNumber)
    {
        this.halfDistanceBetweenExtendedPositionAndRetractedPosition = topNumber - ((topNumber - bottomNumber) / 2);
    }
}
