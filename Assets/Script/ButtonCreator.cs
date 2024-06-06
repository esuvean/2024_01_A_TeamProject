using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab; // ������ ��ư ������
    private float timer = 0f;
    private bool buttonCreated = false;

    void Update()
    {
        // Ÿ�̸� ������Ʈ
        timer += Time.deltaTime;

        // 10�ʰ� ����ϰ� ���� ��ư�� �������� ���� ���
        if (timer >= 10f && !buttonCreated)
        {
            CreateButton(); // ��ư ����
            buttonCreated = true; // ��ư�� �����Ǿ����� ǥ��
        }
    }

    void CreateButton()
    {
        // ��ư ����
        GameObject button = Instantiate(buttonPrefab, transform.parent);
        // ������ ��ư�� ���� ���� (��: ��ġ, �ؽ�Ʈ ��)
        // ��ư�� � ������ ������ ���� ���� ���� (��: Ŭ�� �� �޼��� ȣ�� ��)
    }
}
