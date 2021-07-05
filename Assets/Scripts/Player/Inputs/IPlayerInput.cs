﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerInput
{
    InputAction getMovementInput { get; }
    InputAction getJumpInput { get; }
    InputAction getUpInput { get; }
    InputAction getDownInput { get; }
}