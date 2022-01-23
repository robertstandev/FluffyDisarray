using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryRandomMap : MonoBehaviour
{
    [SerializeField]private GameObject tutorialPrefab, bossInsidePrefab, bossOutsidePrefab;
    [SerializeField]private GameObject[] insidePrefabs, outsidePrefabs;

    private void Start() { Instantiate(tutorialPrefab, Vector3.zero, Quaternion.identity); }
}
