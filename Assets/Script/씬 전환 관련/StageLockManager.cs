using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageLockManager : MonoBehaviour
{
    public Button stage3Button;
    public Button stage4Button;
    public Button stage5Button;

    private bool stage3Unlocked = false; // Stage 3의 잠금 상태
    private bool stage4Unlocked = false; // Stage 4의 잠금 상태
    private bool stage5Unlocked = false; // Stage 5의 잠금 상태

    void Start()
    {
        UpdateStageButtons(); // 버튼 상태 초기화
    }

    void UpdateStageButtons()
    {
        stage3Button.interactable = stage3Unlocked; // Stage 3 버튼 활성화 여부 설정
        stage4Button.interactable = stage4Unlocked; // Stage 4 버튼 활성화 여부 설정
        stage5Button.interactable = stage5Unlocked; // Stage 5 버튼 활성화 여부 설정
    }

    public void UnlockStage(int stageNumber)
    {
        switch (stageNumber)
        {
            case 3:
                stage3Unlocked = true;
                break;
            case 4:
                stage4Unlocked = true;
                break;
            case 5:
                stage5Unlocked = true;
                break;
            default:
                Debug.LogWarning("잘못된 Stage 번호입니다: " + stageNumber);
                break;
        }

        UpdateStageButtons(); // 업데이트된 버튼 상태 적용
    }

    public void LoadStage(int stageNumber)
    {
        string sceneName = "Stage" + stageNumber + "Scene";

        if (IsStageUnlocked(stageNumber))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Stage " + stageNumber + "은(는) 잠겨 있습니다. 해제 후 접근할 수 있습니다.");
        }
    }

    bool IsStageUnlocked(int stageNumber)
    {
        switch (stageNumber)
        {
            case 3:
                return stage3Unlocked;
            case 4:
                return stage4Unlocked;
            case 5:
                return stage5Unlocked;
            default:
                Debug.LogWarning("잘못된 Stage 번호입니다: " + stageNumber);
                return false;
        }
    }
}
