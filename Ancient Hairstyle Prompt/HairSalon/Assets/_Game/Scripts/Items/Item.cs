using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public ItemType itemType;

    public void PlaySound(string soundFileName)
    {
        var clip = Resources.Load(soundFileName) as AudioClip;
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void PlayPickupSound()
    {
        PlaySound("itemSwipe");
    }

    public void PlayUseSound()
    {
        PlaySound(GetClipName());
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
