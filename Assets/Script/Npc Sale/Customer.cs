using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer2D : MonoBehaviour
{
    public int money = 500; // 손님의 소지금액

    public bool BuyItem(int price)
    {
        if (money >= price)
        {
            money -= price;
            Debug.Log("Bought item for " + price + " dollars. Remaining money: " + money);
            return true;
        }
        else
        {
            Debug.Log("Not enough money to buy item");
            return false;
        }
    }
}
