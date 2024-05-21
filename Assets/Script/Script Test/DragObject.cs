using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector2 mousePosition;

    private float offsetX, offsetY;

    public static bool mouseButtonReleased;

    private void OnMouseDown()
    {
        mouseButtonReleased = false;
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);
    }
    
    private void OnMouseUp()
    {
        mouseButtonReleased = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string thisGameObjectName;
        string collisionGameObjectName;

        thisGameObjectName = gameObject.name.Substring(0, name.IndexOf("_"));
        collisionGameObjectName = collision.gameObject.name.Substring(0, name.IndexOf("_"));

        if (mouseButtonReleased && thisGameObjectName == "Flour" && thisGameObjectName == collisionGameObjectName)
        {
            Instantiate(Resources.Load("MorningBread_Object"), transform.position, Quaternion.identity);
            mouseButtonReleased = false;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (mouseButtonReleased && thisGameObjectName == "MorningBread" && thisGameObjectName == collisionGameObjectName)
        {
            Instantiate(Resources.Load("PlainBread_Object"), transform.position, Quaternion.identity);
            mouseButtonReleased = false;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (mouseButtonReleased && thisGameObjectName == "PlainBread" && thisGameObjectName == collisionGameObjectName)
        {
            Instantiate(Resources.Load("Cupcake_Object"), transform.position, Quaternion.identity);
            mouseButtonReleased = false;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (mouseButtonReleased && thisGameObjectName == "Cupcake" && thisGameObjectName == collisionGameObjectName)
        {
            Instantiate(Resources.Load("Cake_Object"), transform.position, Quaternion.identity);
            mouseButtonReleased = false;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
