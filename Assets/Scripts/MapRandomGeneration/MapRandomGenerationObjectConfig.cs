using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRandomGenerationObjectConfig : MonoBehaviour
{
    public enum partTemplate { left, mid, right }
    [SerializeField]private partTemplate objectPart = partTemplate.mid;

    [SerializeField]private Vector2 objectSize = Vector2.zero;
    [SerializeField]private bool canAddEmptySpaceAfter = false;
    [SerializeField]private bool canAddHigherObjectAfter = true;
    [SerializeField]private bool canAddLowerObjectAfter = true;

    public enum sortTemplate { onlyBackground, onlyForeground, bothBackgroundAndForeground }
    [SerializeField]private sortTemplate objectSort = sortTemplate.onlyForeground;

    public partTemplate getObjectPart() { return this.objectPart; }
    public Vector2 getObjectSize() { return this.objectSize; }
    public bool canAddEmptySpaceAfterValue() { return this.canAddEmptySpaceAfter; }
    public bool canAddHigherObjectAfterValue() { return this.canAddHigherObjectAfter; }
    public bool canAddLowerObjectAfterValue() { return this.canAddLowerObjectAfter; }
    public sortTemplate getObjectSort() { return this.objectSort; }
}
