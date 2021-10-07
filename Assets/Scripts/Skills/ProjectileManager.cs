using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField]private GameObject muzzleEffectPrefab;
    [SerializeField]private Vector2 muzzleEffectPositionToCharacter;
    [SerializeField]private SpriteRenderer characterSpriteRenderer;
    [SerializeField]private GameObject projectileEffectPrefab;

    public void executeSkill()
    {
        if(!projectileEffectPrefab.activeInHierarchy)
        {
            this.muzzleEffectPrefab.transform.position = new Vector2(this.transform.position.x + (this.characterSpriteRenderer.flipX ? -this.muzzleEffectPositionToCharacter.x : this.muzzleEffectPositionToCharacter.x) , this.transform.position.y + this.muzzleEffectPositionToCharacter.y);
            this.muzzleEffectPrefab.SetActive(true);
            this.projectileEffectPrefab.SetActive(true);
        }
    }

}
