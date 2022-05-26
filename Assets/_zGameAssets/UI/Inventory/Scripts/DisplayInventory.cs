using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DisplayInventory : MonoBehaviour
{
    //overworld
    PlayerInventory pi;

    public InventoryObject inventory;
    GameObject child;

    [Header("Display Dimensions")]
    [SerializeField] int xStart;
    [SerializeField] int yStart;
    [SerializeField] int numberOfColumns;
    [SerializeField] int xSpaceBetwItems;
    [SerializeField] int ySpaceBetwItems;

    [Header("Inventory Specs")]
    [SerializeField] GameObject displayPrefab;
    [SerializeField] int itemsPerPage = 9;
    int totalPages;
    int pageNumber = 1;

    [Header("Inventory Page Navigation")]
    [SerializeField] TextMeshProUGUI pageNumID;
    [SerializeField] Button firstPage;
    [SerializeField] Button prevPage;
    [SerializeField] Button nextPage;
    [SerializeField] Button lastPage;

    [Header("SelectedItem")]
    [SerializeField] GameObject selected;
    Image sIcon;
    TextMeshProUGUI sQuantity;
    TextMeshProUGUI sName;
    TextMeshProUGUI sDescription;
    [SerializeField] GameObject sFirstSelected;

    //backgroundList
    GameObject[] bgList;
    Image[] bgIcon;
    RectTransform[] bgRectTransform;
    TextMeshProUGUI[] bgQuantity;
    TextMeshProUGUI[] bgName;
    TextMeshProUGUI[] bgDescription;
    Button[] bgButton;

    void Start()
    {
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

        #region Selected Icon Setting
        sIcon = selected.GetComponentsInChildren<Image>()[1];
        sQuantity = selected.GetComponentInChildren<TextMeshProUGUI>();
        sName = selected.GetComponentsInChildren<TextMeshProUGUI>()[1];
        sDescription = selected.GetComponentsInChildren<TextMeshProUGUI>()[2];
        #endregion

        child = transform.GetChild(0).gameObject;
        ConstructBgList();
        child.SetActive(false);
    }

    void Update()
    {
        if (child.activeInHierarchy && !selected.activeInHierarchy)
        {
            UpdateCounters();

            if (Input.GetAxis("L2")+1 > 1f) PageNavi(1);
            else if (Input.GetButtonDown("L1")) PageNavi(2);
            else if (Input.GetButtonDown("R1")) PageNavi(3);
            else if (Input.GetAxis("R2") + 1 > 1f) PageNavi(4);
        }
    }

    public void SetOpenInventory()
    {
        OpenInventory();
    }

    public bool OpenInventory()
    {
        child.SetActive(!child.activeInHierarchy);
        if (child.activeInHierarchy)
        {
            LoadList();
        }
        else
        {
            ClearList();
        }
        return child.activeInHierarchy;
    }

    public bool GetOpenInventory() { return child.activeInHierarchy; }

    public bool GetSelectedOpen() { return selected.activeInHierarchy; }

    void PageNavi(int button)
    {
        switch (button)
        {
            case 1:
                pageNumber = 1;
                break;
            case 2:
                pageNumber--;
                break;
            case 3:
                pageNumber++;
                break;
            case 4:
                pageNumber = totalPages;
                break;
        }
        Debug.Log(pageNumber);
        LoadList();
    }

    void ButtonPressed(int index)
    {
        Debug.Log(index);
        ClearList();
        selected.SetActive(true);
        sIcon.sprite = inventory.itemSlots[index].itemObject.icon;
        sQuantity.text = ("x" + inventory.itemSlots[index].stackQuantity.ToString("n0"));
        sName.text = inventory.itemSlots[index].itemObject.name;
        sDescription.text = inventory.itemSlots[index].itemObject.itemDescription;

        pi.RecieveItemIndex(index);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(sFirstSelected);
    }

    void UpdateCounters()
    {
        int j = 0;
        int endPoint = (pageNumber * itemsPerPage);
        for (int i = endPoint - itemsPerPage; i < Mathf.Min(endPoint, inventory.itemSlots.Count); i++)
        {
            if (inventory.itemSlots.Contains(inventory.itemSlots[i]))
            {
                bgQuantity[j].text = ("x" + inventory.itemSlots[i].stackQuantity.ToString("n0"));
            }
            else if (!inventory.itemSlots.Contains(inventory.itemSlots[i]))
            {
                LoadList();
            }
            j++;
        }
    }

    void ConstructBgList()
    {
        bgList = new GameObject[itemsPerPage];
        bgIcon = new Image[itemsPerPage];
        bgRectTransform = new RectTransform[itemsPerPage];
        bgQuantity = new TextMeshProUGUI[itemsPerPage];
        bgName = new TextMeshProUGUI[itemsPerPage];
        bgDescription = new TextMeshProUGUI[itemsPerPage];
        bgButton = new Button[itemsPerPage];

        for (int i = 0; i < itemsPerPage; i++)
        {
            bgList[i] = Instantiate(displayPrefab, Vector3.zero, Quaternion.identity, child.transform);
            bgIcon[i] = bgList[i].GetComponentsInChildren<Image>()[1];
            bgRectTransform[i] = bgList[i].GetComponent<RectTransform>();
            bgQuantity[i] = bgList[i].GetComponentInChildren<TextMeshProUGUI>();
            bgName[i] = bgList[i].GetComponentsInChildren<TextMeshProUGUI>()[1];
            bgDescription[i] = bgList[i].GetComponentsInChildren<TextMeshProUGUI>()[2];
            bgButton[i] = bgList[i].GetComponent<Button>();
        }

        LoadList();
    }

    public void LoadList()
    {
        ClearList();

        #region Update Page Number
        totalPages = (int)Mathf.Ceil((inventory.itemSlots.Count - 1) / itemsPerPage) + 1;

        if (pageNumber > totalPages)
        {
            pageNumber = totalPages;
        }
        else if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageNumber <= 1)
        {
            firstPage.gameObject.SetActive(false);
            prevPage.gameObject.SetActive(false);
        }
        else
        {
            firstPage.gameObject.SetActive(true);
            prevPage.gameObject.SetActive(true);
        }

        if (pageNumber >= totalPages)
        {
            nextPage.gameObject.SetActive(false);
            lastPage.gameObject.SetActive(false);
        }
        else
        {
            nextPage.gameObject.SetActive(true);
            lastPage.gameObject.SetActive(true);
        }

        pageNumID.SetText(pageNumber + "/" + totalPages);
        #endregion

        int j = 0;
        int endPoint = (pageNumber * itemsPerPage);
        for (int i = endPoint - itemsPerPage; i < Mathf.Min(endPoint, inventory.itemSlots.Count); i++)
        {
            bgList[j].SetActive(true);
            bgIcon[j].sprite = inventory.itemSlots[i].itemObject.icon;
            bgRectTransform[j].localPosition = GetPos(j);
            bgQuantity[j].text = ("x" + inventory.itemSlots[i].stackQuantity.ToString("n0"));
            bgName[j].text = inventory.itemSlots[i].itemObject.name;
            bgDescription[j].text = inventory.itemSlots[i].itemObject.itemDescription;
            bgButton[j].AddOnClickListener(i, ButtonPressed);
            j++;
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(bgList[0]);
    }

    void ClearList()
    {
        selected.SetActive(false);
        for (int i = 0; i < itemsPerPage; i++)
        {
            bgList[i].SetActive(false);
        }
    }

    Vector3 GetPos(int i)
    {
        return new Vector3(xStart + (xSpaceBetwItems * (i % numberOfColumns)), yStart + (-ySpaceBetwItems * (i / numberOfColumns)), 0f);
    }
}

public static class ButtonExtension
{
    public static void AddOnClickListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate () { OnClick(param); });
    }
}
