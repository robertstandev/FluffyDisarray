using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableOnCollision : MonoBehaviour
{
    [SerializeField]private Component[] test;   //o sa fac un IActionPerformer cu (performAction())

    void Start()
    {
        if(test.Length > 0)
        {
            if(!(test[0] is IController))   { return; } //IActionPerformer
            test[0].GetComponent<IController>().disableController();//IActionPerformer().performAction()
        }
    }

}
