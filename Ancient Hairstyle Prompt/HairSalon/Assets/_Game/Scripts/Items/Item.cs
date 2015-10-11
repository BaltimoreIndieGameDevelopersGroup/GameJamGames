using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public ItemType itemType;

    public void PlaySound()
    {
        var clip = Resources.Load(GetClipName()) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public string GetClipName()
    {
        switch (itemType)
        {
            case ItemType.Clippers: return "Clippers";
            case ItemType.Dryer: return "Dryer";
            case ItemType.Scissors: return "Scissors";
            case ItemType.Shampoo: return "Shampoo";
            default:  return null;
        }
    }

}
