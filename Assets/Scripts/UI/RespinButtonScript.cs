using UnityEngine;

public class RespinButtonScript : MonoBehaviour
{
    [SerializeField] private GenerateFieldScript generateFieldScript;

    private void Start()
    {
        if (generateFieldScript != null)
        {
            generateFieldScript.onRespin += OnRespin;
        }
    }

    public void Respin()
    {
        if (StaticLogicScript.isActiveButton)
        {
            generateFieldScript.onRespin?.Invoke();
        }
    }

    private void OnRespin(){}
}
