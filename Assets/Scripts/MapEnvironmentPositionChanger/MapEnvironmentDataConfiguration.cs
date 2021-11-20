using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public struct MapEnvironmentDataConfiguration
    {
        [SerializeField]private GameObject spritePrefab;
        [SerializeField]private int instances;
        [SerializeField]private Vector3[] spritePositions;

        public GameObject getSpritePrefab() { return this.spritePrefab; }
        public int getInstancesNumber() { return this.instances; }
        public Vector3[] getSpritePositions() { return this.spritePositions; }
    }