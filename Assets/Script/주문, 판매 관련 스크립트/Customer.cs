using UnityEngine;

public class Customer : MonoBehaviour
{
    private string greeting;
    private string closing;
    private int requiredItemID;
    private int requiredItemCount;
    private int requiredItemPrice;

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

    public string GetClosing()
    {
        return closing;
    }

    public void SetClosing(string text)
    {
        closing = text;
    }

    public int GetRequiredItemCount()
    {
        return requiredItemCount;
    }

    public int RequiredItemID
    {
        get { return requiredItemID; }
    }

    public int RequiredItemPrice
    {
        get { return requiredItemPrice; }
    }
}
