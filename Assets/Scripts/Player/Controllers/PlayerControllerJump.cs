using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Jump))]
public class PlayerControllerJump : MonoBehaviour
{
    private InputAction jumpInput;
    private Jump jumpComponent;
    private void Awake() {
        jumpInput = GetComponent<IPlayerInput>().getJumpInput;
        jumpComponent = GetComponent<Jump>();
    }
}
