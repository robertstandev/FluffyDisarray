﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D) , typeof(SpriteRenderer))]
public class OnCollisionTransitionHide : MonoBehaviour
{
    [SerializeField]private float hideTransitionStartAfter = 0f;
    [SerializeField]private float hideTransitionSpeed = 10f;
    [SerializeField]private float showTransitionStartAfter = 10f;
    [SerializeField]private float showTransitionSpeed = 20f;
    private MapCharacterManager getCharactersFromSceneScript;
    private BoxCollider2D boxCollider2DComponent;
    private SpriteRenderer spriteRendererComponent;
    private float originalSpriteRendererAlpha;
    private bool alreadyActivated = false;
    private float progress = 0f;

    private void Awake()
    {
        this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>();
        this.boxCollider2DComponent = GetComponent<BoxCollider2D>();
        this.spriteRendererComponent = GetComponent<SpriteRenderer>();
        this.originalSpriteRendererAlpha = this.spriteRendererComponent.color.a;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        this.spriteRendererComponent.color = new Color(this.spriteRendererComponent.color.r,this.spriteRendererComponent.color.g,this.spriteRendererComponent.color.b,this.originalSpriteRendererAlpha);
        this.boxCollider2DComponent.enabled = true;
        this.alreadyActivated = false;
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

        this.progress = 0f;

        while(this.spriteRendererComponent.color.a != alphaToChangeTo)
        {
            this.progress += Time.deltaTime * (this.progress <= 0.003f ? transitionSpeed / 10000 : 1f);

            this.spriteRendererComponent.color = new Color(this.spriteRendererComponent.color.r, this.spriteRendererComponent.color.g, this.spriteRendererComponent.color.b ,Mathf.Lerp(this.spriteRendererComponent.color.a, alphaToChangeTo, this.progress));
            yield return null;
        }
    }

    private IEnumerator visibilityManagerTimer()
    {
        yield return StartCoroutine(alphaTransitionTimer(0.1f, this.hideTransitionStartAfter, this.hideTransitionSpeed));
        this.boxCollider2DComponent.enabled = false;
        yield return StartCoroutine(alphaTransitionTimer(1f, this.showTransitionStartAfter, this.showTransitionSpeed));
        this.boxCollider2DComponent.enabled = true;
        this.alreadyActivated = false;
    }
}
