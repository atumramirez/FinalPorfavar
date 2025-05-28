using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerStatsServerUI PlayerStatsServerUI;

    [SerializeField] private int coins;
    public int Coins => coins;
    [SerializeField] private int stars;
    public int Stars => stars;
    public int coinsBeforeChange;

    public List<Item> inventory = new List<Item>();
    [SerializeField] private int maxItems = 3;



    [HideInInspector] public UnityEvent OnInitialize;
    [HideInInspector] public UnityEvent<int> OnAnimation;

    private void Start()
    {
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
    }

    public void AddStars(int amount)
    {
        stars += amount;
        stars = Mathf.Clamp(stars, 0, 999);
    }

    public void TakeDamage(int amount)
    {
        stars -= amount;
        stars = Mathf.Clamp(stars, 0, 999);
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

    //Shop stuff
    public bool HasInventorySpace()
    {
        return inventory.Count < maxItems;
    }

    public bool TryBuyItem(Item item)
    {
        if (coins >= item.price)
        {
            coins -= item.price;
            inventory.Add(item);
            return true;
        }
        return false;
    }

    public void GetItem(Item item)
    {
        inventory.Add(item);
    }
}
