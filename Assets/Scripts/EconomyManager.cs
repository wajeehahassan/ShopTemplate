using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
	public static EconomyManager Instance;

	public Text CurrencyText ;

	void Awake ()
	{
		Instance = this;
        AssignKeyToPlayerPref();
    }

	void Start ()
	{
		
        CurrentCurrency();
	}

	// Update is called once per frame
	void Update ()
	{

	}


	public void AssignKeyToPlayerPref ()
	{

		if (!PlayerPrefs.HasKey (Constants.PLAYERPREFS_CURRENCY)) {
			PlayerPrefs.SetInt (Constants.PLAYERPREFS_CURRENCY, 0);
		} else {
			PlayerPrefs.SetInt (Constants.PLAYERPREFS_CURRENCY, PlayerPrefs.GetInt (Constants.PLAYERPREFS_CURRENCY));
		}
		Debug.Log ("Currency = " + PlayerPrefs.GetInt (Constants.PLAYERPREFS_CURRENCY));
	}

	public void AddCurrency (int amount)
	{
		PlayerPrefs.SetInt (Constants.PLAYERPREFS_CURRENCY, PlayerPrefs.GetInt (Constants.PLAYERPREFS_CURRENCY) + amount);
        CurrentCurrency();
	}

	public void SubtractCurrency (int amount)
	{
		PlayerPrefs.SetInt (Constants.PLAYERPREFS_CURRENCY, PlayerPrefs.GetInt (Constants.PLAYERPREFS_CURRENCY) - amount);
        CurrentCurrency();
	}

	public void CurrentCurrency ()
	{
        CurrencyText.text = PlayerPrefs.GetInt (Constants.PLAYERPREFS_CURRENCY).ToString ();
	}

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        AssignKeyToPlayerPref();
        ShopManager.Instance.AssignKeyToPlayerPref();
        ItemsShopManager.Instance.ResetShop();
        CurrentCurrency();
    }

}
