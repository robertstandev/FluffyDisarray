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
    private WaitForSeconds timerWait = new WaitForSeconds(0.1f);
    private int effectTimer = 0;

    private bool isCharacterLookingTowardsTheTarget;
    private Color32 originalColor, activeColor = new Color32(255,0,0, 255);

    private void Awake()
    {
        Transform temporaryCharacterTransform = transform.parent;
        this.characterSpriteRenderer = temporaryCharacterTransform.GetComponent<IController>().getCharacterRenderer;
        this.characterHealthComponent = temporaryCharacterTransform.GetComponent<Health>();

        this.effectSpriteRenderer = GetComponent<SpriteRenderer>();
        this.originalColor = this.effectSpriteRenderer.color;
    }

    public void setTarget(Transform targetToPut)
    {
        this.target = targetToPut;
        this.targetSpriteRenderer = this.target.GetComponent<IController>().getCharacterRenderer;
    }

    private void OnEnable()
    {
        this.effectSpriteRenderer.color = this.originalColor;
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
            checkIfCharacterIsLookingTowardsTheTarget();
            this.effectTimer -= 1;
            this.effectSpriteRenderer.color = this.isCharacterLookingTowardsTheTarget ? this.activeColor : this.originalColor;

            if(this.isCharacterLookingTowardsTheTarget && (this.effectTimer <= 0))
            {
                this.characterHealthComponent.substractHealth(20);
                this.effectTimer = 10;
            }
        }
    }
    private void checkIfCharacterIsLookingTowardsTheTarget()
    {
        this.isCharacterLookingTowardsTheTarget = (((transform.position.x > this.target.position.x) && this.characterSpriteRenderer.flipX && !this.targetSpriteRenderer.flipX) || ((transform.position.x < this.target.position.x) && !this.characterSpriteRenderer.flipX && this.targetSpriteRenderer.flipX));
    }

}
