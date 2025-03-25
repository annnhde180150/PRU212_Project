using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HatManager : MonoBehaviour
{
    public SpriteRenderer hatRenderer; // The SpriteRenderer for the hat
    public Sprite[] hatSprites; // Array of hat sprites
    public static HatManager instance;

    //private const string HatPurchasedKey = "HatPurchased";
    //private const string HatIndexKey = "HatIndex";
     void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        //// Check if the hat was purchased
        //if (PlayerPrefs.GetInt(HatPurchasedKey, 0) == 1)
        //{
        //    hatObject.SetActive(true);
        //    int savedHatIndex = PlayerPrefs.GetInt(HatIndexKey, 0);
        //    hatRenderer.sprite = hatSprites[savedHatIndex]; // Load the saved hat
        //}
        //else
        //{
        //    hatObject.SetActive(false); // Hide the hat if not purchased
        //}
    }
    public void GiveHat()
    {
        if (HatManager.instance == null)
        {
            Debug.LogError("HatManager instance is null! Make sure the script is attached to a GameObject in the scene.");
            return;
        }
        if (instance.hatSprites.Length == 0)
        {
            Debug.LogWarning("No hat sprites available!");
            return;
        }

        // Choose a random hat sprite
        int randomHatIndex = Random.Range(0, hatSprites.Length);
        //PlayerPrefs.SetInt(HatPurchasedKey, 1);
        //PlayerPrefs.SetInt(HatIndexKey, randomHatIndex);
        //PlayerPrefs.Save();

        instance.hatRenderer.gameObject.SetActive(true);
        instance.hatRenderer.sprite = hatSprites[randomHatIndex]; // Assign random hat
    }
}
