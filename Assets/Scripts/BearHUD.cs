using System.Collections;
using UnityEngine;

public class BearHUD : MonoBehaviour
{
    [SerializeField] ProgressBar healthBar;
    [SerializeField] ProgressBar hungerBar;
    [SerializeField] ProgressBar eatingBar;

    Bear bear;
    private string bearTag = "Bear";

    private void Start()
    {
        bear = GameObject.FindGameObjectWithTag(bearTag).GetComponent<Bear>();
    }

    public void SetData(Bear bear) {
        this.bear = bear;
        healthBar.SetProgress((float) bear.GetHealth());
        hungerBar.SetProgress((float) bear.GetHunger());
    }

    public IEnumerator UpdateHealth() {
        yield return healthBar.SetProgressSmooth((float) bear.GetHealth());
    }
    public IEnumerator UpdateHunger() {
        yield return hungerBar.SetProgressSmooth((float) bear.GetHunger());
    }
    public IEnumerator UpdateTimeEating(float timeEating) {
        yield return hungerBar.SetProgressSmooth((float) timeEating);
    }
}
