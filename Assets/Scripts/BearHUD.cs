using System.Collections;
using UnityEngine;

public class BearHUD : MonoBehaviour
{
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar hungerBar;
    [SerializeField] private ProgressBar eatingBar;

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
        eatingBar.SetProgress((float) bear.GetComponent<EatHoney>().GetDuration());
    }

    public IEnumerator UpdateHealth() {
        yield return healthBar.SetProgressSmooth((float) bear.GetHealth());
    }
    public IEnumerator UpdateHunger() {
        yield return hungerBar.SetProgressSmooth((float) bear.GetHunger());
    }
    public IEnumerator UpdateTimeEating() {
        yield return eatingBar.SetProgressSmooth((float) bear.GetComponent<EatHoney>().GetDuration());
    }
}
