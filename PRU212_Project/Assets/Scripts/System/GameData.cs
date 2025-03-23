using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int score;
    public Vector2 checkpointPos;
    public Dictionary<string, bool> coinsCollected = new Dictionary<string, bool>();
    public int health;
    public int gameHeart;
    public string currentLevel;
    public GameData()
    {
        this.score = 0;
        this.checkpointPos = new Vector2(-37.68f, -3.01f);
        this.health = 3;
        this.gameHeart = 3;
        this.currentLevel = "GameScene";
    }
}
