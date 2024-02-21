using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using ElephantSDK;
[System.Serializable]
public class UpgradeClass
{
    public string upgradeName;
    public Image upgradeImage;
    public List<Sprite> sprites = new List<Sprite>();
    public TextMeshProUGUI upgradeLevelText;
    public TextMeshProUGUI upgradePriceText;
    public List<int> upgradePrices = new List<int>();
}
[System.Serializable]
public class UpgradePrices
{
    public List<int> ratePrices = new List<int>();
    public List<int> rangePrices = new List<int>();
}

public class UpgradeManager : MonoBehaviour
{
    public TextAsset defaultUpgradePrices;
    [SerializeField] private List<UpgradeClass> upgrades = new List<UpgradeClass>();
    private void Awake()
    {
        UpgradePrices up = new UpgradePrices();
        up = JsonUtility.FromJson<UpgradePrices>(RemoteConfig.GetInstance().Get("UpgradePrices", defaultUpgradePrices.text));
        for (int i = 0; i < upgrades.Count; i++)
        {
            upgrades[i].upgradePrices.Clear();
        }
        for (int i = 0; i < upgrades.Count; i++)
        {
            if (i == 1)
            {
                for (int u = 0; u < up.ratePrices.Count; u++)
                {
                    upgrades[i].upgradePrices.Add(up.ratePrices[u]);
                }
            }
            else if (i == 0)
            {
                for (int u = 0; u < up.rangePrices.Count; u++)
                {
                    upgrades[i].upgradePrices.Add(up.rangePrices[u]);
                }
            }
        }
        RefreshUI();
    }
    public void RefreshUI()
    {
        for (int i = 0; i < upgrades.Count; i++)
        {
            RefreshUpgrade(i);
        }
    }
    public void RefreshUpgrade(int upgradeNumber)
    {
        UpgradeClass uc = upgrades[upgradeNumber];
        int price = 0;
        if (PlayerPrefs.GetInt(uc.upgradeName + "Level") < uc.upgradePrices.Count)
        {
            price = uc.upgradePrices[PlayerPrefs.GetInt(uc.upgradeName + "Level")];
        }
        else
        {
            price = uc.upgradePrices[uc.upgradePrices.Count - 1];
        }
        if (PlayerPrefs.GetInt("Coin") >= price)
        {
            uc.upgradeImage.sprite = uc.sprites[0];
        }
        else
        {
            uc.upgradeImage.sprite = uc.sprites[1];
        }
        uc.upgradeLevelText.text = "lvl " + (PlayerPrefs.GetInt(uc.upgradeName + "Level") + 1).ToString();
        uc.upgradePriceText.text = price.ToString() + "$";
    }
    public void PurchaseUpgrade(int upgradeNum)
    {
        UpgradeClass uc = upgrades[upgradeNum];
        int price = 0;
        if (PlayerPrefs.GetInt(uc.upgradeName + "Level") < uc.upgradePrices.Count)
        {
            price = uc.upgradePrices[PlayerPrefs.GetInt(uc.upgradeName + "Level")];
        }
        else
        {
            price = uc.upgradePrices[uc.upgradePrices.Count - 1];
        }
        switch (upgradeNum)
        {
            case 0:
                break;
            case 1:
                break;
        }
        if (PlayerPrefs.GetInt("Coin") >= price)
        {
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - price);
            PlayerPrefs.SetInt(uc.upgradeName + "Level", PlayerPrefs.GetInt(uc.upgradeName + "Level") + 1);
            GameManager.instance.RefreshCoinText();
            StartCoroutine(ShakeMainBar(uc.upgradeImage.gameObject));
            RefreshUI();
        }
    }
    private IEnumerator ShakeMainBar(GameObject mainBar)
    {
        yield return new WaitForSeconds(0);
        Vector3 startScale = Vector3.one;
        Vector3 startEuler = Vector3.zero;
        mainBar.transform.DOScale(startScale * 1.1f, .15f);
        mainBar.transform.DOLocalRotate(startEuler + new Vector3(0, 0, -5), .1f);
        yield return new WaitForSeconds(.1f);
        mainBar.transform.DOLocalRotate(startEuler + new Vector3(0, 0, 5), .05f);
        yield return new WaitForSeconds(.05f);
        mainBar.transform.DOLocalRotate(startEuler + new Vector3(0, 0, 0), .05f);
        yield return new WaitForSeconds(.05f);
        mainBar.transform.DOScale(startScale, .1f);
    }
}
