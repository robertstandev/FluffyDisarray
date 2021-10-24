using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotDisableControlsUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private Text textComponent;
    [SerializeField]private GameObject controlsGroupGameObject;

    public void OnPointerDown(PointerEventData eventData) { checkTextComponentAndShowHideControlsUI(); }

    private void checkTextComponentAndShowHideControlsUI() { this.controlsGroupGameObject.SetActive(!this.textComponent.text.Equals("Bot")); }
}
