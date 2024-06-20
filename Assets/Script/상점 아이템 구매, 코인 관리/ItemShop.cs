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

        UpdateUI(); // 초기화 시 UI 업데이트
    }

    public void BuyItem(int itemID)
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        if (gameManager.GetCoins() >= gameManager.GetItemPrice(itemID))
        {
            gameManager.BuyItem(itemID); // 아이템 구매 처리
            Debug.Log("아이템을 구매했습니다!");
            UpdateUI(); // 구매 후 UI 업데이트
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    void Update()
    {
        // 매 프레임마다 코인 텍스트와 인벤토리 텍스트 업데이트
        if (gameManager != null)
        {
            coinsText.text = gameManager.GetCoins().ToString();

            for (int i = 0; i < inventoryTexts.Length; i++)
            {
                inventoryTexts[i].text = gameManager.GetInventoryItemCount(i + 1).ToString();
            }
        }
    }

    void OnEnable()
    {
        // 활성화 시 UI 업데이트
        UpdateUI();
    }

    void UpdateUI()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        coinsText.text = gameManager.GetCoins().ToString();

        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventoryTexts[i].text = gameManager.GetInventoryItemCount(i + 1).ToString();
        }
    }
}
