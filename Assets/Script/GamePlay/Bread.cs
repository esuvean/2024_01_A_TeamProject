using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public int level;
    public SlotManager parentSlot;

    private void Start()
    {
        // Renderer ������Ʈ ��������
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // ���� ���� �������ϵ��� sortingOrder ����
            renderer.sortingOrder = 9999; 
        }
    }
    public void Init(int level, SlotManager slot)
    {//������ ������ �Է��ϴ� �Լ�
        this.level = level;
        this.parentSlot = slot;
    }
}
