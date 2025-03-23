using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public int coins;
    public TMP_Text coinsUi;
    public GameObject shop;
    public ShopItemSo[] shopItemsSO;
    public GameObject[] shopPanelGO;
    public ShopTemplate[] shopPanel;
    private PlayerController playerController;
    public static ShopManager instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanelGO[i].SetActive(true);
        }
        LoadPanel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Addcoins(int coins)
    {        
        coinsUi.text = "Coin:" + coins.ToString();
    }

    public void LoadPanel()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanel[i].titleTxt.text = shopItemsSO[i].title;
            shopPanel[i].descriptionTxt.text = shopItemsSO[i].description;
            shopPanel[i].costTxt.text = "Coins:" + shopItemsSO[i].baseCost.ToString();
        }
    }

    public void ExitBtn()
    {
        shop.SetActive(false);
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    } 
}
