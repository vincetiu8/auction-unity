using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    public static MainScript instance;

    [SerializeField] private GameObject[] screens;
    [SerializeField] private Text text, errorText;
    [SerializeField] private InputField numberText, descText, priceText, searchText;
    [SerializeField] private Button button;
    [SerializeField] private Transform contentHolder;

    [SerializeField] private GameObject panel;
    [SerializeField] private Text itemText, descriptText, highBidText, errText;
    [SerializeField] private InputField bidAmount, buyerNo;

    [SerializeField] private Transform endContentHolder, endPanel;
    [SerializeField] private Button endButton;
    [SerializeField] private Text endErrorText, endItemNumber, endItemDescription, endText1, endText2, endText3, endText4;
    [SerializeField] private InputField endSearchText;

    private int[] number;
    private string[] desc;
    private float[] price;
    private int[] bids;
    private float[] highbid;
    private int[] buyerNumber;

    private int displayedItem;
    private int activeScreen;
    private int numberOfItems;
    private int activeItem;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(this);

        activeItem = 0;
        activeScreen = 0;
        ActivateScreen();
    }

    private void ActivateScreen()
    {
        for (int i = 0; i < screens.Length; i++)
        {
            if (i == activeScreen)
                screens[i].SetActive(true);

            else
                screens[i].SetActive(false);
        }

        if (activeScreen == 3)
            SetUpButtons();
    }

    public void ToScreen(int _screenID)
    {
        activeScreen = _screenID;
        ActivateScreen();
    }

    public void SetNumberOfItems(int _numberOfItems)
    {
        numberOfItems = _numberOfItems;
        number = new int[numberOfItems];
        desc = new string[numberOfItems];
        price = new float[numberOfItems];
        bids = new int[numberOfItems];
        for (int i = 0; i < numberOfItems; i++)
            bids[i] = 0;
        highbid = new float[numberOfItems];
        buyerNumber = new int[numberOfItems];
        ToScreen(2);
    }

    public void EnterData()
    {
        number[activeItem] = int.Parse(numberText.text);
        desc[activeItem] = descText.text;
        double _price = Math.Round(float.Parse(priceText.text), 2);
        price[activeItem] = (float) _price;
        numberText.text = "";
        descText.text = "";
        priceText.text = "";
        activeItem++;
        if (activeItem == numberOfItems)
            ToScreen(3);

        else
            text.text = "Item " + (activeItem + 1);
    }

    private void SetUpButtons()
    {
        panel.SetActive(false);
        foreach (Transform _child in contentHolder.GetComponentsInChildren<Transform>())
        {
            if (_child != contentHolder && _child != errorText)
                Destroy(_child.gameObject);
        }

        for (int i = 0; i < numberOfItems; i++)
        {
            Button _button = Instantiate(button, contentHolder);
            _button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 125 - i * 50);
            _button.GetComponentInChildren<Text>().text = "Item " + number[i];
            _button.GetComponent<ButtonItemNumber>().itemNumber = i;
        }
    }

    public void FindItem()
    {
        panel.SetActive(false);

        foreach (Transform _child in contentHolder.GetComponentsInChildren<Transform>())
        {
            if (_child != contentHolder && _child != errorText)
                Destroy(_child.gameObject);
        }

        if (searchText.text == "")
            SetUpButtons();
        
        else
        {
            int counter = 0;

            for (int i = 0; i < numberOfItems; i++)
            {
                if (number[i].ToString().Substring(0, searchText.text.Length) == searchText.text)
                {
                    Button _button = Instantiate(button, contentHolder);
                    _button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 125 - counter * 50);
                    _button.GetComponentInChildren<Text>().text = "Item " + number[i];
                    _button.GetComponent<ButtonItemNumber>().itemNumber = i;
                    counter++;
                }
            }

            if (counter == 0)
            {
                errorText.gameObject.SetActive(true);

                errorText.text = "No items found.";
            }

            else
            {
                errorText.gameObject.SetActive(false);
            }
        }
    }

    public void DisplayItem(int _itemID)
    {
        errorText.gameObject.SetActive(false);
        displayedItem = _itemID;
        panel.SetActive(true);
        itemText.text = number[_itemID].ToString();
        descriptText.text = desc[_itemID];
        highBidText.text = "Highest Bid: " + ((double) highbid[_itemID]).ToString();
        bidAmount.text = "";
    }

    public void EnterBid()
    {
        if (Math.Round(float.Parse(bidAmount.text), 2) >= highbid[displayedItem])
        {
            errorText.gameObject.SetActive(false);
            bids[displayedItem]++;
            highbid[displayedItem] = float.Parse(bidAmount.text);
            buyerNumber[displayedItem] = int.Parse(buyerNo.text);
            highBidText.text = highbid[displayedItem].ToString();
            DisplayItem(displayedItem);
        }

        else
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Entered bid price is lower than highest bid price.";
        }
    }

    public void EndAuction()
    {
        ToScreen(4);

        SetupEndButtons();
    }

    private void SetupEndButtons()
    {
        endPanel.gameObject.SetActive(false);

        foreach (Transform _child in endContentHolder.GetComponentsInChildren<Transform>())
        {
            if (_child != endContentHolder && _child != endErrorText)
                Destroy(_child.gameObject);
        }

        Button _butt = Instantiate(endButton, endContentHolder);
        _butt.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 125);
        _butt.GetComponentInChildren<Text>().text = "Item Summary";
        _butt.GetComponent<ButtonItemNumber>().itemNumber = -1;

        for (int i = 0; i < numberOfItems; i++)
        {
            Button _button = Instantiate(endButton, endContentHolder);
            _button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 125 - (i + 1) * 50);
            _button.GetComponentInChildren<Text>().text = "Item " + number[i];
            _button.GetComponent<ButtonItemNumber>().itemNumber = i;
        }
    }

    public void FindEndItem()
    {
        endPanel.gameObject.SetActive(false);

        foreach (Transform _child in endContentHolder.GetComponentsInChildren<Transform>())
        {
            if (_child != endContentHolder && _child != endErrorText)
                Destroy(_child.gameObject);
        }

        if (searchText.text == "")
            SetUpButtons();

        else
        {
            int counter = 0;

            for (int i = 0; i < numberOfItems; i++)
            {
                if (number[i].ToString().Substring(0, endSearchText.text.Length) == endSearchText.text)
                {
                    Button _button = Instantiate(button, endContentHolder);
                    _button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 125 - counter * 50);
                    _button.GetComponentInChildren<Text>().text = "Item " + number[i];
                    _button.GetComponent<ButtonItemNumber>().itemNumber = i;
                    counter++;
                }
            }

            if (counter == 0)
            {
                endErrorText.gameObject.SetActive(true);

                errorText.text = "No items found.";
            }

            else
            {
                endErrorText.gameObject.SetActive(false);
            }
        }
    }

    public void DisplayEndItem(int _itemID)
    {
        endPanel.gameObject.SetActive(true);

        if (_itemID == -1)
        {
            endItemNumber.text = "Auction Summary";
            endItemDescription.text = "";

            int _itemsSold = 0;
            int _itemsBidded = 0;

            for (int i = 0; i < numberOfItems; i++)
            {
                if (price[i] <= highbid[i])
                    _itemsSold++;

                else if (bids[i] > 0)
                    _itemsBidded++;
            }

            endText1.text = "Item Breakdown";
            endText2.text = "Sold: " + _itemsSold;
            endText3.text = "Insufficient Bids: " + _itemsBidded;
            endText4.text = "No Bids: " + (numberOfItems - _itemsSold - _itemsBidded);
            return;
        }

        endItemNumber.text = number[_itemID].ToString();
        endItemDescription.text = desc[_itemID];

        if (bids[_itemID] == 0)
        {
            endText1.text = "No Bids";
            endText2.text = "";
            endText3.text = "";
            endText4.text = "";
        }

        else if (price[_itemID] <= highbid[_itemID])
        {
            endText1.text = "Successfully Auctioned";
            endText2.text = "Final Bid: " + (double) highbid[_itemID];
            endText3.text = "Total Fee: " + (highbid[_itemID] + Math.Round(highbid[_itemID] * 0.1f, 2));
            endText4.text = "Final Bidder: " + buyerNumber[_itemID];
        }

        else
        {
            endText1.text = "Insufficient Final Bid";
            endText2.text = "Final Bid: " + highbid[_itemID];
            endText3.text = "Reserve Price: " + price[_itemID];
            endText4.text = "Final Bidder: " + buyerNumber[_itemID];
        }
    }
}
