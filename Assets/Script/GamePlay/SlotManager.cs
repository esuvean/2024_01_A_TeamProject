using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public enum SLOTSTATE
    {
        EMPTY,
        FULL
    }

    public int id;
    public Bread BreadObject;
    public SLOTSTATE state = SLOTSTATE.EMPTY;

    private static Dictionary<int, int> breadInventory = new Dictionary<int, int>();

    // 슬롯 상태를 변경하는 메서드
    public void ChangeStateTo(SLOTSTATE targetState)
    {
        state = targetState;
    }

    // 슬롯에 있는 빵을 가져가는 메서드
    public void BreadGrabbed()
    {
        if (BreadObject != null)
        {
            Destroy(BreadObject.gameObject);
            BreadObject = null;
            ChangeStateTo(SLOTSTATE.EMPTY);
        }
    }

    // 새로운 빵을 생성하여 슬롯에 배치하는 메서드
    public void CreateBread(int id)
    {
        string breadPath = "Prefabs/Bread_" + id.ToString("0");
        var breadGo = Instantiate(Resources.Load<GameObject>(breadPath));

        breadGo.transform.SetParent(this.transform);
        breadGo.transform.localPosition = new Vector3(0f, 0f, -1f);
        breadGo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        BreadObject = breadGo.GetComponent<Bread>();
        BreadObject.Init(id, this);

        ChangeStateTo(SLOTSTATE.FULL);

        AddBreadToInventory(id);
    }

    // 인벤토리에 빵을 추가하는 메서드
    private void AddBreadToInventory(int breadLevel)
    {
        if (breadInventory.ContainsKey(breadLevel))
        {
            breadInventory[breadLevel]++;
        }
        else
        {
            breadInventory[breadLevel] = 1;
        }
        Debug.Log($"Bread Level {breadLevel} added. Total: {breadInventory[breadLevel]}");
    }

    // 특정 레벨의 빵 개수를 반환하는 메서드
    public static int GetTotalBreadCount(int breadLevel)
    {
        return breadInventory.ContainsKey(breadLevel) ? breadInventory[breadLevel] : 0;
    }

    // 인벤토리에서 특정 레벨의 빵을 제거하는 메서드
    public static void RemoveBread(int breadLevel, int count)
    {
        if (breadInventory.ContainsKey(breadLevel))
        {
            breadInventory[breadLevel] -= count;
            if (breadInventory[breadLevel] <= 0)
            {
                breadInventory.Remove(breadLevel);
            }
            Debug.Log($"Bread Level {breadLevel} removed. Remaining: {(breadInventory.ContainsKey(breadLevel) ? breadInventory[breadLevel] : 0)}");
        }
    }
}
