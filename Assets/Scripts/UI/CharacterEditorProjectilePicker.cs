using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditorProjectilePicker : MonoBehaviour
{
    private GameObject selectedProjectilePrefab;
    public GameObject getProjectilePrefab() { return this.selectedProjectilePrefab; }
}
