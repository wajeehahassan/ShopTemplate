using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public GameObject shopPopUp;
    public GameObject[] items;

    public List<int> box1ItemsList;
    public List<int> box2ItemsList;

    public Text priceToUnlockItemsText;
    public int priceToUnlockItems;


    public List<int> uniqueNumbersList;
    public List<int> randomUniqueNumbersList;
    int ranNumToUnlockItem;
    public GameObject UnlockBtn;

    // Start is called before the first frame update
    private void Awake()
    {
        AssignKeyToPlayerPref();
        box1ItemsList = new List<int>();
        box2ItemsList = new List<int>();
    }

    void Start()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CloseBtnPressed()
    {
        shopPopUp.transform.GetChild(0).transform.gameObject.GetComponents<TweenScale>()[1].ResetToBeginning();
        shopPopUp.transform.GetChild(0).transform.gameObject.GetComponents<TweenScale>()[1].PlayForward();
    }
    public void ShopBtnPressed()
    {
        ShopActive(true);
        shopPopUp.transform.GetChild(0).transform.gameObject.GetComponents<TweenScale>()[0].ResetToBeginning();
        shopPopUp.transform.GetChild(0).transform.gameObject.GetComponents<TweenScale>()[0].PlayForward();
    }

    public void ShopActive(bool isActive)
    {
        shopPopUp.SetActive(isActive);
    }

    public void AssignKeyToPlayerPref()
    {
        if (!PlayerPrefs.HasKey("UnlockPrice"))

        {
            priceToUnlockItems = 400;
            PlayerPrefs.SetInt("UnlockPrice", priceToUnlockItems);
        }
        else
        {
            PlayerPrefs.SetInt("UnlockPrice", PlayerPrefs.GetInt("UnlockPrice"));
        }
        Debug.Log("UnlockPrice = " + PlayerPrefs.GetInt("UnlockPrice"));
        priceToUnlockItemsText.text = string.Format("UNLOCK RANDOM {0} ", PlayerPrefs.GetInt("UnlockPrice").ToString());


        for (int i = 0; i < items.Length; i++)
        {
            if (!PlayerPrefs.HasKey(items[i].name))

            {
                PlayerPrefs.SetInt(items[i].name, 0);
            }
            else
            {
                PlayerPrefs.SetInt(items[i].name, PlayerPrefs.GetInt(items[i].name));
            }
            Debug.Log(items[i].name +" = "+ PlayerPrefs.GetInt(items[i].name));
        }
        CheckLockedItems();
    }

    public void CheckLockedItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (PlayerPrefs.GetInt(items[i].name, 0) == 1)
            {
                items[i].transform.GetChild(1).gameObject.SetActive(false);
            }else
            {
                items[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void GenerateRandomList()
    {
        uniqueNumbersList = new List<int>();
        randomUniqueNumbersList = new List<int>();
        int counterofUnlockItems = 0;
        for (int i = 0; i < items.Length; i++)
        {
            uniqueNumbersList.Add(i);
            if (PlayerPrefs.GetInt(items[i].name, 0) == 1)
            {
                uniqueNumbersList.Remove(i);
                counterofUnlockItems++;
            }
        }

        for (int i = 0; i < items.Length - counterofUnlockItems; i++)
        {
            int ranNum = uniqueNumbersList[Random.Range(0, uniqueNumbersList.Count)];
            randomUniqueNumbersList.Add(ranNum);
            uniqueNumbersList.Remove(ranNum);
        }
    }
    public void UnlockRandomItem()
    {
        if (randomUniqueNumbersList.Count != 0)
        {
            ranNumToUnlockItem = randomUniqueNumbersList[Random.Range(0, randomUniqueNumbersList.Count)];
            PlayerPrefs.SetInt(items[ranNumToUnlockItem].name, 1);
            items[ranNumToUnlockItem].transform.GetChild(1).gameObject.SetActive(false);
            items[ranNumToUnlockItem].transform.GetChild(3).gameObject.SetActive(true);
        }
        priceToUnlockItems = PlayerPrefs.GetInt("UnlockPrice");
        priceToUnlockItems = priceToUnlockItems+150;
        PlayerPrefs.SetInt("UnlockPrice", priceToUnlockItems);
        priceToUnlockItemsText.text = string.Format("UNLOCK RANDOM {0} ", PlayerPrefs.GetInt("UnlockPrice").ToString());
        Debug.Log(priceToUnlockItems);
        
    }
    public void UnlockBtnPressed()
    {
        if (PlayerPrefs.GetInt(Constants.PLAYERPREFS_CURRENCY) >= PlayerPrefs.GetInt("UnlockPrice"))
        {
            GenerateRandomList();
            StartCoroutine(InvokeMethodToUnlockRandom(HoverOnRandomItems, 1, randomUniqueNumbersList.Count));
            UnlockBtn.GetComponent<Button>().interactable = false;
        }
    }

    private int hoverItem;
    public IEnumerator InvokeMethodToUnlockRandom(System.Action method, float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            hoverItem = i;
            method();
            yield return new WaitForSeconds(interval/5);
        }
        yield return new WaitForSeconds(0);
        if (PlayerPrefs.GetInt(Constants.PLAYERPREFS_CURRENCY) >= PlayerPrefs.GetInt("UnlockPrice"))
        {
            EconomyManager.Instance.SubtractCurrency(PlayerPrefs.GetInt("UnlockPrice"));
            UnlockRandomItem();
        }
        UnlockBtn.GetComponent<Button>().interactable = true;
    }
    private void HoverOnRandomItems()
    {

        items[randomUniqueNumbersList[hoverItem]].transform.GetChild(2).gameObject.SetActive(true);
        items[randomUniqueNumbersList[hoverItem]].transform.GetChild(2).gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
        items[randomUniqueNumbersList[hoverItem]].transform.GetChild(2).gameObject.GetComponent<TweenAlpha>().PlayForward();

    }
}
