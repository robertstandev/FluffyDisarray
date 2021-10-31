using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    void setInputManager(PlayerInputManager inputManager);
    SpriteRenderer getCharacterRenderer { get; }
    void disableController();
    void enableController();
    void setMenu(GameObject menuToSet);
}
