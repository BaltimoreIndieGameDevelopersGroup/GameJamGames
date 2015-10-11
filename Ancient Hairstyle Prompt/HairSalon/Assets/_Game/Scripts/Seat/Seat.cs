using UnityEngine;
using System.Collections;

public class Seat : MonoBehaviour {

    public GameObject customerPosition;
    public GameObject customer;

    public bool IsEmpty()
    {
        return (customer == null);
    }

    public void AddCustomer(GameObject customer)
    {
        this.customer = customer;
        customer.transform.SetParent(customerPosition.transform);
        customer.transform.localPosition = Vector3.zero;
    }

    public void RemoveCustomer()
    {
        customer = null;
    }
}
