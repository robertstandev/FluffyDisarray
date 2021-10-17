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

    public InputAction getMovementInput { get { return this.movementInput; } }
    public InputAction getJumpInput { get { return this.jumpInput; } }
    public InputAction getUpInput { get { return this.upInput; } }
    public InputAction getDownInput { get { return this.downInput; } }
    public InputAction getProjectileInput { get { return this.projectileInput; } }
    public InputAction getSlashInput { get { return this.slashInput; } }

}