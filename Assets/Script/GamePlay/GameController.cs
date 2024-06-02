using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{   
    public SlotManager[] slots;                                //게임 컨트롤러에서는 Slot 배열을 관리

    private Vector3 _target;
    private BreadManager carryingBread;                      //잡고 있는 아이템 정보 값 관리

    public Dictionary<int, SlotManager> slotDictionary;       //Slot id, Slot class 관리하기 위한 자료구조

    public bool isGameOver;
    public float playTime;
    public Text TimeText;

    public float coin;
    public Text coinText;             //coin 관리 

    private void Start()
    {
        playTime = 0;
        coin = 10000;
        slotDictionary = new Dictionary<int, SlotManager>();   //초기화

        for (int i = 0; i < slots.Length; i++)
        {                                               //각 슬롯의 ID를 설정하고 딕셔너리에 추가
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            playTime += Time.deltaTime;
            TimeText.text = " : " + (int)playTime;
            coinText.text = " : " + coin;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateBread();
        }

        if (Time.timeScale > 0)
        {
            if (Input.GetMouseButtonDown(0)) //마우스 누를 때
            {
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && carryingBread)    //잡고 이동시킬 때
            {
                OnBreadSelected();
            }

            if (Input.GetMouseButtonUp(0))  //마우스 버튼을 놓을 때
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
                    var BreadGo = (GameObject)Instantiate(Resources.Load<GameObject>(BreadPath));  // 아이템 생성

                    BreadGo.transform.SetParent(this.transform);
                    BreadGo.transform.localPosition = Vector3.zero;
                    BreadGo.transform.localScale = Vector3.one * 5;

                    carryingBread = BreadGo.GetComponent<BreadManager>();  // 슬롯 정보 입력
                    carryingBread.InitDummy(slot.id, slot.BreadObject.level);

                    slot.BreadGrabbed();
                }
                else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingBread != null)
                {
                    slot.CreateBread(carryingBread.BreadLevel);  // 잡고 있는 것 슬롯 위치에 생성
                    Destroy(carryingBread.gameObject);  // 잡고 있는 것 파괴
                    carryingBread = null;  // carryingBread 초기화
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
                        OnBreadMergedWithTarget(slot.id);  // 병합 함수 호출
                    }
                    else
                    {
                        OnBreadCarryFail();  // 아이템 배치 실패
                    }
                }
            }
        }
        else
        {
            if (!carryingBread) return;
            OnBreadCarryFail();  // 아이템 배치 실패
        }
    }


    void OnBreadSelected()
    {   //아이템을 선택하고 마우스 위치로 이동 
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //좌표변환
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
            Destroy(slot.BreadObject.gameObject);            //slot에 있는 물체 파괴
            slot.CreateBread(carryingBread.BreadLevel + 1);       //슬롯에 다음 번호 물체 생성
            Destroy(carryingBread.gameObject);               //잡고 있는 물체 파괴
        }
        else
        {
            OnBreadCarryFail();
        }
    }

    void OnBreadCarryFail()
    {//아이템 배치 실패 시 실행
        var slot = GetSlotById(carryingBread.slotId);        //슬롯 위치 확인
        slot.CreateBread(carryingBread.BreadLevel);               //해당 슬롯에 다시 생성
        Destroy(carryingBread.gameObject);                   //잡고 있는 물체 파괴
        carryingBread = null;                                // carryingBread 초기화
    }

    void PlaceRandomBread()
    {//랜덤한 슬롯에 아이템 배치
        if (AllSlotsOccupied())
        {
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length); //유니티 랜덤함수를 가져와서 0 ~ 배열 크기 사이 값
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
        if (coin >= 100)
        {
            PlaceRandomBread();
            coin -= 100;
        }
    }

    bool AllSlotsOccupied()
    {//모든 슬롯이 채워져 있는지 확인
        foreach(var slot in slots)                       //foreach문을 통해서 Slots 배열을 검사 후
        {
            if (slot.state == SlotManager.SLOTSTATE.EMPTY)       //비어있는지 확인
            {
                return false;
            }
        }
        return true;
    }

    SlotManager GetSlotById(int id)
    {//슬롯 ID로 딕셔너리에서 Slot 클래스를 리턴
        return slotDictionary[id];
    }
}
