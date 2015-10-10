using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string actionButton = "Action1";
    public float speed = 5;
    public GameObject itemPosition;

    private CharacterController characterController;
    private GameObject item = null;
    private Table currentTable = null;

    void Start () {
        characterController = GetComponent<CharacterController>();
	}
	
	void Update () {
        var motion = new Vector3(Input.GetAxis(horizontalAxis) * Time.deltaTime * speed, 0, Input.GetAxis(verticalAxis) * Time.deltaTime * speed);
        characterController.Move(motion);
        if (Input.GetButtonDown(actionButton))
        {
            if (currentTable != null)
            {
                UseTable();
            }
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        currentTable = other.GetComponent<Table>();
        if (currentTable != null) Debug.Log("Entered table", currentTable);
    }

    public void OnTriggerExit(Collider other)
    {
        if (currentTable != null) Debug.Log("Left table", currentTable);
        currentTable = null;
    }

    private void UseTable()
    {
        Debug.Log("Use table", currentTable);
        if (currentTable.IsEmpty())
        {
            Debug.Log("Table is empty");
            if (item != null)
            {
                Debug.Log("Leaving item");
                currentTable.LeaveItem(item);
                item = null;
            }
        }
        else
        {
            Debug.Log("Table has something");
            if (item == null)
            {
                Debug.Log("Taking item");
                item = currentTable.TakeItem();
                item.transform.SetParent(itemPosition.transform);
                item.transform.localPosition = Vector3.zero;
            }
        }
    }

}
