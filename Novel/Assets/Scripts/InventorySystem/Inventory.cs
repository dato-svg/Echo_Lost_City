using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public List<Item> items = new List<Item>();

        public void AddItem(Item newItem)
        {
            items.Add(newItem);
            Debug.Log("Добавлен новый предмет :" + newItem.name);
        }


        public void RemoveItem(Item itemRemove)
        {
            if (items.Contains(itemRemove))
            {
                items.Remove(itemRemove);
                Debug.Log("Удален предмет :" + itemRemove);
            }
        }
    }
}
