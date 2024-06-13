using UnityEngine;
using UnityEngine.UI;

public class PrefabGenerator : MonoBehaviour
{
    public GameObject[] prefabOptions; // 프리팹 옵션들
    public string[] textOptions; // 텍스트 옵션들
    public string[] itemNames; // 아이템 이름 옵션들
    public string[] dialogueOptions; // 대화 옵션들
    public Sprite[] imageOptions; // 이미지 옵션들
    public Transform spawnPoint; // 생성 위치

    void Start()
    {
        GeneratePrefab();
    }

    void GeneratePrefab()
    {
        // 랜덤한 프리팹 선택
        int randomPrefabIndex = Random.Range(0, prefabOptions.Length);
        GameObject prefabToSpawn = prefabOptions[randomPrefabIndex];

        // 프리팹 생성
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // 텍스트 생성
        Text textComponent = spawnedPrefab.GetComponentInChildren<Text>();
        if (textComponent != null && textOptions.Length > 0)
        {
            int randomTextIndex = Random.Range(0, textOptions.Length);
            string combinedText = dialogueOptions[0] + " " + itemNames[Random.Range(0, itemNames.Length)] + " " + Random.Range(1, 11).ToString() + " " + dialogueOptions[1];
            textComponent.text = combinedText;
        }

        // 이미지 생성
        Image imageComponent = spawnedPrefab.GetComponentInChildren<Image>();
        if (imageComponent != null && imageOptions.Length > 0)
        {
            // 텍스트와 관련된 이미지 찾기
            string itemName = textComponent.text.Split(' ')[1]; // 텍스트에서 아이템 이름 추출
            int itemIndex = System.Array.IndexOf(itemNames, itemName); // 아이템 이름이 있는 위치 찾기
            if (itemIndex != -1 && itemIndex < imageOptions.Length)
            {
                imageComponent.sprite = imageOptions[itemIndex]; // 해당하는 이미지로 설정
            }
            else
            {
                Debug.LogWarning("해당하는 이미지를 찾을 수 없습니다.");
            }
        }
    }
}
