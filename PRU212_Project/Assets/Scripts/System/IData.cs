using UnityEngine;

public interface IData 
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
