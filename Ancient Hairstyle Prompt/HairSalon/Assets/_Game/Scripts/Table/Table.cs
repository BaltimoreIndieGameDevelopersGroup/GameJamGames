using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour
{

    public GameObject itemPosition;
    public Item item = null;

    public void Start()
    {
        if (item == null) item = GetComponentInChildren<Item>();
    }

    public bool IsEmpty()
    {
        return (item == null);
    }

    public Item TakeItem()
    {
        var itemToReturn = item;
        item = null;
        return itemToReturn;
    }

    public void LeaveItem(Item itemToLeave)
    {
        if (IsEmpty() && (itemToLeave != null))
        {
            itemToLeave.transform.SetParent(itemPosition.transform);
            itemToLeave.transform.localPosition = Vector3.zero;
            item = itemToLeave;
        }
    }

}
