using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleLookAway : MonoBehaviour
{
    [SerializeField]private float duration;
    [SerializeField]private GameObject lookAwayEffectPrefab;
    private List<GameObject> instantiatedLookAwayEffects = new List<GameObject>();
}
