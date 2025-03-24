using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TMP_Text coinsUi;
    public GameObject shop;
    public ShopItemSo[] shopItemsSO;
    public GameObject[] shopPanelGO;
    public ShopTemplate[] shopPanel;
    public Button[] purchaseBtn;
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

    //Add coins to shop
    public void Addcoins(int coins)
    {        
        coinsUi.text = "Coin:" + coins.ToString();
    }

    //check if player can purchase item
    public void CheckPuchaseBtn(int coins)
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            if (coins >= shopItemsSO[i].baseCost)
            {
                purchaseBtn[i].interactable = true;
            }
            else
            {
                purchaseBtn[i].interactable = false;
            }
        }
    }

    //initiate shop panel
    public void LoadPanel()
    {
        int coins = GameManager.instant.GetCointCount();
        Addcoins(coins);
        CheckPuchaseBtn(coins);
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanel[i].titleTxt.text = shopItemsSO[i].title;
            shopPanel[i].descriptionTxt.text = shopItemsSO[i].description;
            shopPanel[i].costTxt.text = "Coins:" + shopItemsSO[i].baseCost.ToString();
        }
    }

    // Exit button
    public void ExitBtn()
    {
        shop.SetActive(false);
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    //Purchase heart button
    public void PurchaseHeart()
    {
        if (HeathManager.GetHeart() == 6)
        {
            LoadPanel();
        }
        else
        {
            HeathManager.AddHealth(1);
            GameManager.instant.addScore(-5);
            LoadPanel();
        }      
    }

    public void PurchaseFullGeneration()
    {
        int gameheart = HeathManager.GetGameHeart();
        int heart = HeathManager.GetHeart();
        if(heart < gameheart)
        {
            HeathManager.AddHealth(gameheart - heart);
            GameManager.instant.addScore(-10);
        }       
        LoadPanel();
    }

    public void buyHat()
    {
        LoadPanel();
    }
}
