using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateFieldScript : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();
    private List<GameObject> fieldObjects = new List<GameObject>();
    private List<GameObject> animatedObjects = new List<GameObject>();
    private List<GameObject> winObjects = new List<GameObject>();
    private List<GameObject> highLighted = new List<GameObject>();

    public Action onRespin;

    [SerializeField]
    private AudioSource respinSound;

    [SerializeField]
    private List<GameObject> indicators = new List<GameObject>();

    [SerializeField]
    private TextMeshProUGUI winText;
    private int amountOfAllWins = 0;

    private float currentTime = 0f;

    [SerializeField]
    private AudioSource winAudio;
    private bool isActiveWin = true;

    [SerializeField]
    private TextMeshProUGUI totalBetText;

    private Dictionary<String, float> bets = new Dictionary<String, float>();

    private void Start()
    {
        StaticLogicScript.isActiveButton = true;

        StaticLogicScript.totalBet = 1000;

        StaticLogicScript.currentBet = 1;

        onRespin += RandomElementsInField;

        GameObject lines = GameObject.Find("Lines");


        InitializeBets();


        InitializeField(lines);


        RandomElementsInField();
    }

    private void InitializeBets()
    {
        bets.Add(sprites[0].name, 0.1f);
        bets.Add(sprites[1].name, 0.5f);
        bets.Add(sprites[2].name, 0.15f);
        bets.Add(sprites[3].name, 0.25f);
        bets.Add(sprites[4].name, 0.5f);
        bets.Add(sprites[5].name, 0.7f);
        bets.Add(sprites[6].name, 0.8f);
        bets.Add(sprites[7].name, 0.9f);
        bets.Add(sprites[8].name, 1f);
        bets.Add(sprites[9].name, 5f);
    }

    private void InitializeField(GameObject lines)
    {
        for (int i = 0; i < lines.transform.childCount; i++)
        {
            GameObject lineElement = lines.transform.GetChild(i).gameObject;
            animatedObjects.Add(lineElement);
            for (int j = 0; j < lineElement.transform.childCount; j++)
            {
                fieldObjects.Add(lineElement.transform.GetChild(j).gameObject);
            }
        }

        for(int i = 0; i < lines.transform.childCount; i++)
        {
            winObjects.Add(GameObject.FindGameObjectWithTag(lines.transform.GetChild(i).gameObject.name + "Upper"));
            winObjects.Add(GameObject.FindGameObjectWithTag(lines.transform.GetChild(i).gameObject.name + "Middle"));
            winObjects.Add(GameObject.FindGameObjectWithTag(lines.transform.GetChild(i).gameObject.name + "Lower"));
        }

        for(int i = 0; i < winObjects.Count; i++)
        {
            highLighted.Add(winObjects[i].gameObject.transform.GetChild(0).gameObject);
            highLighted[i].SetActive(false);
        }
    }

    public void RandomElementsInField()
    {
        foreach(var item in fieldObjects)
        {
            int randNum = UnityEngine.Random.Range(0, sprites.Count);
            item.GetComponent<SpriteRenderer>().sprite = sprites[randNum];
        }
        foreach(var item in animatedObjects)
        {
            item.GetComponent<Animator>().Play(item.gameObject.name, 0, 0.0f);
        }
        StaticLogicScript.isActiveButton = false;

        respinSound.Play();

        StaticLogicScript.totalBet -= StaticLogicScript.currentBet;
        totalBetText.text = $"Total bet: {StaticLogicScript.totalBet}";
        RemoveIndicators();
        HighLightOff();
        CheckWin();
    }

    public void CheckWin()
    {
        for (int i = 0; i < winObjects.Count -6; i++)
        {
            if (winObjects[i].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name &&
                winObjects[i].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 6].GetComponent<SpriteRenderer>().sprite.name)
            {
                SetIndicator(i);
                HighLight(i);
            }
            else if (winObjects[i].GetComponent<SpriteRenderer>().sprite.name == sprites[9].name &&
                winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 6].GetComponent<SpriteRenderer>().sprite.name)
            {
                SetIndicator(i);
                HighLight(i);
            }
            else if (winObjects[i].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 6].GetComponent<SpriteRenderer>().sprite.name &&
                winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name == sprites[9].name)
            {
                SetIndicator(i);
                HighLight(i);
            }
            else if (winObjects[i].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name &&
                winObjects[i + 6].GetComponent<SpriteRenderer>().sprite.name == sprites[9].name)
            {
                SetIndicator(i);
                HighLight(i);
            }
            else if (winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name == sprites[9].name &&
                winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 6].GetComponent<SpriteRenderer>().sprite.name)
            {
                SetIndicator(i);
                HighLight(i);
            }
            else if (winObjects[i].GetComponent<SpriteRenderer>().sprite.name == sprites[9].name &&
                winObjects[i].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 6].GetComponent<SpriteRenderer>().sprite.name)
            {
                SetIndicator(i);
                HighLight(i);
            }
            else if (winObjects[i].GetComponent<SpriteRenderer>().sprite.name == sprites[9].name &&
                winObjects[i].GetComponent<SpriteRenderer>().sprite.name == winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name)
            {
                SetIndicator(i);
                HighLight(i);
            }
        }
    }

    public void HighLight(int i)
    {
        highLighted[i].SetActive(true);
        highLighted[i + 3].SetActive(true);
        highLighted[i + 6].SetActive(true);

        highLighted[i].GetComponent<Animator>().Play("Win", 0, 0.0f);
        highLighted[i + 3].GetComponent<Animator>().Play("Win", 0, 0.0f);
        highLighted[i + 6].GetComponent<Animator>().Play("Win", 0, 0.0f);

        StaticLogicScript.totalBet += (
            bets[winObjects[i].GetComponent<SpriteRenderer>().sprite.name]
            + bets[winObjects[i + 3].GetComponent<SpriteRenderer>().sprite.name]
            + bets[winObjects[i + 6].GetComponent<SpriteRenderer>().sprite.name]) * StaticLogicScript.currentBet * 0.25f;

        if (isActiveWin)
        {
            this.isActiveWin = false;
            this.winAudio.Play();
        }

        this.amountOfAllWins++;
        winText.text = $"All wins: {this.amountOfAllWins}";
        totalBetText.text = $"Total bet: {StaticLogicScript.totalBet}";
    }

    public void HighLightOff()
    {
        foreach (var i in highLighted)
        {
            i.SetActive(false);
        }
    }

    public void SetIndicator(int i)
    {
        GameObject indicator = Instantiate(indicators[UnityEngine.Random.Range(0, indicators.Count)]);

        indicator.gameObject.transform.SetParent(highLighted[i + 3].transform);
        indicator.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void RemoveIndicators()
    {
        GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("Indicator");
        foreach (var i in toDestroy)
        {
            Destroy(i.gameObject);
        }
    }

    private void Update()
    {
        if(StaticLogicScript.totalBet <= 0)
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && StaticLogicScript.isActiveButton) {
            RandomElementsInField();
            StaticLogicScript.isActiveButton = false;
        }
        if (!StaticLogicScript.isActiveButton)
        {
            this.currentTime += Time.deltaTime;
            if(this.currentTime >= 0.5f)
            {
                this.currentTime = 0f;
                StaticLogicScript.isActiveButton = true;
                this.isActiveWin = true;
            }
        }
    }
}
