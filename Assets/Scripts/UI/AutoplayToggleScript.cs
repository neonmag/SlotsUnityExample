using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoplayToggleScript : MonoBehaviour
{
    [SerializeField] private GenerateFieldScript generateFieldScript;
    private float currentTime = 0f;
    private void Start()
    {
        if (generateFieldScript != null)
        {
            generateFieldScript.onRespin += OnRespin;
        }
    }

    private void Respin()
    {
        if (StaticLogicScript.isActiveButton)
        {
            generateFieldScript.onRespin?.Invoke();
        }
    }
    public void SetIsPlay()
    {
        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            Respin();
        }
    }

    private void OnRespin() { }

    private void Update()
    {
        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            this.currentTime += Time.deltaTime;
            if (currentTime >= 1f)
            {
                currentTime = 0f;
                SetIsPlay();
            }
        }
    }
}
