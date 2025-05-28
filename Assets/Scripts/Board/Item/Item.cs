using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "BoardGame/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int Id;
    public int price;
    public Sprite icon;
    [TextArea] public string description;
}
