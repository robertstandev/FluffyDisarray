using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField][Range(1,4)]private int playerCount = 1;
    [SerializeField][Range(1,4)]private int botCount = 1;

    public int getPlayerCount() { return this.playerCount; }
    public void setPlayerCount(int value) { this.playerCount = value; }

    public int getBotCount() { return this.botCount; }
    public void setBotCount(int value) { this.botCount = value; }
    
    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
}
