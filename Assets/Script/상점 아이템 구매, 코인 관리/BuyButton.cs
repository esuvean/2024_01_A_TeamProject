using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public ItemShop itemShop; // ItemShop 스크립트에 접근하기 위한 참조
    public int itemID; // 아이템의 ID

    // 버튼을 클릭했을 때 호출되는 함수
    public void BuyItem()
    {
        // ItemShop 스크립트의 BuyItem 함수 호출
        itemShop.BuyItem(itemID); // 올바른 itemID를 전달
    }
}
