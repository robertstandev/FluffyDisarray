using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]private InputAction movementInput;
    [SerializeField]private InputAction upInput;
    [SerializeField]private InputAction downInput;
    [SerializeField]private InputAction jumpInput;
    [SerializeField]private InputAction projectileInput;
    [SerializeField]private InputAction slashInput;
    [SerializeField]private InputAction menuInput;

    public InputAction getMovementInput { get { return this.movementInput; } }
    public InputAction getJumpInput { get { return this.jumpInput; } }
    public InputAction getUpInput { get { return this.upInput; } }
    public InputAction getDownInput { get { return this.downInput; } }
    public InputAction getProjectileInput { get { return this.projectileInput; } }
    public InputAction getSlashInput { get { return this.slashInput; } }
    public InputAction getMenuInput { get { return this.menuInput; } }

    public void setMovementInput(InputAction inputAction) { this.movementInput = inputAction; }
    public void setUpInput(InputAction inputAction) { this.upInput = inputAction; }
    public void setDownInput(InputAction inputAction) { this.downInput = inputAction; }
    public void setJumpInput(InputAction inputAction) { this.jumpInput = inputAction; }
    public void setProjectileInput(InputAction inputAction) { this.projectileInput = inputAction; }
    public void setSlashInput(InputAction inputAction) { this.slashInput = inputAction; }
    public void setMenuInput(InputAction inputAction) { this.menuInput.AddBinding(inputAction.bindings[0]); }

    public void setInputKeys(PlayerInputManager playerInputManagerScript)
    {
        // this.movementInput = playerInputManagerScript.getMovementInput;
        // this.upInput = playerInputManagerScript.getUpInput;
        // this.downInput = playerInputManagerScript.downInput;
        // this.jumpInput = playerInputManagerScript.jumpInput;
        // this.projectileInput = playerInputManagerScript.projectileInput;
        // this.slashInput = playerInputManagerScript.slashInput;
        //this.menuInput.AddBinding(playerInputManagerScript.menuInput.bindings[0]);
    }
}