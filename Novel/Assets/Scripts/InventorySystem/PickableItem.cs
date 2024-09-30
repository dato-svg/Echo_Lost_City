using System;
using UnityEngine;

namespace InventorySystem
{
    public class PickableItem : MonoBehaviour
    {
        public Item item;
        public Inventory inventory;

        public void Start()
        {
            inventory.AddItem(item);
        }


        public void RemoveThisItem()
        {
            inventory.RemoveItem(item);
        }
    }
}