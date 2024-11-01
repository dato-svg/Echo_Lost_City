using System.Collections.Generic;
using UnityEngine;

public class SaveSlotsManager : MonoBehaviour
{
    public List<Slots> slots = new List<Slots>();

    private void Start()
    {
        CurenntSlotActivate(0);
    }

    public void CurenntSlotActivate(int index)
    {
        foreach (var slot in slots)
        {
            slot.transform.localScale = Vector3.zero;
        }
        slots[index].transform.localScale = Vector3.one;
    }
}
