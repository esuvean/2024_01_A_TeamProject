using UnityEngine;
using UnityEngine.UI;

public class PrefabGenerator : MonoBehaviour
{
    public GameObject[] prefabOptions; // ������ �ɼǵ�
    public string[] textOptions; // �ؽ�Ʈ �ɼǵ�
    public string[] itemNames; // ������ �̸� �ɼǵ�
    public string[] dialogueOptions; // ��ȭ �ɼǵ�
    public Sprite[] imageOptions; // �̹��� �ɼǵ�
    public Transform spawnPoint; // ���� ��ġ

    void Start()
    {
        GeneratePrefab();
    }

    void GeneratePrefab()
    {
        // ������ ������ ����
        int randomPrefabIndex = Random.Range(0, prefabOptions.Length);
        GameObject prefabToSpawn = prefabOptions[randomPrefabIndex];

        // ������ ����
        GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // �ؽ�Ʈ ����
        Text textComponent = spawnedPrefab.GetComponentInChildren<Text>();
        if (textComponent != null && textOptions.Length > 0)
        {
            int randomTextIndex = Random.Range(0, textOptions.Length);
            string combinedText = dialogueOptions[0] + " " + itemNames[Random.Range(0, itemNames.Length)] + " " + Random.Range(1, 11).ToString() + " " + dialogueOptions[1];
            textComponent.text = combinedText;
        }

        // �̹��� ����
        Image imageComponent = spawnedPrefab.GetComponentInChildren<Image>();
        if (imageComponent != null && imageOptions.Length > 0)
        {
            // �ؽ�Ʈ�� ���õ� �̹��� ã��
            string itemName = textComponent.text.Split(' ')[1]; // �ؽ�Ʈ���� ������ �̸� ����
            int itemIndex = System.Array.IndexOf(itemNames, itemName); // ������ �̸��� �ִ� ��ġ ã��
            if (itemIndex != -1 && itemIndex < imageOptions.Length)
            {
                imageComponent.sprite = imageOptions[itemIndex]; // �ش��ϴ� �̹����� ����
            }
            else
            {
                Debug.LogWarning("�ش��ϴ� �̹����� ã�� �� �����ϴ�.");
            }
        }
    }
}
