using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    [SerializeField]private int projectileDamage = 50;
    [SerializeField]private GameObject projectileEffectPrefab;
    [SerializeField]private Vector2 projectilePositionOffset;
    [SerializeField]private GameObject muzzleEffectPrefab;
    [SerializeField]private Vector2 muzzlePositionOffset;

    private GameObject instantiatedMuzzleEffect, instantiatedprojectileEffect;
    private SpriteRenderer characterSpriteRenderer;
    
    private void Start()
    {
        this.characterSpriteRenderer = GetComponent<IController>().getCharacterRenderer;
        instantiateMuzzleEffect();
        instantiateProjectileEffect();
        this.instantiatedprojectileEffect.GetComponent<ProjectileEffect>().setCharacterSpriteRenderer(this.characterSpriteRenderer);
        this.instantiatedprojectileEffect.GetComponent<ProjectileEffect>().instantiateImpactEffect();
    }
    public void executeSkill()
    {
        if(!this.instantiatedprojectileEffect.activeInHierarchy)
        {
            this.instantiatedMuzzleEffect.transform.position = new Vector2(this.transform.position.x + (this.characterSpriteRenderer.flipX ? -this.muzzlePositionOffset.x : this.muzzlePositionOffset.x) , this.transform.position.y + this.muzzlePositionOffset.y);
            this.instantiatedMuzzleEffect.SetActive(true);
            this.instantiatedprojectileEffect.SetActive(true);
        }
    }

    private void instantiateMuzzleEffect() { this.instantiatedMuzzleEffect = Instantiate(this.muzzleEffectPrefab , Vector3.zero , Quaternion.identity); }
    private void instantiateProjectileEffect()
    {
        this.instantiatedprojectileEffect = Instantiate(this.projectileEffectPrefab , Vector3.zero , Quaternion.identity);
        this.instantiatedprojectileEffect.GetComponent<ProjectileEffect>().setProjectilePositionOffset(this.projectilePositionOffset);
        this.instantiatedprojectileEffect.GetComponent<ProjectileEffect>().setProjectileDamage(this.projectileDamage);
    }
}
