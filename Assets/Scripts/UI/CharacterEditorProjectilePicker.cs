using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterEditorProjectilePicker : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private GameObject[] projectilePrefab;
    [SerializeField]private Vector2[] projectilePositionOffset;
    [SerializeField]private GameObject[] projectileMuzzleEffectPrefab;
    [SerializeField]private Vector2[] projectileMuzzlePositionOffset;
    [SerializeField]private Sprite[] projectileThumbnail;
    private Image imageComponent;
    private int currentSelectedProjectileIndex = 0;

    private void Awake() { this.imageComponent = GetComponent<Image>(); }


    public void OnPointerDown(PointerEventData eventData) { changeProjectile(); }
    public GameObject getProjectilePrefab() { return this.projectilePrefab[this.currentSelectedProjectileIndex]; }
    public Vector2 getProjectilePositionOffset() { return this.projectilePositionOffset[this.currentSelectedProjectileIndex]; }
    public GameObject getProjectileMuzzleEffectPrefab() { return this.projectileMuzzleEffectPrefab[this.currentSelectedProjectileIndex]; }
    public Vector2 getProjectileMuzzlePositionOffset() { return this.projectileMuzzlePositionOffset[this.currentSelectedProjectileIndex]; }


    private void changeProjectile()
    {
        selectNextProjectile();
        updateImageThumbnail();
    }

    private void selectNextProjectile()
    {
        for(int i = 0 ; i < this.projectilePrefab.Length ; i ++)
        {
            if(i.Equals(this.currentSelectedProjectileIndex))
            {
                this.currentSelectedProjectileIndex = i.Equals(this.projectilePrefab.Length - 1) ? 0 : i + 1;
                break;
            }
        }
    }

    private void updateImageThumbnail() { this.imageComponent.sprite = this.projectileThumbnail[this.currentSelectedProjectileIndex]; }
}
