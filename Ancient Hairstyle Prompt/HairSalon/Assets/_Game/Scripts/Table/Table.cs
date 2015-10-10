using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour
{

    public GameObject itemPosition;
    public GameObject item = null;

    public bool IsEmpty()
    {
        return (item == null);
    }

    public GameObject TakeItem()
    {
        var itemToReturn = item;
        item = null;
        //itemToReturn.SetActive(false);
        return itemToReturn;
    }

    public void LeaveItem(GameObject itemToLeave)
    {
        if (IsEmpty() && (itemToLeave != null))
        {
            //itemToLeave.SetActive(true);
            itemToLeave.transform.SetParent(itemPosition.transform);
            itemToLeave.transform.localPosition = Vector3.zero;
            item = itemToLeave;
        }
    }

}
