using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField]private Dropdown dropdownComponent;
    [SerializeField]private InputField ipInputField;
    [SerializeField]private InputField portInputField;
    [SerializeField]private Text statusTextComponent;
    [SerializeField]private Button joinHostButton;

    //add on click listener to dropdown
    //get ipInputField and portInputField and send them to add on click listener to joinHostButton
    //change status text
    //daca da join va ignora toate caracterele create in afara de asta la care era cand da join, iar cand da join trimite comanda la ip/portul ala cu ce a ales el , butoane culoare , proiectil etc si pune automat ca un player acolo fara sa mai configureze nimic acolo
    //acolo poate sa ignore cati jucatori au fost creati deja offline ca ala va vedea doar camera lui pe online
}
