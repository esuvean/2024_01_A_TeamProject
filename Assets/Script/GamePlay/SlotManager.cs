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

    private void ChangeStateTo(SLOTSTATE targetState)
    {
        state = targetState;
    }

    public void BreadGrabbed()
    {
        if (BreadObject != null)
        {
            int breadLevel = BreadObject.level; // Bread의 레벨 정보 가져오기
            Destroy(BreadObject.gameObject); // Bread 오브젝트 제거
            BreadObject = null;
            ChangeStateTo(SLOTSTATE.EMPTY);

            // 인벤토리에서 Bread 수량 감소
            RemoveBreadFromInventory(breadLevel);
        }
    }

    public void CreateBread(int id)
    {
        string BreadPath = "Prefabs/Bread_" + id.ToString("0");
        var BreadGo = (GameObject)Instantiate(Resources.Load<GameObject>(BreadPath));

        BreadGo.transform.SetParent(this.transform);
        BreadGo.transform.localPosition = new Vector3(0f, 0f, -1f);
        BreadGo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        BreadObject = BreadGo.GetComponent<Bread>();
        BreadObject.Init(id, this);

        ChangeStateTo(SLOTSTATE.FULL);

        AddBreadToInventory(id);
    }

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

    public static int GetTotalBreadCount(int breadLevel)
    {
        return breadInventory.ContainsKey(breadLevel) ? breadInventory[breadLevel] : 0;
    }

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
    private void RemoveBreadFromInventory(int breadLevel)
    {
        if (breadInventory.ContainsKey(breadLevel))
        {
            breadInventory[breadLevel]--;
            if (breadInventory[breadLevel] <= 0)
            {
                breadInventory.Remove(breadLevel);
            }
            Debug.Log($"Bread Level {breadLevel} removed from inventory. Remaining: {(breadInventory.ContainsKey(breadLevel) ? breadInventory[breadLevel] : 0)}");
        }
    }
}
