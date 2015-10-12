using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour {

    public GameObject unhappyIndicator;
    public GameObject recipeCanvas;
    public GameObject scissorsIndicatorTemplate;
    public GameObject clippersIndicatorTemplate;
    public GameObject shampooIndicatorTemplate;
    public GameObject dryerIndicatorTemplate;

    public List<ItemType> recipe;

    private bool isBeingServed = false;
    private List<GameObject> instantiatedIndicators = new List<GameObject>();

    public void Start()
    {
        unhappyIndicator.SetActive(false);
        HideItemIndicatorTemplates();
        SetRecipe(recipe);
    }

    public void SetRecipe(List<ItemType> recipe)
    {
        instantiatedIndicators.ForEach(indicator => Destroy(indicator));
        instantiatedIndicators.Clear();
        this.recipe = recipe;
        foreach (var itemType in recipe)
        {
            var template = GetItemIndicatorTemplate(itemType);
            var indicator = Instantiate(template);
            indicator.transform.SetParent(recipeCanvas.transform, false);
            indicator.SetActive(true);
            instantiatedIndicators.Add(indicator);
            
        }
    }

    private void HideItemIndicatorTemplates()
    {
        scissorsIndicatorTemplate.SetActive(false);
        clippersIndicatorTemplate.SetActive(false);
        shampooIndicatorTemplate.SetActive(false);
        dryerIndicatorTemplate.SetActive(false);
    }

    private GameObject GetItemIndicatorTemplate(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Scissors: return scissorsIndicatorTemplate;
            case ItemType.Clippers: return clippersIndicatorTemplate;
            case ItemType.Shampoo: return shampooIndicatorTemplate;
            case ItemType.Dryer: return dryerIndicatorTemplate;
            default: return null;
        }
    }

    public bool IsWaitingForService(ItemType itemType)
    {
        return (recipe.Count > 0) && (recipe[0] == itemType) && !isBeingServed;
    }

    public void Serve()
    {
        recipe.RemoveAt(0);
        SetRecipe(recipe);
        if (recipe.Count == 0)
        {
            RecycleCustomer();
        }
    }

    public void RecycleCustomer()
    {
        var seats = new List<Seat>(FindObjectsOfType<Seat>());
        var oldSeat = seats.Find(seat => seat.customer == this);
        foreach (var seat in FindObjectsOfType<Seat>())
        {
            if (seat.IsEmpty())
            {
                seat.AddCustomer(this);
                SetRecipe(GenerateRecipe());
                break;
            }
        }
        if (oldSeat != null) oldSeat.RemoveCustomer();
    }

    public List<ItemType> GenerateRecipe()
    {
        var recipe = new List<ItemType>();
        var size = Random.Range(1, 6);
        for (int i = 0; i < size; i++)
        {
            recipe.Add((ItemType)Random.Range(0, 4));
        }
        return recipe;
    }

}
