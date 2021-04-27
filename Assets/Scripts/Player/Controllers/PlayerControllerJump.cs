using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Jump))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControllerJump : MonoBehaviour
{
    private InputAction jumpInput;
    private Jump jumpComponent;
    private Stamina staminaComponent;
    private Rigidbody2D rb;

    private void Awake() {
        jumpInput = GetComponent<IPlayerInput>().getJumpInput;
        jumpInput.performed += context => OnJump();

        jumpComponent = GetComponent<Jump>();
        staminaComponent = GetComponent<Stamina>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() { jumpInput.Enable(); }
    private void OnDisable() { jumpInput.Disable(); }

    private void OnJump() { jumpComponent.jump(rb, staminaComponent, 10); }
}
