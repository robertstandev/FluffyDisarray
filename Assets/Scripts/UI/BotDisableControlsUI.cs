using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotDisableControlsUI : MonoBehaviour
{
    private Text textComponent;
    private GameObject gameObjectToShowHide;

    public void checkTextComponent()//send command to check from continue button and arrow buttons
    {
        if(this.textComponent.text.Equals("Bot"))
        {
            this.gameObjectToShowHide.SetActive(false);
        }
        else
        {
            this.gameObjectToShowHide.SetActive(true);
        }
    }
}
