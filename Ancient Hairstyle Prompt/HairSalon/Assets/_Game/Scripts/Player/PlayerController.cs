using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";
    public string actionButton = "Action1";
    public float speed = 5;
    public GameObject itemPosition;
    public UnityEngine.UI.Text scoreText;

    private int score = 0;
    private CharacterController characterController;
    private Item item = null;
    private Table currentTable = null;
    private Customer currentCustomer = null;

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
            else if (currentCustomer != null)
            {
                UseCustomer();
            }
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        currentTable = other.GetComponent<Table>();
        currentCustomer = other.GetComponent<Customer>();
    }

    public void OnTriggerExit(Collider other)
    {
        currentTable = null;
        currentCustomer = null;
    }

    private void UseTable()
    {
        if (currentTable.IsEmpty())
        {
            if (item != null)
            {
                currentTable.LeaveItem(item);
                item = null;
            }
        }
        else
        {
            if (item == null)
            {
                item = currentTable.TakeItem();
                item.transform.SetParent(itemPosition.transform);
                item.transform.localPosition = Vector3.zero;
                item.PlayPickupSound();
            }
        }
    }

    private void UseCustomer()
    {
        if ((item != null) && currentCustomer.IsWaitingForService(item.itemType))
        {
            item.PlayUseSound();
            ReturnItem();
            currentCustomer.Serve();
            AddScore();
        }
    }

    private void ReturnItem()
    {
        foreach (var table in FindObjectsOfType<Table>())
        {
            if (table.IsEmpty())
            {
                table.LeaveItem(item);
                break;
            }
        }
        item = null;
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }

}
