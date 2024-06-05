using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab; // 생성할 버튼 프리팹
    private float timer = 0f;
    private bool buttonCreated = false;

    void Update()
    {
        // 타이머 업데이트
        timer += Time.deltaTime;

        // 10초가 경과하고 아직 버튼이 생성되지 않은 경우
        if (timer >= 10f && !buttonCreated)
        {
            CreateButton(); // 버튼 생성
            buttonCreated = true; // 버튼이 생성되었음을 표시
        }
    }

    void CreateButton()
    {
        // 버튼 생성
        GameObject button = Instantiate(buttonPrefab, transform.parent);
        // 생성된 버튼에 대한 설정 (예: 위치, 텍스트 등)
        // 버튼이 어떤 동작을 수행할 지에 대한 설정 (예: 클릭 시 메서드 호출 등)
    }
}
