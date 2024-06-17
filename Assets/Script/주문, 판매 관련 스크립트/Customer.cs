using UnityEngine;

public class Customer : MonoBehaviour
{
    private string greeting;
    public int requiredItemID; // 접근 권한을 public으로 변경
    private int requiredItemCount;
    public int requiredItemPrice; // 접근 권한을 public으로 변경

    public void SetRequirements(int itemID, int itemCount, int itemPrice)
    {
        requiredItemID = itemID;
        requiredItemCount = itemCount;
        requiredItemPrice = itemPrice;
    }

    public string GetGreeting()
    {
        return greeting;
    }

    public void SetGreeting(string text)
    {
        greeting = text;
    }

    public int GetRequiredItemCount()
    {
        return requiredItemCount;
    }
}
