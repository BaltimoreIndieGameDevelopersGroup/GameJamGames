using UnityEngine;
using System.Collections;

public class Seat : MonoBehaviour {

    public GameObject customerPosition;
    public Customer customer;

    public void Start()
    {
        if (customer == null) customer = GetComponentInChildren<Customer>();
    }

    public bool IsEmpty()
    {
        return (customer == null);
    }

    public void AddCustomer(Customer customer)
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
