using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClashAIController : MonoBehaviour
{


    //fac coroutine care verifica la 1 sec
    //ideea AI-ului:
    //-mai intai cauta si se duce spre collectibles (dar sa astept sa vina putin spre pamant nu s-a spawnat collectible-ul si asta deja fuge spre collectible cand nici nu se vede)
    //-nu e collectible atunci ataci tinta cea mai aproape
    //-tii cont daca tu ca AI ai putina viata (20), sa eviti sa fi mai aproape de gen 10f de tinte si sa folosesti doar projectile cand e valabil si in rest sa fugi si ataci (cu slash) DOAR daca nu ai projectile ai 20 hp sau mai putin , ai mai putin de 30 stamina si tinta e aproape (< 10f de tine)
    //-daca tinta foloseste projectile spre tine si tu ai projectile valabil sa il folosesti si tu spre el ca sa se anuleze (dar dai sa tintesti projectile-ul nu jucatorul)
    //-tii cont daca tinta are pregatit skillul de projectile
    //-tii cont daca tinta nu mai are stamina ca inseamna ca nu mai poate sa sara si daca ai projectile gata poti sa dai spre el iar apoi fugi sa il omori
    //
    //-si citeste mai jos daca tinta are efecte ce fac

    //Fac asa daca tintele spre care se duc au efecte activate gen:
    //switch -> daca a folosit switch ORICINE atunci nu mai iei tinte ci doar te misti random
    //stats -> te feresti de el , te duci sa ataci alte tinte si chiar si atunci te feresti de tinta cu stats
    //invizibilitate -> te duci si ataci alta tinta pe harta si daca nu e (sunt morti) doar fugi random
    //time -> te duci si il ataci automat insa doar daca nu are doar PRIMELE 2 efecte
    //gravity -> te duci sa il ataci (dar doar daca nu are si vreuna din primele 3 efecte)
    //high voltage-> te feresti de el daca are viata peste 21 (stai la peste 15f departe de el si dai doar cu proiectile spre el (daca sunt valabile)) , daca are mai putin de 21 atunci daca ai mai mult de 60 viata te duci spre el altfel doar cu proiectile dai
}
