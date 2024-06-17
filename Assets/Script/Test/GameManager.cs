using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text coinText; // 코인을 표시할 텍스트
    public int[] itemCosts; // 각 아이템의 가격
    public Text[] inventoryTexts; // 인벤토리 아이템 갯수를 표시하는 텍스트 UI 배열

    private int coins; // 현재 코인
    private Dictionary<int, int> inventory = new Dictionary<int, int>(); // 아이템 인벤토리
    private bool isInitialized = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeGameManager(); // 기존 GameManager의 초기화 메서드 호출
    }

    void InitializeGameManager()
    {
        LoadCoins();
        InitializeInventory();
        UpdateUI();
        isInitialized = true;
    }

    void LoadCoins()
    {
        // 에디터 모드에서는 초기화 값만 설정
        coins = 2000;
        Debug.Log("Editor mode: Coins initialized to 2000");
    }

    void SaveCoins()
    {
        // 에디터 모드에서는 저장하지 않음
        Debug.Log("Editor mode: Coins not saved");
    }

    void InitializeInventory()
    {
        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventory.Add(i + 1, 0);
        }
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        SaveCoins();
        UpdateUI();
    }

    public void SubtractCoin(int amount)
    {
        coins -= amount;
        SaveCoins();
        UpdateUI();
    }

    public int GetCoins()
    {
        return coins;
    }

    public int GetInventoryItemCount(int itemID)
    {
        if (inventory.ContainsKey(itemID))
        {
            return inventory[itemID];
        }
        return 0;
    }

    internal string GetItemName(int requiredItemID)
    {
        throw new NotImplementedException();
    }

    public int GetItemPrice(int itemID)
    {
        if (itemID > 0 && itemID <= itemCosts.Length)
        {
            return itemCosts[itemID - 1];
        }
        return 0;
    }

    public void BuyItem(int itemID)
    {
        int itemPrice = GetItemPrice(itemID);
        if (coins >= itemPrice)
        {
            SubtractCoin(itemPrice); // 코인 차감
            inventory[itemID]++; // 해당 아이템 갯수 증가
            Debug.Log("아이템을 구매했습니다!");
            UpdateUI();
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    public void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = " " + coins.ToString();
        }

        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventoryTexts[i].text = GetInventoryItemCount(i + 1).ToString();
        }
    }
}
