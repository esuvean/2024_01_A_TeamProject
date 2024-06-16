using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public Text timerText; // 타이머를 표시할 텍스트
    public Button endButton; // 게임 종료 버튼
    public Text endText; // 게임 종료 텍스트
    private float timeRemaining = 45f;
    private bool timerIsRunning = false;
    private GameController gameController; // GameController 참조

    void Start()
    {
        timerIsRunning = true;
        endButton.gameObject.SetActive(false); // 버튼을 초기에는 비활성화
        endText.gameObject.SetActive(false); // 게임 종료 텍스트를 초기에는 비활성화
        endButton.onClick.AddListener(ReturnToMapScene); // 버튼 클릭 이벤트 추가

        // GameController 찾기
        gameController = FindObjectOfType<GameController>();
        if (gameController == null)
        {
            Debug.LogError("GameController를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                EndGame();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1; // 타이머가 0에서 멈추지 않도록 하기 위해 1초를 더함
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndGame()
    {
        if (gameController != null)
        {
            gameController.isGameOver = true; // GameController에 게임 오버 상태 전달
            Debug.Log("Game Over");
        }
        endButton.gameObject.SetActive(true); // 타이머가 끝나면 버튼을 활성화
        endText.gameObject.SetActive(true); // 게임 종료 텍스트를 활성화
        Time.timeScale = 0; // 게임을 멈추기
    }

    void ReturnToMapScene()
    {
        Time.timeScale = 1; // 이전 씬으로 돌아가기 전에 게임 속도를 다시 정상으로
        SceneManager.LoadScene("MapScene"); // 이전 씬의 이름으로 변경하세요.
    }
}
