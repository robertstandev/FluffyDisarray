using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D) , typeof(SpriteRenderer))]
public class OnCollisionTransitionHide : MonoBehaviour
{
    [SerializeField]private float hideAfterInterval = 3f;
    [SerializeField]private float showAfterInterval = 10f;
    private MapCharacterManager getCharactersFromSceneScript;
    private BoxCollider2D boxCollider2DComponent;
    private SpriteRenderer spriteRendererComponent;
    private float originalSpriteRendererAlpha;
    private bool alreadyActivated = false;
   

    private void Awake()
    {
        this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>();
        this.boxCollider2DComponent = GetComponent<BoxCollider2D>();
        this.spriteRendererComponent = GetComponent<SpriteRenderer>();
        this.originalSpriteRendererAlpha = this.spriteRendererComponent.color.a;
    }

    private void OnDisable()
    {
        this.spriteRendererComponent.color = new Color(this.spriteRendererComponent.color.r,this.spriteRendererComponent.color.g,this.spriteRendererComponent.color.b,this.originalSpriteRendererAlpha);
        StopAllCoroutines();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(this.getCharactersFromSceneScript.isCollidedObjectInList(other.gameObject) && !this.alreadyActivated)
        {
            this.alreadyActivated = true;
            StartCoroutine(visibilityManagerTimer());
        }
    }

    private IEnumerator alphaTransitionTimer(float alphaToChangeTo, float startDelay, float transitionSpeed)
    {
        yield return new WaitForSeconds(startDelay);

        float progress = 0f;
        while(this.spriteRendererComponent.color.a != alphaToChangeTo)
        {
            progress += Time.deltaTime / (progress <= 0.008f ? transitionSpeed : 1f);

            this.spriteRendererComponent.color = new Color(this.spriteRendererComponent.color.r, this.spriteRendererComponent.color.g, this.spriteRendererComponent.color.b ,Mathf.Lerp(this.spriteRendererComponent.color.a, alphaToChangeTo, progress));
            yield return null;
        }
    }

    private IEnumerator visibilityManagerTimer()
    {
        yield return StartCoroutine(alphaTransitionTimer(0.1f, this.hideAfterInterval, 800f));
        this.boxCollider2DComponent.enabled = false;
        yield return StartCoroutine(alphaTransitionTimer(1f, this.showAfterInterval, 300f));
        this.boxCollider2DComponent.enabled = true;
        this.alreadyActivated = false;
    }
}
