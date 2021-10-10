using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    [SerializeField]private GameObject muzzleEffectPrefab;
    [SerializeField]private Vector2 muzzleEffectPositionToCharacter;
    private SpriteRenderer characterSpriteRenderer;
    [SerializeField]private GameObject projectileEffectPrefab;

    private void Awake()
    {
        this.characterSpriteRenderer = GetComponent<IController>().getCharacterRenderer;
        instantiateMuzzleEffect();
        instantiateProjectileEffect();
        this.projectileEffectPrefab.GetComponent<ProjectileEffect>().setCharacterSpriteRenderer(this.characterSpriteRenderer);
        this.projectileEffectPrefab.GetComponent<ProjectileEffect>().instantiateImpactEffect();
    }
    public void executeSkill()
    {
        if(!projectileEffectPrefab.activeInHierarchy)
        {
            this.muzzleEffectPrefab.transform.position = new Vector2(this.transform.position.x + (this.characterSpriteRenderer.flipX ? -this.muzzleEffectPositionToCharacter.x : this.muzzleEffectPositionToCharacter.x) , this.transform.position.y + this.muzzleEffectPositionToCharacter.y);
            this.muzzleEffectPrefab.SetActive(true);
            this.projectileEffectPrefab.SetActive(true);
        }
    }

    private void instantiateMuzzleEffect() { this.muzzleEffectPrefab = Instantiate(this.muzzleEffectPrefab , Vector3.zero , Quaternion.identity); }
    private void instantiateProjectileEffect() { this.projectileEffectPrefab = Instantiate(this.projectileEffectPrefab , Vector3.zero , Quaternion.identity); }
}
