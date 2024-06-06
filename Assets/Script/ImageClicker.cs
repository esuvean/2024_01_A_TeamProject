using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageClicker : MonoBehaviour
{
    public Sprite[] images;
    private int currentIndex = 0;
    public Image imageDisplay;

    // 클릭 간격을 조절할 변수
    public float clickCooldown = 0.5f;
    private float clickTimer = 0f;

    private void Awake()
    {
        gameObject.SetActive(true);
    }
    void Start()
    {
        DisplayImage();
    }

    private void Update()
    {
        // 클릭 간격 타이머 업데이트
        clickTimer -= Time.deltaTime;

        // 클릭을 확인하고 타이머가 0 이하인 경우에만 이미지 변경을 시도합니다.
        if (Input.GetMouseButtonDown(0) && clickTimer <= 0f)
        {
            NextImage();
            // 클릭 간격 타이머를 리셋합니다.
            clickTimer = clickCooldown;
        }
    }

    public void NextImage()
    {
        currentIndex++;
        if (currentIndex >= images.Length)
        {
            currentIndex = 0;
        }
        DisplayImage();
    }

    void DisplayImage()
    {
        imageDisplay.sprite = images[currentIndex];
        Debug.Log("Image Displayed: " + images[currentIndex].name);
    }
}
