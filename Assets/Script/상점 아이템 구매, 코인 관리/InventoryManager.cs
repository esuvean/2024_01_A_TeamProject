using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Text inventoryText; // 인벤토리 아이템 갯수를 표시하는 텍스트 UI
    private int itemCount = 0; // 현재 인벤토리에 있는 아이템의 갯수

    void Start()
    {
        UpdateInventoryUI();
    }

    // 아이템을 추가하는 함수
    public void AddItem()
    {
        itemCount++; // 아이템 갯수 증가
        UpdateInventoryUI();
    }

    // 인벤토리 UI 업데이트 함수
    void UpdateInventoryUI()
    {
        inventoryText.text = " " + itemCount.ToString();
    }
}