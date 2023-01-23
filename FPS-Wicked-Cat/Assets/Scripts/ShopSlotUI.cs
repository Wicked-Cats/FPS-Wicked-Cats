using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] Image _itemSprite;
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemCost;

    [SerializeField] private Button _addItemToCartButton;
    [SerializeField] private Button _removeItemFromCartButton;

    public ShopKeeperDisplay ParentDisplay { get; private set; }

    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;
        _itemName.text = "";
        _itemCost.text = "";

        _addItemToCartButton?.onClick.AddListener(AddItemToCart);
        _removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);

        ParentDisplay.transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    private void RemoveItemFromCart()
    {
        Debug.Log("Removing item from cart");
    }

    private void AddItemToCart()
    {
        Debug.Log("Adding item to cart");
    }


    public void SetSprite(Image sprite)
    {
        _itemSprite = sprite;
    }

    public void SetItemName(string itemName)
    {
        _itemName.SetText(itemName);
    }

    public void SetItemCost(string itemCost)
    {
        _itemCost.SetText(itemCost);
    }
}
