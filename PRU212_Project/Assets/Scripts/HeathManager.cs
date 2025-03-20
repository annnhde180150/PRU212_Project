using UnityEngine;
using UnityEngine.UI;

public class HeathManager : MonoBehaviour
{
    [Header("Heath Settings")]
    [SerializeField] public static int Heath = 3;
    public Image[] heart;
    [SerializeField] public Sprite fullHeart;
    [SerializeField] public Sprite emptyHeart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Image image in heart)
        {
            image.sprite = emptyHeart;
        }
        for(int i=0; i < Heath; i++)
        {
            heart[i].sprite = fullHeart;
        }
    }
}
