using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadManager : MonoBehaviour
{//���� ���� ���� ���� ������ �ִ� Ŭ����
    public int slotId;      //���� ��ȣ (Slot Ŭ���� ���� ��)
    public int BreadLevel;      //������ ��ȣ


    public void InitDummy(int slotId, int BreadLevel)
    {//�μ��� ���� ������ Class�ʿ� �Է�
        this.slotId = slotId;
        this.BreadLevel = BreadLevel;
    }
}
