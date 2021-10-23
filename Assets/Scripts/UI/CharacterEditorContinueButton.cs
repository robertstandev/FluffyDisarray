using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterEditorContinueButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private MapCharacterManager mapCharacterManagerGameObject;
    [SerializeField]private Text characterTypeText;
    [SerializeField]private Image characterColorImage;
    [SerializeField]private CharacterEditorKeyBindingManager keyBindingManager;
    public void OnPointerDown(PointerEventData eventData)
    {
       Destroy(GetComponent<HideShowTouch>());
       //this.mapCharacterManagerGameObject.setCharacterDetails
    }
}
