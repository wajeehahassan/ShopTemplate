using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemsShopManager : MonoBehaviour
{
    public static ItemsShopManager Instance;
    public List<ItemsData> items;
    public enum ItemsUnlockType
    {

        Currency1,
        Currency2,
        RewardedVideo,

    };

   public Queue<ItemsData>[] Queques = new Queue<ItemsData>[3];

   public Button unlockButton1;
   public Button unlockButton2;
   public Button unlockButton3;

    public GameObject itemsShop2D;
    public GameObject itemsShop3D;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        AssignKeyToThePlayerPrefs();
        CheckLockedItems();
        PopulateQueues();
        PlaceIngredientsToPlaceHolders();
        AddClickListerners();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AssignKeyToThePlayerPrefs()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (!PlayerPrefs.HasKey(items[i].name))

            {
                PlayerPrefs.SetInt(items[i].name, 0);
            }
            else
            {
                PlayerPrefs.SetInt(items[i].name, PlayerPrefs.GetInt(items[i].name));
            }
            Debug.Log(items[i].name + " = " + PlayerPrefs.GetInt(items[i].name));
        }
    }

    public void AddClickListerners()
    {
        unlockButton1.onClick.AddListener(() => { PurchaseUsingCurrency(50, 0, false); });
        unlockButton2.onClick.AddListener(() => { PurchaseUsingCurrency(100, 1, false); });
        unlockButton3.onClick.AddListener(() => { PurchaseUsingCurrency(0, 2, true); });

    }


    public void PopulateQueues()
    {
        for (int i = 0; i < Queques.Length; i++)
        {
            Queques[i] = new Queue<ItemsData>();
            Queques[i].Clear();

        }


        foreach (ItemsData ingredient in items)
        {
            if (ingredient.unlockType == ItemsUnlockType.Currency1 && PlayerPrefs.GetInt(ingredient.name)!=1)
            {
                Queques[0].Enqueue(ingredient);
            }
            if (ingredient.unlockType == ItemsUnlockType.Currency2 && PlayerPrefs.GetInt(ingredient.name)!= 1)
            {
                Queques[1].Enqueue(ingredient);
            }
            if (ingredient.unlockType == ItemsUnlockType.RewardedVideo && PlayerPrefs.GetInt(ingredient.name)!= 1)
            {
                Queques[2].Enqueue(ingredient);
            }
        }
       
    }



    public void CheckLockedItems()
    {
        foreach (ItemsData ingredient in items)
        {

            if (ingredient.isUnlocked)
            {
                PlayerPrefs.SetInt(ingredient.name, 1);
            }


            if (PlayerPrefs.GetInt(ingredient.name, 0) == 1)
            {
                ingredient.icon.transform.GetChild(1).gameObject.SetActive(false);
                ingredient.model.SetActive(false);
            }
            else
            {
                ingredient.icon.transform.GetChild(1).gameObject.SetActive(true);
                ingredient.model.SetActive(true);
            }
        }
    }


    public void PlaceIngredientsToPlaceHolders()
    {

        for (int i = 0; i < Queques.Length; i++)
        {
            if (Queques[i].Count != 0)
            {
                Queques[i].Peek().model.transform.position = Queques[i].Peek().placeHolder.position;
                Queques[i].Peek().purchaseButton.SetActive(true);
            }
        }
    }

    public void UnlockIngredient(int index)
    {
        Debug.Log(Queques[index].Peek().model.name);
        PlayerPrefs.SetInt(Queques[index].Peek().name, 1);
        Queques[index].Peek().isUnlocked = true;
        Queques[index].Peek().icon.transform.GetChild(1).gameObject.SetActive(false);
        Queques[index].Peek().model.SetActive(false);
        Queques[index].Peek().purchaseButton.SetActive(false);
        Queques[index].Dequeue();
        //PopulateQueues();
        //PlaceIngredientsToPlaceHolders();
    }


    public void PurchaseUsingCurrency(int price, int index , bool isRewarded)
    {

        if (!isRewarded)
        {
            if (PlayerPrefs.GetInt(Constants.PLAYERPREFS_CURRENCY) >= price)
            {
                EconomyManager.Instance.SubtractCurrency(price);
                UnlockIngredient(index);
            }
        }
        else
        {
            UnlockIngredient(index);
            Debug.Log("HEre i am ");
        }

    }

    public void ResetShop()
    {
        AssignKeyToThePlayerPrefs();
        CheckLockedItems();
        PopulateQueues();
        PlaceIngredientsToPlaceHolders();
    }

    [System.Serializable]
    public class ItemsData
    {
        public string name;
        public GameObject model;
        public GameObject icon;
        public ItemsUnlockType unlockType;
        public bool isUnlocked;
        public int price;
        public Transform placeHolder;
        public GameObject purchaseButton;
    }

    public void CloseBtnPressed()
    {
        itemsShop2D.SetActive(false);
        itemsShop3D.SetActive(false);
    }
    public void ItemsShopBtnPressed()
    {
        itemsShop2D.SetActive(true);
        itemsShop3D.SetActive(true);
        CheckLockedItems();
        PopulateQueues();
        PlaceIngredientsToPlaceHolders();
    }
}