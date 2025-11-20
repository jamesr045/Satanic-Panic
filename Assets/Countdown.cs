using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image bgDim;

    private float _countdownTime;
    private bool _startCountdown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartCountdown()
    {
        _startCountdown = true;
        
        _countdownTime = 4.2f;

        bgDim.enabled = true;
        text.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_startCountdown)
        {
            _countdownTime -= Time.deltaTime;

            text.text = ((int)_countdownTime).ToString();

            if (_countdownTime <= 1)
            {
                bgDim.enabled = false;
                text.enabled = false;
            }
        }
    }
}
