using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragHandler2D : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalPosition;

    void Update()
    {
        if (isDragging)
        {
            // 마우스 위치에 따라 물건 위치를 업데이트
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10.0f; // 카메라로부터의 거리 설정
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    void OnMouseDown()
    {
        // 드래그 시작
        isDragging = true;
        originalPosition = transform.position;
    }

    void OnMouseUp()
    {
        // 드래그 종료
        isDragging = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // 손님과 충돌 확인
        if (!isDragging)
        {
            Customer2D customer = other.GetComponent<Customer2D>();
            if (customer != null)
            {
                SellItem(customer);
            }
        }
    }

    void SellItem(Customer2D customer)
    {
        Item2D item = GetComponent<Item2D>();
        if (item != null && customer.BuyItem(item.price))
        {
            Debug.Log("Item sold for " + item.price + " dollars");
            // 판매 후 물건 비활성화 또는 제거
            Destroy(gameObject); // 아이템 오브젝트 삭제
            // gameObject.SetActive(false); // 아이템 오브젝트 비활성화
        }
        else
        {
            Debug.Log("Customer doesn't have enough money or item script missing");
        }
    }
}
