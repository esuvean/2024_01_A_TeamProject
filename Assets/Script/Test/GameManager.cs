using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public static GameManager Instance; // 싱글톤 인스턴스

    public CoinManager coinManager; // 코인 매니저
    private int[] inventoryCounts; // 인벤토리 아이템 갯수 배열
    internal static object instance;

    private static GameManager _instance; // 싱글톤 인스턴스

    private int coins = 0;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>(); // Scene에서 GameManager 찾기
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<GameManager>(); // 없으면 새로 생성
                }
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            _instance = this; // 인스턴스 할당
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있는 경우 중복된 GameManager 파괴
        }

        // 인벤토리 초기화 (여기서는 예시로 크기 5의 배열을 생성)
        inventoryCounts = new int[5];
    }

    // 인벤토리 아이템 갯수 반환 메서드
    public int GetInventoryItemCount(int itemId)
    {
        // itemId에 해당하는 인벤토리 아이템 갯수 반환 (예시로 배열에서 아이템 갯수 반환)
        return inventoryCounts[itemId - 1];
    }

    // 인벤토리 아이템 갯수 증가 메서드
    public void IncreaseInventoryItemCount(int itemId, int amount)
    {
        // itemId에 해당하는 인벤토리 아이템 갯수 증가
        inventoryCounts[itemId - 1] += amount;
    }

    // 인벤토리 아이템 갯수 감소 메서드
    public void DecreaseInventoryItemCount(int itemId, int amount)
    {
        // itemId에 해당하는 인벤토리 아이템 갯수 감소
        inventoryCounts[itemId - 1] -= amount;

        // 인벤토리 갯수가 음수가 되지 않도록 보정
        if (inventoryCounts[itemId - 1] < 0)
        {
            inventoryCounts[itemId - 1] = 0;
        }
    }

    public int GetCoins()
    {
        return coins;
    }
    public void BuyItem(int itemID)
    {
        // 아이템을 구매하는 로직을 여기에 추가
        Debug.Log($"아이템 ID {itemID}를 구매했습니다!");
    }
}
