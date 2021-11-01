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

    /*
    add on click listener to dropdown
    get ipInputField and portInputField and send them to add on click listener to joinHostButton
    change status text
    daca da join va ignora toate caracterele create in afara de asta la care era cand da join, iar cand da join trimite comanda la ip/portul ala cu ce a ales el , bind keys, culoare , proiectil etc si pune automat ca un player acolo fara sa mai configureze nimic acolo
    acolo poate sa ignore cati jucatori au fost creati deja offline ca ala va vedea doar camera lui pe online

    daca alege host din dropdown atunci ipInputField sa fie ne interactive si sa apara ipul tau (ca sa poti sa il zici lu ala care da join) si port pui ce port vrei in portInputField
    daca alege join din dropdown atunci ipInputField sa fie interactive si sa scrii tu ipul lu ala care da host si scrii ce port a pus in portInputField

    la status sa apara mesaje precum
    -host :
    Status: opening port....
    Status: setting connection....
    Status: connection established !
    Status: waiting for players...
    (apoi cand vrei mai selectezi de pe acolo caractere noi)
    iar cand termini de configurat caracterele sa se opreasca si asta de conectiune de cautat pentru altii si sa joace doar cu aia gata conectati
    pun la scriptul ala CharacterEditorContinueButton la OnDisable sa faca asta

    -join:
    Status: checking connection...
    Status: connection established ! / Status: connection unavailable !
    Status: sending configured data...
    Status: waiting for host... (ca sa termine de configurat toate caracterele)
    */
}
