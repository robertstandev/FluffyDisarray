using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    SpriteRenderer getCharacterRenderer { get; }
    void disableController();
    void enableController();
}
