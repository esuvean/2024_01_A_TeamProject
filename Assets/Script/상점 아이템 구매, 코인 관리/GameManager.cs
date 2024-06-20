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
        // 각 아이템 ID에 해당하는 프리팹을 Resources 폴더에서 로드합니다.
        for (int i = 0; i < itemNames.Length; i++)
        {
            // Item Text에 표시된 이름으로 프리팹을 로드합니다.
            string prefabName = "Bread_" + (i + 1); // 예시: Bread_1, Bread_2, ...
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + prefabName);

            if (prefab != null)
            {
                itemPrefabsMap.Add(i + 1, prefab);
            }
            else
            {
                Debug.LogError("아이템 ID에 해당하는 프리팹을 찾을 수 없습니다: " + prefabName);
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
            Debug.LogError("아이템 ID에 해당하는 프리팹을 찾을 수 없습니다: " + itemID);
            return null;
        }
    }

    void Start()
    {
        if (!isInitialized)
        {
            LoadCoins();
            FindAndAssignUITexts(); // Start 메서드에서 UI 텍스트들을 찾아서 할당합니다.
            UpdateUI(); // UI 업데이트
            isInitialized = true;
        }
    }

    void FindAndAssignUITexts()
    {
        coinText = GameObject.FindObjectOfType<Text>(); // 게임 오브젝트 전체에서 Text 컴포넌트를 찾습니다.
        if (coinText == null)
        {
            Debug.LogError("CoinText를 찾을 수 없습니다.");
        }

        // inventoryTexts 배열의 각 요소를 GameObject.Find로 찾아와서 할당합니다.
        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventoryTexts[i] = GameObject.Find("InventoryText" + i)?.GetComponent<Text>();
            if (inventoryTexts[i] == null)
            {
                Debug.LogError("InventoryText" + i + "를 찾을 수 없습니다.");
            }
        }
    }

    public void AddCoin(int amount)
    {
        coins += amount;
        SaveCoins();
        UpdateUI(); // 코인이 추가될 때마다 UI를 업데이트합니다.
    }

    public void SubtractCoin(int amount)
    {
        coins -= amount;
        SaveCoins();
        UpdateUI(); // 코인이 차감될 때마다 UI를 업데이트합니다.
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
                AddInventoryItem(itemID); // 인벤토리 아이템 추가
                Debug.Log("아이템을 구매했습니다!");

                UpdateUI(); // UI 업데이트
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
            coinText.text = coins.ToString();
        }
        else
        {
            Debug.LogError("CoinText가 할당되지 않았습니다.");
        }

        // inventoryTexts가 null이 아니고, 길이가 inventory의 크기와 같다면 각각의 인벤토리 텍스트를 업데이트합니다.
        if (inventoryTexts != null && inventoryTexts.Length == inventory.Count)
        {
            for (int i = 0; i < inventoryTexts.Length; i++)
            {
                if (inventoryTexts[i] != null)
                {
                    inventoryTexts[i].text = inventory[i + 1].ToString(); // 인벤토리는 1부터 시작
                }
                //else
                //{
                //    Debug.LogError("InventoryText" + i + "가 할당되지 않았습니다.");
                //}
            }
        }
        else
        {
            Debug.LogError("인벤토리 텍스트 배열의 길이가 inventory 딕셔너리의 크기와 일치하지 않습니다.");
        }
    }

    public void SubtractInventoryItem(int itemID, int count)
    {
        if (inventory.ContainsKey(itemID))
        {
            inventory[itemID] -= count;
            if (inventory[itemID] <= 0)
            {
                inventory.Remove(itemID);
            }
            UpdateUI(); // UI 업데이트
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

    private void AddInventoryItem(int itemID)
    {
        if (inventory.ContainsKey(itemID))
        {
            inventory[itemID]++;
        }
        else
        {
            inventory[itemID] = 1;
        }
    }
}
