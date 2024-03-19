using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ExGameData" , fileName = "New ExGameData" , order = 50)]
public class ExGameData : ScriptableObject

{  
    public string gameName; 
    public int gameScore;
    public bool isGameActive;
}
