using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab; // 손님 프리팹
    public Transform spawnPoint; // 손님이 생성될 위치
    public Text[] requirementTexts; // 요구사항 텍스트 배열

    private Customer currentCustomer; // 현재 생성된 손님

    void Start()
    {
        SpawnInitialCustomer();
    }

    void SpawnInitialCustomer()
    {
        SpawnCustomer();
    }

    void SpawnCustomer()
    {
        // 손님 생성
        GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        currentCustomer = customerObj.GetComponent<Customer>();

        // 손님의 요구사항 설정 (임시로 예시 설정)
        int itemID = Random.Range(1, 4); // 예시로 아이템 ID를 1부터 3까지로 설정
        int itemCount = Random.Range(1, 5); // 예시로 아이템 개수를 1부터 4까지로 설정
        int itemPrice = 100; // 예시로 아이템 가격을 100으로 설정
        string greetingText = "안녕하세요!";

        currentCustomer.SetGreeting(greetingText);
        currentCustomer.SetRequirements(itemID, itemCount, itemPrice);

        // 요구사항 텍스트 업데이트
        UpdateRequirementTexts();
    }

    void UpdateRequirementTexts()
    {
        requirementTexts[0].text = $"인사: {currentCustomer.GetGreeting()}";
        requirementTexts[1].text = $"아이템 ID: {currentCustomer.requiredItemID}";
        requirementTexts[2].text = $"아이템 개수: {currentCustomer.GetRequiredItemCount()}";
        requirementTexts[3].text = $"아이템 가격: {currentCustomer.requiredItemPrice}";
    }

    // 다음 손님 생성 메서드
    public void SpawnNextCustomer()
    {
        Destroy(currentCustomer.gameObject); // 현재 손님 제거
        SpawnCustomer(); // 새로운 손님 생성
    }
}
