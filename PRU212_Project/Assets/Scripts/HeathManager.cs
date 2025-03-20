using UnityEngine;
using UnityEngine.UI;

public class HeathManager : MonoBehaviour
{
    [Header("Heath Settings")]
    public static HeathManager instance;
    [SerializeField] private int maxHealth = 6;
    public static int health = 3;
    public Image[] heart;
    [SerializeField] public Sprite fullHeart;
    [SerializeField] public Sprite emptyHeart;


    void Awake()
    {
        instance = this;  // Gán instance cho Singleton
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHealthUI();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateHealthUI()
    {
        for (int i = 0; i < heart.Length; i++)
        {
            heart[i].enabled = (i < health); 
        }
    }

    public static void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, instance.maxHealth); 
        instance.UpdateHealthUI(); 
    }

    public static void ReloadHealth()
    {
        health = 3;
        instance.UpdateHealthUI();
    }

    public static void AddHealth(int healthAmount)
    {
        health += healthAmount;
        health = Mathf.Clamp(health, 0, instance.maxHealth);
        instance.UpdateHealthUI();
    }
}
