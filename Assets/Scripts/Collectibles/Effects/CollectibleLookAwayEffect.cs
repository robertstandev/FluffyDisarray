using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLookAwayEffect : MonoBehaviour
{
    private SpriteRenderer characterSpriteRenderer;
    private Health characterHealthComponent;
    private SpriteRenderer effectSpriteRenderer;
    private Transform target;
    private SpriteRenderer targetSpriteRenderer;
    private WaitForSeconds timerWait = new WaitForSeconds(1f);

    private void Awake()
    {
        Transform temporaryCharacterTransform = transform.parent;
        this.characterSpriteRenderer = temporaryCharacterTransform.GetComponent<IController>().getCharacterRenderer;
        this.characterHealthComponent = temporaryCharacterTransform.GetComponent<Health>();

        this.effectSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setTarget(Transform targetToPut)
    {
        this.target = targetToPut;
        this.targetSpriteRenderer = this.target.GetComponent<IController>().getCharacterRenderer;
    }

    private void OnEnable()
    {
        this.effectSpriteRenderer.enabled = false;
        StartCoroutine("checkFacingDirectionTimer");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    private IEnumerator checkFacingDirectionTimer()
    {
        while(true)
        {
            yield return timerWait;
            this.effectSpriteRenderer.enabled = isCharacterLookingTowardsTheTarget();
            
            if(this.effectSpriteRenderer.enabled)
            {
                this.characterHealthComponent.substractHealth(20);
            }
        }
    }
    private bool isCharacterLookingTowardsTheTarget()
    {
        return (((transform.position.x > this.target.position.x) && this.characterSpriteRenderer.flipX && !this.targetSpriteRenderer.flipX) || ((transform.position.x < this.target.position.x) && !this.characterSpriteRenderer.flipX && this.targetSpriteRenderer.flipX));
    }

}
