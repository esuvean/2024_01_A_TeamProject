using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemShop : MonoBehaviour
{
    public Text coinsText; // 플레이어의 코인을 표시하는 텍스트 UI
    public Text[] inventoryTexts; // 인벤토리 아이템 갯수를 표시하는 텍스트 UI 배열
    public int[] itemCosts; // 각 아이템의 가격
    private Dictionary<int, int> inventory = new Dictionary<int, int>(); // 아이템 인벤토리

    void Start()
    {
        // 아이템 인벤토리 초기화
        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventory.Add(i + 1, 0);
        }
        UpdateUI();
    }

    // 아이템을 구매하는 함수
    public void BuyItem(int itemID)
    {
        if (GameManager.Instance.Coins >= itemCosts[itemID - 1])
        {
            GameManager.Instance.Coins -= itemCosts[itemID - 1]; // 코인 차감
            inventory[itemID]++; // 해당 아이템 갯수 증가
            Debug.Log("아이템을 구매했습니다!");

            UpdateUI();
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    // UI 업데이트 함수
    void UpdateUI()
    {
        coinsText.text = " " + GameManager.Instance.Coins.ToString();

        // 인벤토리 각 아이템의 갯수를 표시
        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventoryTexts[i].text =  inventory[i + 1].ToString();
        }
    }
}


