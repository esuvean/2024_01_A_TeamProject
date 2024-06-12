using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public SlotManager[] slots;                                //���� ��Ʈ�ѷ������� Slot �迭�� ����

    private Vector3 _target;
    private BreadManager carryingBread;                      //��� �ִ� ������ ���� �� ����

    public Dictionary<int, SlotManager> slotDictionary;       //Slot id, Slot class �����ϱ� ���� �ڷᱸ��

    public bool isGameOver;
    public float playTime;
    public Text TimeText;

    public float coin;
    public Text coinText;             //coin ���� 

    private void Start()
    {
        playTime = 0;
        coin = 2000;
        slotDictionary = new Dictionary<int, SlotManager>();   //�ʱ�ȭ

        for (int i = 0; i < slots.Length; i++)
        {                                               //�� ������ ID�� �����ϰ� ��ųʸ��� �߰�
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
            if (Input.GetMouseButtonDown(0)) //���콺 ���� ��
            {
                SendRayCast();
            }

            if (Input.GetMouseButton(0) && carryingBread)    //��� �̵���ų ��
            {
                OnBreadSelected();
            }

            if (Input.GetMouseButtonUp(0))  //���콺 ��ư�� ���� ��
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
                    var BreadGo = (GameObject)Instantiate(Resources.Load<GameObject>(BreadPath));  // ������ ����

                    BreadGo.transform.SetParent(this.transform);
                    BreadGo.transform.localPosition = Vector3.zero;
                    BreadGo.transform.localScale = Vector3.one * 5;

                    carryingBread = BreadGo.GetComponent<BreadManager>();  // ���� ���� �Է�
                    carryingBread.InitDummy(slot.id, slot.BreadObject.level);

                    slot.BreadGrabbed();
                }
                else if (slot.state == SlotManager.SLOTSTATE.EMPTY && carryingBread != null)
                {
                    slot.CreateBread(carryingBread.BreadLevel);  // ��� �ִ� �� ���� ��ġ�� ����
                    Destroy(carryingBread.gameObject);  // ��� �ִ� �� �ı�
                    carryingBread = null;  // carryingBread �ʱ�ȭ
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
                        OnBreadMergedWithTarget(slot.id);  // ���� �Լ� ȣ��
                    }
                    else
                    {
                        OnBreadCarryFail();  // ������ ��ġ ����
                    }
                }
            }
        }
        else
        {
            if (!carryingBread) return;
            OnBreadCarryFail();  // ������ ��ġ ����
        }
    }


    void OnBreadSelected()
    {   //�������� �����ϰ� ���콺 ��ġ�� �̵� 
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //��ǥ��ȯ
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
            Destroy(slot.BreadObject.gameObject);            //slot�� �ִ� ��ü �ı�
            slot.CreateBread(carryingBread.BreadLevel + 1);       //���Կ� ���� ��ȣ ��ü ����
            Destroy(carryingBread.gameObject);               //��� �ִ� ��ü �ı�
        }
        else
        {
            OnBreadCarryFail();
        }
    }

    void OnBreadCarryFail()
    {//������ ��ġ ���� �� ����
        var slot = GetSlotById(carryingBread.slotId);        //���� ��ġ Ȯ��
        slot.CreateBread(carryingBread.BreadLevel);               //�ش� ���Կ� �ٽ� ����
        Destroy(carryingBread.gameObject);                   //��� �ִ� ��ü �ı�
        carryingBread = null;                                // carryingBread �ʱ�ȭ
    }

    void PlaceRandomBread()
    {//������ ���Կ� ������ ��ġ
        if (AllSlotsOccupied())
        {
            return;
        }

        var rand = UnityEngine.Random.Range(0, slots.Length); //����Ƽ �����Լ��� �����ͼ� 0 ~ �迭 ũ�� ���� ��
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
    {//��� ������ ä���� �ִ��� Ȯ��
        foreach (var slot in slots)                       //foreach���� ���ؼ� Slots �迭�� �˻� ��
        {
            if (slot.state == SlotManager.SLOTSTATE.EMPTY)       //����ִ��� Ȯ��
            {
                return false;
            }
        }
        return true;
    }

    SlotManager GetSlotById(int id)
    {//���� ID�� ��ųʸ����� Slot Ŭ������ ����
        return slotDictionary[id];
    }
}