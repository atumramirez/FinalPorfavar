using UnityEngine;

public class ClientController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerMenu;
    [SerializeField] private GameObject RollMenu;
    [SerializeField] private GameObject ItemMenu;
    [SerializeField] private GameObject MapMenu;

    [Header("Shop")]
    [SerializeField] private GameObject ShopMenu;


    public void PressRoll()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(true);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(false);
    }

    public void PressItem()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(true);
        MapMenu.SetActive(false);
    }

    public void PressMap()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(true);
    }

    public void PressBack()
    {
        PlayerMenu.SetActive(true);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        PlayerMenu.SetActive(false);
        RollMenu.SetActive(false);
        ItemMenu.SetActive(false);
        MapMenu.SetActive(false);
        ShopMenu.SetActive(true);
    }
}
