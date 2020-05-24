using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    [SerializeField] private MainScript ms;
    [SerializeField] private Text text;

    private int num;

    private void Awake()
    {
        num = 10;
    }

    public void UpdateNum(int i)
    {
        if (num + i >= 3)
        {
            num += i;
            text.text = num.ToString();
        }
    }

    public void EnterItems()
    {
        ms.SetNumberOfItems(num);
    }
}
