using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnvironmentDataStorage
{
    private List<GameObject> spriteInstances = new List<GameObject>();
    public List<GameObject> getSpriteInstancesList() { return this.spriteInstances; }
}
