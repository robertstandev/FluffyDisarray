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
    [SerializeField]private Sprite[] projectileThumbnail;
    private Image imageComponent;
    private GameObject selectedProjectilePrefab, selectetProjectileMuzzleEffectPrefab;
    private Vector2 selectedProjectilePositionOffset;
    private int currentSelectedProjectileIndex = 0;

    private void Awake() { this.imageComponent = GetComponent<Image>(); }
    private void Start()
    {
        this.selectedProjectilePrefab = this.projectilePrefab[0];
        this.selectedProjectilePositionOffset = this.projectilePositionOffset[0];
        this.selectetProjectileMuzzleEffectPrefab = this.projectileMuzzleEffectPrefab[0];
    }

    public void OnPointerDown(PointerEventData eventData) { changeProjectile(); }
    public GameObject getProjectilePrefab() { return this.selectedProjectilePrefab; }
    public Vector2 getProjectilePositionOffset() { return this.selectedProjectilePositionOffset; }
    public GameObject getProjectileMuzzleEffectPrefab() { return this.selectetProjectileMuzzleEffectPrefab; }


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
                this.selectedProjectilePrefab = this.projectilePrefab[this.currentSelectedProjectileIndex];
                this.selectedProjectilePositionOffset = this.projectilePositionOffset[this.currentSelectedProjectileIndex];
                this.selectetProjectileMuzzleEffectPrefab = this.projectileMuzzleEffectPrefab[this.currentSelectedProjectileIndex];
                break;
            }
        }
    }

    private void updateImageThumbnail() { this.imageComponent.sprite = this.projectileThumbnail[this.currentSelectedProjectileIndex]; }
}
