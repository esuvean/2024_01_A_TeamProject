using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SlotManager[] slots; // 슬롯 매니저들의 배열

    private GameManager gameManager;
    private BreadManager carryingBread; // 현재 들고 있는 빵

    public Dictionary<int, SlotManager> slotDictionary; // 슬롯 ID와 슬롯 매니저를 매핑한 딕셔너리

    public bool isGameOver;
    public Text coinText; // 코인 텍스트
    public string coinTextName = "CoinText"; // 코인 텍스트 오브젝트 이름

    private int customerCount = 0; // 손님 수를 저장하는 변수
    public Button actionButton; // 특정 조건에서 활성화될 버튼

    private void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }
        coinText = GameObject.Find(coinTextName)?.GetComponent<Text>();
        if (coinText == null)
        {
            Debug.LogError("CoinText를 찾을 수 없습니다.");
        }

        slotDictionary = new Dictionary<int, SlotManager>(); // 초기화

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }

        UpdateCoinText();
        UpdateActionButton(); // 시작 시 버튼 상태 업데이트
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateBread();
        }

        if (Time.timeScale > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && carryingBread != null)
            {
                OnBreadSelected();
            }

            if (Input.GetMouseButtonUp(0))
            {
                SendRayCast();
            }
        }
    }

    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<SlotManager>();
            if (slot != null)
            {
                if (slot.state == SlotManager.SLOTSTATE.FULL && carryingBread == null)
                {
                    if (slot.BreadObject == null)
                    {
                        return;
                    }

                    string breadPath = "Prefabs/Bread_Grabbed_" + slot.BreadObject.level.ToString("0");
                    var breadGO = Instantiate(Resources.Load<GameObject>(breadPath));
                    breadGO.transform.SetParent(transform);
                    breadGO.transform.localPosition = Vector3.zero;
                    breadGO.transform.localScale = Vector3.one * 5;

                    carryingBread = breadGO.GetComponent<BreadManager>();
                    carryingBread.InitDummy(slot.id, slot.BreadObject.level);

                    slot.BreadGrabbed();
                }
                else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingBread != null)
                {
                    slot.CreateBread(carryingBread.BreadLevel);
                    Destroy(carryingBread.gameObject);
                    carryingBread = null;
                }
                else if (slot.state == SlotManager.SLOTSTATE.FULL && carryingBread != null)
                {
                    if (slot.BreadObject == null)
                    {
                        OnBreadCarryFail();
                        return;
                    }

                    if (slot.BreadObject.level == carryingBread.BreadLevel)
                    {
                        OnBreadMergedWithTarget(slot.id);
                    }
                    else
                    {
                        OnBreadCarryFail();
                    }
                }
            }
        }
        else
        {
            if (carryingBread != null)
            {
                OnBreadCarryFail();
            }
        }
    }

    void OnBreadSelected()
    {
        Vector3 _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _target.z = 0;
        float delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingBread.transform.position = Vector3.MoveTowards(carryingBread.transform.position, _target, delta);
    }

    void OnBreadMergedWithTarget(int targetSlotId)
    {
        if (carryingBread.BreadLevel < 4)
        {
            SlotManager slot = GetSlotById(targetSlotId);
            Destroy(slot.BreadObject.gameObject);
            slot.CreateBread(carryingBread.BreadLevel + 1);
            Destroy(carryingBread.gameObject);
        }
        else
        {
            OnBreadCarryFail();
        }
    }

    void OnBreadCarryFail()
    {
        SlotManager slot = GetSlotById(carryingBread.slotId);
        slot.CreateBread(carryingBread.BreadLevel);
        Destroy(carryingBread.gameObject);
        carryingBread = null;
    }

    void PlaceRandomBread()
    {
        if (AllSlotsOccupied())
        {
            return;
        }

        int rand = Random.Range(0, slots.Length);
        SlotManager slot = GetSlotById(rand);
        while (slot.state == SlotManager.SLOTSTATE.FULL)
        {
            rand = Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }
        slot.CreateBread(0);
    }

    public void CreateBread()
    {
        if (gameManager.GetCoins() >= 100)
        {
            gameManager.SubtractCoin(100); // 코인 감소
            PlaceRandomBread(); // 빵 생성
            UpdateCoinText(); // 코인 UI 업데이트
        }
        else
        {
            Debug.Log("코인이 부족합니다!");
        }
    }

    SlotManager GetSlotById(int id)
    {
        return slotDictionary[id];
    }

    bool AllSlotsOccupied()
    {
        foreach (SlotManager slot in slots)
        {
            if (slot.state == SlotManager.SLOTSTATE.EMPTY)
            {
                return false;
            }
        }
        return true;
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = gameManager.GetCoins().ToString();
        }
        else
        {
            Debug.LogError("CoinText가 할당되지 않았습니다.");
        }
    }

    public void IncrementCustomerCount()
    {
        customerCount++;
        UpdateActionButton(); // 손님 수가 증가할 때 버튼 상태 업데이트
    }

    void UpdateActionButton()
    {
        if (actionButton != null)
        {
            actionButton.interactable = customerCount >= 4; // 손님 수가 4명 이상일 때 버튼 활성화
        }
    }
}
