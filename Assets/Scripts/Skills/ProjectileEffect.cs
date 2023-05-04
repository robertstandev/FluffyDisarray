using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    [SerializeField]private GameObject projectileEffectGroup;
    [SerializeField]private GameObject impactEffect;

    private GameObject instantiatedImpactEffect;
    private int projectileDamage;

    private SpriteRenderer characterSpriteRenderer;
    private Vector2 startLocalPosition;
    private Vector2 currentModifiedPosition = Vector2.zero; 
    private WaitForSeconds wait = new WaitForSeconds(0.003f);
    private float moveDirection;

    private void OnCollisionEnter2D(Collision2D other)
    {
        this.projectileEffectGroup.SetActive(false);
        this.instantiatedImpactEffect.transform.position = this.projectileEffectGroup.transform.position;
        this.instantiatedImpactEffect.SetActive(true);
        other.gameObject.GetComponent<Health>()?.substractHealth(this.projectileDamage);
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = new Vector2(this.characterSpriteRenderer.transform.position.x + (this.characterSpriteRenderer.flipX ? -this.startLocalPosition.x : this.startLocalPosition.x), this.characterSpriteRenderer.transform.position.y + this.startLocalPosition.y);
        this.currentModifiedPosition = this.transform.position;

        this.moveDirection = this.characterSpriteRenderer.flipX ? -0.1f : 0.1f;

        StartCoroutine("executeProjectile");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        this.projectileEffectGroup.SetActive(true);
    }

    private IEnumerator executeProjectile()
    {
        while(true)
        {
            yield return this.wait;
            this.currentModifiedPosition.x += this.moveDirection;
            this.transform.position = this.currentModifiedPosition;
        }
    }

    public void setProjectileDamage(int value) { this.projectileDamage = value; }
    public void setCharacterSpriteRenderer(SpriteRenderer charSpriteRenderer) { this.characterSpriteRenderer = charSpriteRenderer; }
    public void setProjectilePositionOffset(Vector2 value) { this.startLocalPosition = value; }
    public void instantiateImpactEffect() { this.instantiatedImpactEffect = Instantiate(this.impactEffect , Vector3.zero , Quaternion.identity); }
}
