using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour {

    public GameObject unhappyIndicator;
    public GameObject recipeCanvas;
    public GameObject scissorsIndicatorTemplate;
    public GameObject clipperIndicatorTemplate;
    public GameObject shampooIndicatorTemplate;
    public GameObject dryerIndicatorTemplate;

    public List<ItemType> recipe;

    public void Start()
    {
        unhappyIndicator.SetActive(false);
        HideItemIndicatorTemplates();
        SetRecipe(recipe.ToArray());
    }

    public void SetRecipe(ItemType[] recipe)
    {
        this.recipe = new List<ItemType>(recipe);
        foreach (var itemType in recipe)
        {
            var template = GetItemIndicatorTemplate(itemType);
            var indicator = Instantiate(template);
            indicator.transform.SetParent(recipeCanvas.transform, false);
            indicator.SetActive(true);
            
        }
    }

    private void HideItemIndicatorTemplates()
    {
        scissorsIndicatorTemplate.SetActive(false);
        clipperIndicatorTemplate.SetActive(false);
        shampooIndicatorTemplate.SetActive(false);
        dryerIndicatorTemplate.SetActive(false);
    }

    private GameObject GetItemIndicatorTemplate(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Scissors: return scissorsIndicatorTemplate;
            case ItemType.Clipper: return clipperIndicatorTemplate;
            case ItemType.Shampoo: return shampooIndicatorTemplate;
            case ItemType.Dryer: return dryerIndicatorTemplate;
            default: return null;
        }
    }

}
