using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItemNumber : MonoBehaviour
{
    public int itemNumber;
    public bool end;

    public void ButtonClick()
    {
        if (end)
            MainScript.instance.DisplayEndItem(itemNumber);

        else
            MainScript.instance.DisplayItem(itemNumber);
    }
}
