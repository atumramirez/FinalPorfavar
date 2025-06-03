using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerStatsServerUI PlayerStatsServerUI;
    [SerializeField] private DiscardItemLogic discardLogic;
    [SerializeField] private PlayerController playerController;

    [Header("Coins")]
    [SerializeField] private int coins;
    public int Coins => coins;

    [Header("Health")]
    [SerializeField] private int health;
    public int Health => health;

    [HideInInspector] public int coinsBeforeChange; 

    [Header("Inventory")]
    public List<Item> inventory = new();
    [SerializeField] private int maxItems = 3;

    [Header("Item")]
    [SerializeField] private ItemDatabase itemDatabase;

    [HideInInspector] public UnityEvent OnInitialize;
    [HideInInspector] public UnityEvent<int> OnAnimation;

    private void Start()
    {
        PlayerStatsServerUI.UpdateHealthAndPointPoints(Health, Coins);
        OnInitialize.Invoke();
        coinsBeforeChange = coins;
    }

    public void AddCoins(int amount)
    {
        coinsBeforeChange = coins;
        coins += amount;
        coins = Mathf.Clamp(coins, 0, 999);
        PlayerStatsServerUI.UpdatePointPoints(coins);
    }

    public void RemoveCoins(int amount)
    {
        coinsBeforeChange = coins;
        coins -= amount;
        coins = Mathf.Clamp(coins, 0, 999);
        PlayerStatsServerUI.UpdatePointPoints(coins);
    }

    public void AddHealth(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, 999);
        PlayerStatsServerUI.UpdateHealthPoints(health);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, 999);
        PlayerStatsServerUI.UpdateHealthPoints(health);
    }

    public void CoinAnimation(int value)
    {
        coinsBeforeChange += value;
        OnAnimation.Invoke(coinsBeforeChange);
    }

    public void UpdateStats()
    {
        OnInitialize.Invoke();
        coinsBeforeChange = coins;
    }

    //ITENS//
    public bool HasInventorySpace()
    {
        return inventory.Count < maxItems;
    }

    public void BuyItem(int Id)
    {
        int price = itemDatabase.GetItemPrice(Id);
        GetItem(itemDatabase.GetItemById(Id));
        RemoveCoins(price);
    }

    public void RemoveItem(int index)
    {
        Debug.Log("Item Removido");
        inventory.RemoveAt(index);
        playerController.ContinueMovement();
    }

    public void GetItem(Item item)
    {
        inventory.Add(item);
        if (!HasInventorySpace())
        {
            Debug.Log("Sem espaço");
            DiscardItem();
        }
    }

    public void DiscardItem()
    {
        Debug.Log("Tou todo maluco");
        playerController.StopMovement();    
        int[] ids = new int[inventory.Count];
        for (int i = 0; i < inventory.Count; i++)
        {
            ids[i] = itemDatabase.GetId(inventory[i]);
        }
        discardLogic.OpenMenu(ids);
    }
}
