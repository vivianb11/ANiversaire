using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Countdown : MonoBehaviour
{
    // variable to store the due date
    private DateTime due = new DateTime(2024, 4, 25, 20, 59, 59);
    // variable to display the countdown
    private TextMeshProUGUI countdownText;

    private void Awake()
    {
        countdownText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        // calculate the time left
        TimeSpan timeLeft = due - DateTime.Now;
        // add a return to the line to make it more readable
        countdownText.text = string.Format("Part 3/4 out in <br> {0:dd\\:hh\\:mm\\:ss}", timeLeft);
    }
}
