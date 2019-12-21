using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonScript : MonoBehaviour
{
    public SwitchScene toPlaySceneSwitcher;

    public void OnPlayClick() {
        FindObjectOfType<SettingsProvider>().updateSettingValues();
        toPlaySceneSwitcher.ChangeScene();
    }
}
