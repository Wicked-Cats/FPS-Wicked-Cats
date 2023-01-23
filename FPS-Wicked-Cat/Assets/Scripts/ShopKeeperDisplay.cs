using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI _shopSlotPrefab;

    [Header("Shopping Cart")]
    [SerializeField] private TextMeshProUGUI _playerComponentText;
    [SerializeField] private TextMeshProUGUI _itemCostText;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyButtonText;

    [Header("Item Preview Section")]
    [SerializeField] private Image _itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI _itemPreviewName;
    [SerializeField] private TextMeshProUGUI _itemPreviewDescription;

    [SerializeField] private GameObject _itemListContentPanel;

    [Header("Upgrades")]
    [SerializeField] UIObjects[] upgrades;

    private void Start()
    {
        ShopSlotUI shopSlotItem = Instantiate(_shopSlotPrefab);
        List<ShopSlotUI> shopSlotItems = new List<ShopSlotUI>();
        
        for (int i = 0; i < upgrades.Length; i++)
        {
            shopSlotItem.SetItemCost(upgrades[i].itemName);
            shopSlotItem.SetItemName(upgrades[i].itemDescription);
            shopSlotItem.SetSprite(upgrades[i].itemSprite);

            shopSlotItems.Add(shopSlotItem);
        }

        for (int i = 0; i < shopSlotItems.Count; i++)
        {
            shopSlotItems[i].transform.SetParent(_itemListContentPanel.transform, false);
        }
    }

    private void RefreshDisplay()
    {
        ClearSlots();

        _buyButton.gameObject.SetActive(false);
        _playerComponentText.text = $"Player Components: {gameManager.instance.componentsCurrent}";
    }

    private void ClearSlots()
    {
        //foreach(var item in _itemListContentPanel.transform.cast<Transform>())
        //{
        //    Destroy(item.gameObject);
        //}
    }
}