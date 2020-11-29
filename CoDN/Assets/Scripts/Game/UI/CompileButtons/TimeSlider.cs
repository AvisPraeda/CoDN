using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;
    private Slider slider;

    private void Start()
    {
        slider = this.GetComponent<Slider>();
    }

    public void SetTime()
    {
        gameHandler.SetTimeScale(slider.value);
    }
}
