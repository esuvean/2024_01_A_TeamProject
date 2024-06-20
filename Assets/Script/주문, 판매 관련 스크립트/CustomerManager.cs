using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public GameObject[] customerPrefabs; // CustomerPrefab을 배열로 변경
    public Transform canvasTransform;
    public Text greetingText;
    public Text itemText;
    public Text quantityText;
    public Text closingText;
    public Image itemImage;

    private List<Customer> customers = new List<Customer>();
    private Customer currentCustomer;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        CreateCustomer(); // 시작할 때 첫 번째 손님 생성
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateCustomer();
        }

        if (Input.GetMouseButtonDown(1)) // 오른쪽 클릭 감지
        {
            SellItemToCustomer();
        }
    }

    void CreateCustomer()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0 || canvasTransform == null)
        {
            Debug.LogError("CustomerPrefab 또는 CanvasTransform이 설정되지 않았습니다.");
            return;
        }

        if (currentCustomer != null)
        {
            return; // 현재 손님이 있는 경우 다음 손님 생성하지 않음
        }

        // 랜덤으로 CustomerPrefab 선택
        int randomIndex = Random.Range(0, customerPrefabs.Length);
        GameObject newCustomerObject = Instantiate(customerPrefabs[randomIndex], canvasTransform);
        newCustomerObject.transform.SetParent(canvasTransform, false);
        newCustomerObject.transform.localScale = Vector3.one;

        Customer newCustomer = newCustomerObject.GetComponent<Customer>();
        if (newCustomer != null)
        {
            newCustomer.SetGreeting("안녕하세요!");
            newCustomer.SetClosing("개 주세요");

            int itemID = Random.Range(1, 3); // 1부터 2까지의 아이템 ID 랜덤 설정
            int itemCount = Random.Range(1, 4); // 1부터 3까지의 아이템 갯수 랜덤 설정
            int itemPrice = gameManager.GetItemPrice(itemID);

            newCustomer.SetRequirements(itemID, itemCount, itemPrice);

            customers.Add(newCustomer);
            SetCurrentCustomer(newCustomer);
        }
        else
        {
            Debug.LogError("선택된 CustomerPrefab에 Customer 스크립트가 없습니다.");
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
                SetItemImage(currentCustomer.RequiredItemID);
            }
        }
        else
        {
            Debug.LogError("현재 손님이 설정되지 않았습니다.");
        }
    }

    void SetItemImage(int itemID)
    {
        string imageName = "";

        switch (itemID)
        {
            case 1:
                imageName = "Bread_1";
                break;
            case 2:
                imageName = "Bread_2";
                break;
            default:
                Debug.LogError("알 수 없는 아이템 ID입니다: " + itemID);
                return;
        }

        Sprite itemSprite = Resources.Load<Sprite>("Images/" + imageName);
        if (itemSprite != null)
        {
            itemImage.sprite = itemSprite;
        }
        else
        {
            Debug.LogError("이미지를 로드할 수 없습니다: " + imageName);
        }
    }

    void SellItemToCustomer()
    {
        if (currentCustomer != null)
        {
            int itemID = currentCustomer.RequiredItemID;
            int itemCount = currentCustomer.GetRequiredItemCount();

            int currentBreadCount = SlotManager.GetTotalBreadCount(itemID);
            Debug.Log($"현재 빵의 수량: {currentBreadCount}, 요청된 빵의 수량: {itemCount}");

            if (currentBreadCount >= itemCount)
            {
                // 아이템 삭제 로직
                for (int i = 0; i < itemCount; i++)
                {
                    bool itemRemoved = false;
                    foreach (var slot in FindObjectsOfType<SlotManager>())
                    {
                        if (slot.state == SlotManager.SLOTSTATE.FULL && slot.BreadObject != null && slot.BreadObject.level == itemID)
                        {
                            Destroy(slot.BreadObject.gameObject);
                            slot.BreadObject = null;
                            slot.ChangeStateTo(SlotManager.SLOTSTATE.EMPTY); // 여기서 접근 오류 발생 가능성 있음
                            itemRemoved = true;
                            break;
                        }
                    }

                    if (!itemRemoved)
                    {
                        Debug.LogError($"아이템 ID {itemID}에 해당하는 BreadObject를 찾을 수 없습니다.");
                    }
                }

                SlotManager.RemoveBread(itemID, itemCount); // SlotManager에서 빵 제거
                int totalPrice = currentCustomer.RequiredItemPrice * itemCount;
                gameManager.AddCoin(totalPrice); // 코인 추가

                customers.Remove(currentCustomer);
                Destroy(currentCustomer.gameObject);
                currentCustomer = null;

                // 다음 손님 생성
                CreateCustomer();
            }
            else
            {
                Debug.Log("아이템이 충분하지 않습니다.");
            }
        }
    }
}
