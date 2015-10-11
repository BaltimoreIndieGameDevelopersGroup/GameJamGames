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
        SetRecipe(recipe.ToArray());
    }

    public void SetRecipe(ItemType[] recipe)
    {
        instantiatedIndicators.ForEach(indicator => Destroy(indicator));
        instantiatedIndicators.Clear();
        this.recipe = new List<ItemType>(recipe);
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
        SetRecipe(recipe.ToArray());
        if (recipe.Count == 0)
        {
            Destroy(this.gameObject);
            SpawnNewCustomer();
        }
    }

    public void SpawnNewCustomer()
    {
        foreach (var seat in FindObjectsOfType<Seat>())
        {
            if (seat.IsEmpty())
            {
                var newCustomer = Instantiate(this.gameObject) as GameObject;
                seat.AddCustomer(newCustomer);
                newCustomer.GetComponent<Customer>().SetRecipe(GenerateRecipe());
                break;
            }
        }
    }

    public ItemType[] GenerateRecipe()
    {
        List<ItemType> recipe = new List<ItemType>();
        var size = Random.Range(1, 6);
        for (int i = 0; i < size; i++)
        {
            recipe.Add((ItemType)Random.Range(0, 4));
        }
        return recipe.ToArray();
    }

}
