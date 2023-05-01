using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    //Va tine cont de toate questurile care sunt de facut si care au fost deja facute

    //Se va face cu 2 array si adaugat din UI din Editor
    //1 va fi pentru descrierea questului
    //2 va fi in care se va zice ce gameobject trebuie sa dispara
    //(ex daca trebuie sa omori un boss cand ala va fi spriteRenderul.enabled false inseamna ca a fost omorat ,
    //la fel daca trebuie sa ia un item ceva de undeva cand devine SpriteRenderul.enabled false atunci a fost luat
    //(daca folosesc alta metoda gen gameobject.visible false atunci trebuie sa fiu atent ca questul sa se activeze doar dupa acel gameobject devine visible.true))
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
