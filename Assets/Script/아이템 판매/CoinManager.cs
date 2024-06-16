using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int Coins { get; private set; } // 코인 변수

    private static CoinManager _instance;
    public static CoinManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Initialize(); // 이 부분을 추가하여 초기화 메서드를 호출
        }
    }

    private void Initialize()
    {
        Coins = 2000; // 초기 코인 설정
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    public void SubtractCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough coins!");
        }
    }

    public int GetCoins()
    {
        return Coins;
    }
}
