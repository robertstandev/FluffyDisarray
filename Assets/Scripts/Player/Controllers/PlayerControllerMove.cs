using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerMove : MonoBehaviour
{
    private InputAction movementInput;
    private Movement movementComponent;

    private void Awake() {
        movementInput = GetComponent<IPlayerInput>().getMovementInput;
        movementComponent = GetComponent<Movement>();
    }
}
