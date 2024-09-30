using UnityEngine;


namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
    public class Item : ScriptableObject
    {
        public string nameItem;
        public Sprite iconItem;
        [TextArea(2,10)] public string description;
    }
}
