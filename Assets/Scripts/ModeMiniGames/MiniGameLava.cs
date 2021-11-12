using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameLava : MonoBehaviour
{
    [SerializeField][Range(5,30)]private int minNrOfRounds = 5;
    [SerializeField][Range(6,30)]private int maxNrOfRounds = 10;
}
