using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text coinText; // 코인을 표시할 텍스트
    public int[] itemCosts; // 각 아이템의 가격
    public string[] itemNames; // 각 아이템의 이름
    public Text[] inventoryTexts; // 인벤토리 아이템 갯수를 표시하는 텍스트 UI 배열

    private int coins; // 현재 코인
    private Dictionary<int, int> inventory = new Dictionary<int, int>(); // 각 아이템 갯수를 저장할 딕셔너리
    private Dictionary<int, GameObject> itemPrefabsMap = new Dictionary<int, GameObject>(); // 아이템 ID와 프리팹을 매핑할 딕셔너리

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

        InitializeItemPrefabs(); // 아이템 프리팹 초기화
        InitializeInventory(); // 인벤토리 초기화
    }

    void InitializeItemPrefabs()
    {
        // 각 아이템 ID에 해당하는 프리팹을 매핑합니다.
        // 예를 들어 아이템 ID 1에는 "Bread_1" 프리팹이라고 가정하면:
        for (int i = 0; i < itemNames.Length; i++)
        {
            GameObject prefab = Resources.Load<GameObject>(itemNames[i]);
            if (prefab != null)
            {
                itemPrefabsMap.Add(i + 1, prefab);
            }
            else
            {
                Debug.LogError("아이템 ID에 해당하는 프리팹을 찾을 수 없습니다: " + itemNames[i]);
            }
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        if (itemPrefabsMap.ContainsKey(itemID))
        {
            return itemPrefabsMap[itemID];
        }
        else
        {
            Debug.LogError("아이템 ID에 해당하는 프리팹이 없습니다: " + itemID);
            return null;
        }
    }

    void Start()
    {
        if (!isInitialized)
        {
            LoadCoins();
            UpdateUI();
            isInitialized = true;
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

    public string GetItemName(int itemID)
    {
        if (itemID > 0 && itemID <= itemNames.Length)
        {
            return itemNames[itemID - 1];
        }
        return "알 수 없는 아이템";
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
        if (itemID > 0 && itemID <= itemCosts.Length)
        {
            if (coins >= itemCosts[itemID - 1])
            {
                SubtractCoin(itemCosts[itemID - 1]); // 코인 차감
                inventory[itemID]++; // 해당 아이템 갯수 증가
                Debug.Log("아이템을 구매했습니다!");

                UpdateUI();
            }
            else
            {
                Debug.Log("코인이 부족합니다!");
            }
        }
        else
        {
            Debug.LogError("잘못된 아이템 ID입니다: " + itemID);
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

    public void SubtractInventoryItem(int itemID, int count)
    {
        if (inventory.ContainsKey(itemID))
        {
            inventory[itemID] -= count;
            UpdateUI();
        }
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
        for (int i = 0; i < itemNames.Length; i++)
        {
            inventory.Add(i + 1, 0);
        }
    }
}
