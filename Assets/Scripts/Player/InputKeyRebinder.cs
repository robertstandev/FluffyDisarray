using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputKeyRebinder : MonoBehaviour, IPointerDownHandler
{
    private enum typesOfInputs {Button , Axis};
    [SerializeField]private typesOfInputs typeOfInput = typesOfInputs.Button;
    [SerializeField]private CharacterEditorKeyBindingManager keyBindingManager;
    private enum inputList { movementInput, downInput, upInput, jumpInput, projectileInput, slashInput, menuInput};
    [SerializeField]private inputList inputToReplace = inputList.movementInput;
    [SerializeField]private Text textComponent;
    private InputAction actionInput;

    private string originalText;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void OnPointerDown(PointerEventData eventData) { getUserInput(); }

    private void getUserInput()
    {
        this.originalText  = this.textComponent.text;

        this.actionInput = new InputAction();

        if(this.typeOfInput.Equals(typesOfInputs.Button))
        {
            this.textComponent.text = "Press Key";
            configureButtonInput();
        }
        else
        {
            this.textComponent.text = "Press Key(s)";
            configureAxisInput();
        }
    }

    private void configureAxisInput()
    {
        this.actionInput.AddCompositeBinding("Axis") // Or just "Axis"
        .With("Positive", "A")
        .With("Negative", "A");

        this.rebindingOperation = actionInput.PerformInteractiveRebinding(0)
        .WithRebindAddingNewBinding()
        .WithControlsExcluding("Mouse")
        .OnMatchWaitForAnother(0.1f)
        .OnCancel(operation => rebindCanceled())
        .Start();

        this.rebindingOperation = actionInput.PerformInteractiveRebinding(1)
        .WithRebindAddingNewBinding()
        .WithControlsExcluding("Mouse")
        .OnMatchWaitForAnother(0.1f)
        .OnCancel(operation => rebindCanceled())
        .Start();
  
        rebindComplete();
    }

    private void configureButtonInput()
    {
        this.rebindingOperation = actionInput.PerformInteractiveRebinding()
        .WithRebindAddingNewBinding()
        .WithControlsExcluding("Mouse")
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(operation => rebindComplete())
        .OnCancel(operation => rebindCanceled())
        .Start();
    }

    private void rebindComplete()
    {
        this.textComponent.text = InputControlPath.ToHumanReadableString(this.actionInput.bindings[0].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        setKeyBinding();
        this.rebindingOperation.Dispose();
    }

    private void rebindCanceled()
    {
        this.textComponent.text = this.originalText;
        this.rebindingOperation.Dispose();
    }

    private void setKeyBinding()
    {
       if(this.inputToReplace.Equals(inputList.movementInput))
       {
           this.keyBindingManager.setMovementInput(this.actionInput);
       }
       else if(this.inputToReplace.Equals(inputList.upInput))
       {
           this.keyBindingManager.setUpInput(this.actionInput);
       }
       else if(this.inputToReplace.Equals(inputList.downInput))
       {
           this.keyBindingManager.setDownInput(this.actionInput);
       }
       else if(this.inputToReplace.Equals(inputList.jumpInput))
       {
           this.keyBindingManager.setJumpInput(this.actionInput);
       }
       else if(this.inputToReplace.Equals(inputList.slashInput))
       {
           this.keyBindingManager.setSlashInput(this.actionInput);
       }
       else if(this.inputToReplace.Equals(inputList.projectileInput))
       {
           this.keyBindingManager.setProjectileInput(this.actionInput);
       }
       else if(this.inputToReplace.Equals(inputList.menuInput))
       {
           this.keyBindingManager.setMenuInput(this.actionInput);
       }
    }
}
