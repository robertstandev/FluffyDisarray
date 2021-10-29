using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterEditorKeyBindingManager : MonoBehaviour
{
    [SerializeField]private InputAction movementInput;
    [SerializeField]private InputAction upInput;
    [SerializeField]private InputAction downInput;
    [SerializeField]private InputAction jumpInput;
    [SerializeField]private InputAction projectileInput;
    [SerializeField]private InputAction slashInput;
    [SerializeField]private InputAction menuInput;

    public void setMovementInput(InputAction inputAction) { this.movementInput = inputAction; }
    public void setUpInput(InputAction inputAction) { this.upInput = inputAction; }
    public void setDownInput(InputAction inputAction) { this.downInput = inputAction; }
    public void setJumpInput(InputAction inputAction) { this.jumpInput = inputAction; }
    public void setProjectileInput(InputAction inputAction) { this.projectileInput = inputAction; }
    public void setSlashInput(InputAction inputAction) { this.slashInput = inputAction; }
    public void setMenuInput(InputAction inputAction) { this.menuInput = inputAction; }
}
