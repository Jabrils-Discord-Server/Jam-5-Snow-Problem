using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject attributions;

    private int currentLevelNum = 0;
    private GameObject currentLevel;
    private GameObject snowballPrefab;
    private GameObject snowball;

    void Start()
    {
        snowballPrefab = Resources.Load<GameObject>("Prefabs/Snowball");
        foreach(Transform level in transform) {
            if(level.gameObject.activeSelf) {
                currentLevel = level.gameObject;
                currentLevelNum = level.GetSiblingIndex() + 1;
                break;
            }
        }
        if(currentLevel) LoadSnowball();
        else ToggleAttributions(false);
    }

    void Update() {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void OnPlayButton() {
        mainMenu.SetActive(false);
        NextLevel();
    }

    public void ToggleAttributions(bool on) {
        mainMenu.SetActive(!on);
        attributions.SetActive(on);
    }

    public void NextLevel() {
        currentLevelNum += 1;
        LoadLevel();
        LoadSnowball();
    }

    public void Reset() {
        LoadSnowball();
    }

    private void LoadSnowball() {
        if(snowball != null) Destroy(snowball);

        snowball = Instantiate(snowballPrefab);
        snowball.transform.localPosition = currentLevel.transform.Find("Grid").Find("Spawn").transform.localPosition;
        SnowballScript snowballScript = snowball.transform.GetChild(0).gameObject.GetComponent<SnowballScript>();
        currentLevel.transform.Find("Grid").Find("Ground").gameObject.GetComponent<TilemapCollisionScript>().snowball = snowballScript;
        snowballScript.gameScript = this;
        snowball.SetActive(true);
    }

    private void LoadLevel() {
        if(currentLevel != null) currentLevel.SetActive(false);

        currentLevel = transform.GetChild(currentLevelNum-1).gameObject;
        currentLevel.SetActive(true);
    }
}
