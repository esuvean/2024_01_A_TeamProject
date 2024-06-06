using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    
    public enum SLOTSTATE       //슬롯상태값
    {
        EMPTY,
        FULL
    }

    public int id;                              //슬롯 번호 ID
    public Bread BreadObject;                     //선언한 커스텀 Class ID
    public SLOTSTATE state = SLOTSTATE.EMPTY;   //Enum 값 선언

    private void ChangeStateTo(SLOTSTATE targetState)
    {//해당 슬롯의 상태값을 변환 시켜주는 함수
        state = targetState;
    }

    public void BreadGrabbed()
    {//RayCast를 통해서 아이템을 잡았을 때
        Destroy(BreadObject.gameObject);         //기존 아이템을 삭제
        ChangeStateTo(SLOTSTATE.EMPTY);         //슬롯은 빈 상태

    }   
    
    public void CreateBread(int id)
    {
        
        //아이템 경로는 (Assets/Resources/Prefabs/Bread_0)
        // Resoueces.Load(path) path = "Prefabs/Bread_0" 이런식으로 작성해야함.
        string BreadPath = "Prefabs/Bread_" + id.ToString("0");
                
        //var BreadGo = (GameObject)Instantiate(Resources.Load(BreadPath));
        // 본 형식은 리소스 로드 시 Object 타입으로 반환하기 때문에 GameObject 생성시 Null Ref. Exception 발생함.
        var BreadGo = (GameObject)Instantiate(Resources.Load<GameObject>(BreadPath));

        BreadGo.transform.SetParent(this.transform);
        BreadGo.transform.localPosition = new Vector3(0f, 0f, -1f);
        BreadGo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //아이템 값 정보를 입력
        BreadObject = BreadGo.GetComponent<Bread>();
        BreadObject.Init(id, this); //함수를 통한 값 입력(this -> Slot Class)

        ChangeStateTo(SLOTSTATE.FULL);

    }
}
