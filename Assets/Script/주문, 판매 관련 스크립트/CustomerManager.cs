using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform canvasTransform;
    public Text greetingText;
    public Text itemText;
    public Text quantityText;
    public Text closingText;
    public Image itemImage;

    private List<Customer> customers = new List<Customer>();
    private Customer currentCustomer;
    private GameManager gameManager;
    public static GameManager Instance;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        CreateCustomer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCustomer();
        }

        if (Input.GetMouseButtonDown(0))
        {
            SellItemToCustomer();
        }
    }

    void CreateCustomer()
    {
        if (customerPrefab == null || canvasTransform == null)
        {
            Debug.LogError("CustomerPrefab 또는 CanvasTransform이 설정되지 않았습니다.");
            return;
        }

        if (currentCustomer != null)
        {
            return;
        }

        GameObject newCustomerObject = Instantiate(customerPrefab, canvasTransform);
        newCustomerObject.transform.SetParent(canvasTransform, false);
        newCustomerObject.transform.localScale = Vector3.one;

        Customer newCustomer = newCustomerObject.GetComponent<Customer>();
        if (newCustomer != null)
        {
            newCustomer.SetGreeting("안녕하세요!");
            newCustomer.SetClosing("개 주세요");

            int itemID = Random.Range(1, 3);
            int itemCount = Random.Range(1, 4);
            int itemPrice = gameManager.GetItemPrice(itemID);

            newCustomer.SetRequirements(itemID, itemCount, itemPrice);

            customers.Add(newCustomer);
            SetCurrentCustomer(newCustomer);
        }
        else
        {
            Debug.LogError("CustomerPrefab에 Customer 스크립트가 없습니다.");
        }
    }

    void SetCurrentCustomer(Customer customer)
    {
        currentCustomer = customer;

        if (currentCustomer != null)
        {
            if (greetingText != null)
            {
                greetingText.text = currentCustomer.GetGreeting();
            }
            if (itemText != null)
            {
                itemText.text = gameManager.GetItemName(currentCustomer.RequiredItemID);
            }
            if (quantityText != null)
            {
                quantityText.text = currentCustomer.GetRequiredItemCount().ToString();
            }
            if (closingText != null)
            {
                closingText.text = currentCustomer.GetClosing();
            }
            if (itemImage != null)
            {
                GameObject itemPrefab = gameManager.GetItemPrefab(currentCustomer.RequiredItemID);
                if (itemPrefab != null)
                {
                    itemImage.sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        else
        {
            Debug.LogError("현재 손님이 설정되지 않았습니다.");
        }
    }

    void SellItemToCustomer()
    {
        if (currentCustomer != null)
        {
            int itemID = currentCustomer.RequiredItemID;
            int itemCount = currentCustomer.GetRequiredItemCount();

            if (gameManager.GetItemName(itemID) == itemText.text)
            {
                if (gameManager.GetInventoryItemCount(itemID) >= itemCount)
                {
                    gameManager.SubtractInventoryItem(itemID, itemCount);
                    int totalPrice = currentCustomer.RequiredItemPrice * itemCount;
                    gameManager.AddCoin(totalPrice);

                    customers.Remove(currentCustomer);
                    Destroy(currentCustomer.gameObject);

                    currentCustomer = null;
                    CreateCustomer(); // 다음 손님 생성
                }
                else
                {
                    Debug.Log("아이템이 충분하지 않습니다.");
                }
            }
            else
            {
                Debug.Log("아이템이 일치하지 않습니다.");
            }
        }
    }
}
