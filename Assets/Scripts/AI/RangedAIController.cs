using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAIController : MonoBehaviour , IController
{
    private SpriteRenderer mySpriteRenderer;
    public SpriteRenderer getCharacterRenderer { get { return this.mySpriteRenderer; } }
    public void disableController() { this.enabled = false; }
    public void enableController() { this.enabled = true; }
    public void setMenu(GameObject menuToSet) {}
    public void setInputManager(PlayerInputManager inputManager) {}
}
