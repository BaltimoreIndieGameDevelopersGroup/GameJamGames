using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

    public float duration = 60;
    public UnityEngine.UI.Slider slider;
    public GameObject endOfGameText;

	public IEnumerator Start() {
        endOfGameText.SetActive(false);
        float elapsed = 0;
        while (elapsed < duration)
        {
            slider.value = elapsed / duration;
            yield return null;
            elapsed += Time.deltaTime;
        }
        endOfGameText.SetActive(true);
        Time.timeScale = 0;
	}
	
}
