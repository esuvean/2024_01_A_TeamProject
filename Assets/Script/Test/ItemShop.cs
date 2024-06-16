using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public Text coinsText; // 플레이어의 코인을 표시하는 텍스트 UI
    public Text[] inventoryTexts; // 인벤토리 아이템 갯수를 표시하는 텍스트 UI 배열

    void Start()
    {
        UpdateUI();
    }

    public void BuyItem(int itemID)
    {
        GameManager gameManager = GameManager.instance as GameManager;
        if (gameManager != null)
        {
            gameManager.BuyItem(itemID); // GameManager의 BuyItem 메서드 호출
            Debug.Log("아이템을 구매했습니다!");
            UpdateUI();
        }
        else
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
        }
    }

    void UpdateUI()
    {
        GameManager gameManager = GameManager.instance as GameManager;
        if (gameManager != null)
        {
            coinsText.text = " " + gameManager.GetCoins().ToString();

            for (int i = 0; i < inventoryTexts.Length; i++)
            {
                inventoryTexts[i].text = gameManager.GetInventoryItemCount(i + 1).ToString();
            }
        }
        else
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
        }
    }
}
