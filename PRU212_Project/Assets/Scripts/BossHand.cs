using UnityEngine;

public class BossHand : MonoBehaviour
{
    public int Damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Damage = (int)FindAnyObjectByType<Enemy>().damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
