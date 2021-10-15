using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashTrigger : MonoBehaviour
{
    [SerializeField]private GameObject slashEffectPrefab;
    [SerializeField]private int slashDamage = 10;
    [SerializeField]private Vector2 slashEffectPositonToCharacter;
    private GameObject instantiatedSlashEffect;
    private SpriteRenderer characterSpriteRenderer;

    private void Start()
    {
        instantiateSlashEffect();
        this.characterSpriteRenderer = GetComponent<IController>().getCharacterRenderer;
    }

    private void instantiateSlashEffect()
    {
        this.instantiatedSlashEffect = Instantiate(this.slashEffectPrefab , Vector3.zero , Quaternion.identity);
        this.instantiatedSlashEffect.transform.parent = this.gameObject.transform;
        this.instantiatedSlashEffect.transform.localPosition = this.slashEffectPositonToCharacter;
        this.instantiatedSlashEffect.GetComponent<SlashEffect>().setDamage(this.slashDamage);
    }

    public void executeSkill()
    {
        if(!this.instantiatedSlashEffect.activeInHierarchy)
        {
            adjustSlashEffectDirection();
            this.instantiatedSlashEffect.SetActive(true);
        }
    }

    private void adjustSlashEffectDirection() { this.instantiatedSlashEffect.transform.localScale = new Vector2(this.characterSpriteRenderer.flipX ? -1 : 1, 1f); }
}
