using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SlotManager[] slots; // 슬롯 매니저들을 담은 배열
    public CoinManager coinManager; // 코인 매니저

    private Vector3 _target;
    private BreadManager carryingBread; // 들고 있는 빵을 담을 변수

    public Dictionary<int, SlotManager> slotDictionary; // Slot id, Slot class 딕셔너리

    public bool isGameOver;

    private void Start()
    {
        slotDictionary = new Dictionary<int, SlotManager>(); // 초기화

        for (int i = 0; i < slots.Length; i++)
        { // 각 슬롯에 ID를 부여하고 딕셔너리에 추가
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
        Debug.Log("GameController 초기화 완료");
    }

    void Update()
    {
        if (isGameOver)
        {
            Debug.Log("게임 오버 상태");
            return; // 게임 오버 상태면 Update 함수의 나머지 부분 실행 안 함
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateBread();
        }

        if (Time.timeScale > 0)
        {
            if (Input.GetMouseButtonDown(0)) // 마우스 버튼 클릭 시
            {
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && carryingBread) // 빵 이동 중
            {
                OnBreadSelected();
            }

            if (Input.GetMouseButtonUp(0)) // 마우스 버튼 해제 시
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

                    string BreadPath = "Prefabs/Bread_Grabbed_" + slot.BreadObject.level.ToString("0");
                    var BreadGo = (GameObject)Instantiate(Resources.Load<GameObject>(BreadPath)); // 빵 생성

                    BreadGo.transform.SetParent(this.transform);
                    BreadGo.transform.localPosition = Vector3.zero;
                    BreadGo.transform.localScale = Vector3.one * 5;

                    carryingBread = BreadGo.GetComponent<BreadManager>(); // 빵 매니저 할당
                    carryingBread.InitDummy(slot.id, slot.BreadObject.level);

                    slot.BreadGrabbed();
                }
                else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingBread != null)
                {
                    slot.CreateBread(carryingBread.BreadLevel); // 들고 있는 빵을 슬롯에 배치
                    Destroy(carryingBread.gameObject); // 들고 있는 빵 삭제
                    carryingBread = null; // 초기화
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
                        OnBreadMergedWithTarget(slot.id); // 빵 합치기
                    }
                    else
                    {
                        OnBreadCarryFail(); // 빵 배치 실패
                    }
                }
            }
        }
        else
        {
            if (!carryingBread) return;
            OnBreadCarryFail(); // 빵 배치 실패
        }
    }

    void OnBreadSelected()
    { // 빵을 선택하고 마우스 위치로 이동
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 위치 변환
        _target.z = 0;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingBread.transform.position = Vector3.MoveTowards(carryingBread.transform.position, _target, delta);
    }

    void OnBreadMergedWithTarget(int targetSlotId)
    {
        if (carryingBread.BreadLevel < 4)
        {
            var slot = GetSlotById(targetSlotId);
            Destroy(slot.BreadObject.gameObject); // 슬롯에 있는 오브젝트 삭제
            slot.CreateBread(carryingBread.BreadLevel + 1); // 새로운 빵 생성
            Destroy(carryingBread.gameObject); // 들고 있는 빵 삭제
        }
        else
        {
            OnBreadCarryFail();
        }
    }

    void OnBreadCarryFail()
    { // 빵 배치 실패 시
        var slot = GetSlotById(carryingBread.slotId); // 원래 슬롯 확인
        slot.CreateBread(carryingBread.BreadLevel); // 해당 슬롯에 다시 생성
        Destroy(carryingBread.gameObject); // 들고 있는 빵 삭제
        carryingBread = null; // 초기화
    }

    void PlaceRandomBread()
    { // 랜덤한 슬롯에 빵 배치
        if (AllSlotsOccupied())
        {
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length); // 랜덤 값 생성
        var slot = GetSlotById(rand);
        while (slot.state == SlotManager.SLOTSTATE.FULL)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }
        slot.GetComponent<SlotManager>().CreateBread(0);
    }

    public void CreateBread()
    {
        if (coinManager.GetCoins() >= 100)
        {
            PlaceRandomBread();
            coinManager.SubtractCoins(100); // Coins 메서드를 SubtractCoins로 수정
        }
    }

    bool AllSlotsOccupied()
    { // 모든 슬롯이 차 있는지 확인
        foreach (var slot in slots) // foreach로 모든 슬롯 검사
        {
            if (slot.state == SlotManager.SLOTSTATE.EMPTY) // 비어있는 슬롯 확인
            {
                return false;
            }
        }
        return true;
    }

    SlotManager GetSlotById(int id)
    { // 슬롯 ID로 SlotManager 클래스 반환
        return slotDictionary[id];
    }
}
