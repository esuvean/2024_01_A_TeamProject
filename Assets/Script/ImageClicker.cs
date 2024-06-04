using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageClicker : MonoBehaviour
{
    public Sprite[] images; // 이미지 배열
    private int currentIndex = 0; // 현재 이미지 인덱스
    private bool hasDisplayed = false; // 이미지가 한 번 표시되었는지 여부

    // 이미지를 표시할 UI 이미지
    public Image imageDisplay;

    void Start()
    {
        DisplayImage();
    }

    void Update()
    {
        // 이미지가 한 번 표시되었고, 마우스 왼쪽 버튼이 클릭되었다면 다음 이미지로 전환
        if (hasDisplayed && Input.GetMouseButtonDown(0))
        {
            currentIndex++;
            // 이미지 인덱스가 이미지 배열의 범위를 벗어나면 초기화
            if (currentIndex >= images.Length)
            {
                currentIndex = 0;
            }
            DisplayImage();
        }

        // 이미지가 한 번 표시되었고, 마우스 왼쪽 버튼이 클릭되었다면 다음 이미지로 전환
        if (hasDisplayed && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse Clicked!"); // 디버그 로그 추가
            currentIndex++;
            // 이미지 인덱스가 이미지 배열의 범위를 벗어나면 초기화
            if (currentIndex >= images.Length)
            {
                currentIndex = 0;
            }
            DisplayImage();
        }
    }

    void DisplayImage()
    {
        // 이미지가 한 번 표시되었으면 리턴
        if (hasDisplayed)
            return;

        // 이미지 표시
        imageDisplay.sprite = images[currentIndex];
        Debug.Log("Image Displayed: " + images[currentIndex].name); // 디버그 로그 추가
        hasDisplayed = true;
    }
}
