using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MargeTest : MonoBehaviour
{
    private bool isClick;

    private void OnMouseDown()
    {
        isClick = false;
    }
    private void OnMouseDrag()
    {
        Vector3 vpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vpos.z = 0f;
        transform.position = vpos;
    }
    private void OnMouseUp()
    {
        isClick = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        string clickObject = transform.name.Substring(transform.name.LastIndexOf("_")+1);
        string collisionObject = collision.name.Substring(collision.name.LastIndexOf("_") + 1);
        int codeNumber=int.Parse(clickObject)+int.Parse(collisionObject);

        if(isClick&&clickObject==collisionObject)
        {
            GameObject newObject = (GameObject)Instantiate(Resources.Load("ItemCode_" + codeNumber), transform.position, Quaternion.identity);
            newObject.name = "ItemCode_" + codeNumber;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
