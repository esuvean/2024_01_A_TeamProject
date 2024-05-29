using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class For : MonoBehaviour
{
    int x;
    int y;

    void XY()
    {
        for (int i = 0; i < 24; i++)
        {
            x = (-30 + (15*((i - 1) % 6)));
            y = (-30 + (15*((i - 1) / 6)));
        }
    }

   
}
