using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public Vector2 checkpointPos;
    public Dictionary<string, bool> coinsCollected = new Dictionary<string, bool>();

    public GameData()
    {
        this.score = 0;
        this.checkpointPos = new Vector2(-37.68f, -3.01f);
        coinsCollected = new Dictionary<string, bool>();
    }
}
