using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject[] breadPrefabs; // Bread_0~4 프리팹들
    public Transform orderPanel; // 손님의 주문을 표시할 패널
    public Text orderText; // 주문을 표시할 텍스트
    public Image orderImage; // 주문 아이템의 이미지를 표시할 이미지

    private string currentOrder;
    private GameObject currentOrderPrefab;

    void Start()
    {
        GenerateNewOrder();
    }

    void GenerateNewOrder()
    {
        // 랜덤으로 손님의 주문 생성
        int randomIndex = Random.Range(0, breadPrefabs.Length);
        currentOrderPrefab = breadPrefabs[randomIndex];
        currentOrder = "Bread_" + randomIndex;

        // 주문 텍스트와 이미지 업데이트
        orderText.text = currentOrder;
        orderImage.sprite = currentOrderPrefab.GetComponent<SpriteRenderer>().sprite;
    }

    public void SellItem(GameObject item)
    {
        if (item.name == currentOrderPrefab.name)
        {
            Destroy(item);
            CoinManager.Instance.AddCoins(1); // 코인 증가
            GenerateNewOrder();
        }
    }
}
