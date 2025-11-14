using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image bgDim;

    private float countdownTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdownTime = 4.2f;

        bgDim.enabled = true;
        text.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        countdownTime -= Time.deltaTime;

        text.text = ((int)countdownTime).ToString();

        if (countdownTime <= 1)
        {
            bgDim.enabled = false;
            text.enabled = false;
        }
    }
}
