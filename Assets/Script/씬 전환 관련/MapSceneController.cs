using UnityEngine;
using UnityEngine.UI;

public class MapSceneController : MonoBehaviour
{
    public Text customerCountText; // 고객 수를 표시하는 텍스트 UI

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        UpdateUI();
    }

    void Update()
    {
        // 예시: 매 프레임마다 UI를 업데이트
        UpdateUI();
    }

    void UpdateUI()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager 인스턴스를 가져올 수 없습니다.");
            return;
        }

        // GameManager에서 고객 수 가져오기
        int customerCount = gameManager.GetInventoryItemCount(1); // 예시로 GetInventoryItemCount 사용

        // 고객 수를 텍스트 UI에 표시
        customerCountText.text = $"고객 수: {customerCount}";
    }
}
