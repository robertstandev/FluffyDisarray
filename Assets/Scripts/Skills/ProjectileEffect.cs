using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    [SerializeField]private GameObject projectileEffect;
    [SerializeField]private int projectileDamage = 50;
    [SerializeField]private GameObject impactEffect;
    private SpriteRenderer characterSpriteRenderer;
    private Vector2 startLocalPosition = new Vector2(1.5f, 0f);
    private Vector2 currentModifiedPosition = Vector2.zero; 
    private WaitForSeconds wait = new WaitForSeconds(0.003f);
    private float moveDirection = 0.1f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        this.projectileEffect.SetActive(false);
        this.impactEffect.transform.position = this.projectileEffect.transform.position;
        this.impactEffect.SetActive(true);
        other.gameObject.GetComponent<Health>()?.substractHealth(this.projectileDamage);
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
        this.projectileEffect.SetActive(true);
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

    public void setCharacterSpriteRenderer(SpriteRenderer charSpriteRenderer) { this.characterSpriteRenderer = charSpriteRenderer; }
    public void instantiateImpactEffect() { this.impactEffect = Instantiate(this.impactEffect , Vector3.zero , Quaternion.identity); }
}
