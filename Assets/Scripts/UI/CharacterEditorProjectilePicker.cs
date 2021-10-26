using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterEditorProjectilePicker : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private GameObject[] projectilePrefab;
    [SerializeField]private Sprite[] projectileThumbnail;
    private Image imageComponent;
    private GameObject selectedProjectilePrefab;
    private int currentSelectedProjectileIndex = 0;

    private void Awake() { this.imageComponent = GetComponent<Image>(); }

    public void OnPointerDown(PointerEventData eventData) { changeProjectile(); }
    public GameObject getProjectilePrefab() { return this.selectedProjectilePrefab; }

    private void changeProjectile()
    {
        selectNextProjectile();
        updateImageThumbnail();
        Debug.Log(this.currentSelectedProjectileIndex);
    }

    private void selectNextProjectile()
    {
        for(int i = 0 ; i < this.projectilePrefab.Length ; i ++)
        {
            if(i.Equals(this.currentSelectedProjectileIndex))
            {
                this.currentSelectedProjectileIndex = i.Equals(this.projectilePrefab.Length - 1) ? 0 : i + 1;
                this.selectedProjectilePrefab = this.projectilePrefab[this.currentSelectedProjectileIndex];
                break;
            }
        }
    }

    private void updateImageThumbnail() { this.imageComponent.sprite = this.projectileThumbnail[this.currentSelectedProjectileIndex]; }
}
