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
    [SerializeField]private PlayerInputManager keyBindingManager;
    private enum inputList { movementInput, downInput, upInput, jumpInput, projectileInput, slashInput, menuInput};
    [SerializeField]private inputList inputToReplace = inputList.movementInput;
    [SerializeField]private Text textComponent;
    private InputAction actionInput = new InputAction();

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
            performInteractiveRebind(false , -1);   //-1 adica il ia exact pe el
        }
        else
        {
            this.textComponent.text = "Press Key(s)";
            //performInteractiveRebind(true , 1);   //cu composite vine mai intai ce e "Axis" , "2DVector" etc (index 0) si mai apoi <Keyboard>/button (index 1) , <keyboard>/button (index 2) etc
            configureAxisInput();//test delete after
        }
    }

    private void configureAxisInput()
    {
        this.actionInput.AddCompositeBinding("Axis") // Or just "Axis"
        .With("Positive", "<Gamepad>/rightTrigger")
        .With("Negative", "<Gamepad>/leftTrigger");

        performInteractiveRebind(true , 1);
    }

    private void performInteractiveRebind(bool hasComposite, int bindingIndex)
    {
        this.rebindingOperation = this.actionInput.PerformInteractiveRebinding(bindingIndex)
        .WithRebindAddingNewBinding()
        .WithControlsExcluding("Mouse")
        .OnMatchWaitForAnother(0.1f)
        .OnComplete(
                    operation =>
                    {
                        rebindComplete();

                        if (hasComposite)
                        {
                            var nextBindingIndex = bindingIndex + 1;
                            if (nextBindingIndex < this.actionInput.bindings.Count && this.actionInput.bindings[nextBindingIndex].isPartOfComposite)
                                performInteractiveRebind(true, nextBindingIndex);
                        }
                    })
        .OnCancel(operation => rebindCanceled())
        .Start();
    }

    private void rebindComplete()
    {
        //sa pun decat rebindingOperation.dispose dupa fiecare iar astea cu text si seKey doar daca nu mai exista composite/partofcomposite

        this.textComponent.text = "";
        for(int i = 0 ; i < this.actionInput.bindings.Count ; i++)
        {
            if(actionInput.bindings[i].isPartOfComposite){ continue; }
            this.textComponent.text += InputControlPath.ToHumanReadableString(this.actionInput.bindings[i].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
        setKeyBinding();
        this.rebindingOperation.Dispose();

        Debug.Log(actionInput);//delete after
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
