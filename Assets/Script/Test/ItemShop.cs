using UnityEngine;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour
{
    public Text coinsText; // 플레이어의 코인을 표시하는 텍스트 UI
    public Text[] inventoryTexts; // 인벤토리 아이템 갯수를 표시하는 텍스트 UI 배열

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        UpdateUI();
    }

    public void BuyItem(int itemID)
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        if (gameManager.GetCoins() >= gameManager.itemCosts[itemID - 1])
        {
            gameManager.BuyItem(itemID); // GameManager의 BuyItem 메서드 호출
            Debug.Log("아이템을 구매했습니다!");
            UpdateUI();
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    void Update()
    {
        // 코인 텍스트와 인벤토리 텍스트 업데이트
        if (gameManager != null)
        {
            coinsText.text = " " + gameManager.GetCoins().ToString();

            for (int i = 0; i < inventoryTexts.Length; i++)
            {
                inventoryTexts[i].text = gameManager.GetInventoryItemCount(i + 1).ToString();
            }
        }
    }

    void OnEnable()
    {
        // 씬 활성화 시에도 UI 업데이트
        UpdateUI();
    }

    void UpdateUI()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        coinsText.text = " " + gameManager.GetCoins().ToString();

        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventoryTexts[i].text = gameManager.GetInventoryItemCount(i + 1).ToString();
        }
    }
}
