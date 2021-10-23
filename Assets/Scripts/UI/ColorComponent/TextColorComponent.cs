using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextColorComponent : MonoBehaviour, IColorComponent
{
    public Component getColorComponent { get { return this.GetComponent<Text>(); } }
}
