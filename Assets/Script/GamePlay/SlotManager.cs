using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    
    public enum SLOTSTATE       //���Ի��°�
    {
        EMPTY,
        FULL
    }

    public int id;                              //���� ��ȣ ID
    public Bread BreadObject;                     //������ Ŀ���� Class ID
    public SLOTSTATE state = SLOTSTATE.EMPTY;   //Enum �� ����

    private void ChangeStateTo(SLOTSTATE targetState)
    {//�ش� ������ ���°��� ��ȯ �����ִ� �Լ�
        state = targetState;
    }

    public void BreadGrabbed()
    {//RayCast�� ���ؼ� �������� ����� ��
        Destroy(BreadObject.gameObject);         //���� �������� ����
        ChangeStateTo(SLOTSTATE.EMPTY);         //������ �� ����

    }   
    
    public void CreateBread(int id)
    {
        
        //������ ��δ� (Assets/Resources/Prefabs/Bread_0)
        // Resoueces.Load(path) path = "Prefabs/Bread_0" �̷������� �ۼ��ؾ���.
        string BreadPath = "Prefabs/Bread_" + id.ToString("0");
                
        //var BreadGo = (GameObject)Instantiate(Resources.Load(BreadPath));
        // �� ������ ���ҽ� �ε� �� Object Ÿ������ ��ȯ�ϱ� ������ GameObject ������ Null Ref. Exception �߻���.
        var BreadGo = (GameObject)Instantiate(Resources.Load<GameObject>(BreadPath));

        BreadGo.transform.SetParent(this.transform);
        BreadGo.transform.localPosition = new Vector3(0f, 0f, -1f);
        BreadGo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //������ �� ������ �Է�
        BreadObject = BreadGo.GetComponent<Bread>();
        BreadObject.Init(id, this); //�Լ��� ���� �� �Է�(this -> Slot Class)

        ChangeStateTo(SLOTSTATE.FULL);

    }
}
