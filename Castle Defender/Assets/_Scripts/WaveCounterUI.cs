using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounterUI : MonoBehaviour {

    private Text waveCounterText;

	// Use this for initialization
	void Awake () {
        waveCounterText = GetComponent<Text>();
	}

    public void setWaveCounter(int waveNum, float waveHP)
    {
        waveCounterText.text = string.Format("Wave: {0, 2}\n({1})", waveNum, (int) waveHP);
    }
}
