using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileImpactEffect : MonoBehaviour
{
    [SerializeField]private SpriteRenderer characterSpriteRenderer;
    [SerializeField]private GameObject projectileParticleGameObject;
    [SerializeField]private GameObject impactEffect;
    
    private Vector2 startLocalPosition = new Vector2(1.5f, 0f);
    private Vector2 currentModifiedPosition = Vector2.zero; 
    private WaitForSeconds wait = new WaitForSeconds(0.005f);
    private float moveDirection = 0.1f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        this.projectileParticleGameObject.SetActive(false);
        this.impactEffect.transform.position = this.projectileParticleGameObject.transform.position;
        this.impactEffect.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = new Vector2(this.characterSpriteRenderer.transform.position.x + (characterSpriteRenderer.flipX ? -this.startLocalPosition.x : this.startLocalPosition.x), this.characterSpriteRenderer.transform.position.y + this.startLocalPosition.y);
        this.currentModifiedPosition = this.transform.position;

        this.moveDirection = characterSpriteRenderer.flipX ? -0.1f : 0.1f;

        StartCoroutine("executeProjectile");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        this.projectileParticleGameObject.SetActive(true);
    }

    private IEnumerator executeProjectile()
    {
        while(true)
        {
            yield return wait;
            this.currentModifiedPosition.x += this.moveDirection;
            this.transform.position = this.currentModifiedPosition;
        }
    }
}
