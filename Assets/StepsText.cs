using UnityEngine;
using TMPro;   


public class StepsText : MonoBehaviour
{
    [SerializeField] private SplineKnotAnimate SplineKnotAnimate;
    [SerializeField] private TextMeshPro texto;
    void Start()
    {
        texto = GetComponent<TextMeshPro>();    
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = $"{SplineKnotAnimate.Step}";

        if (SplineKnotAnimate.Step == 0)
        {
            texto.text = "";
        }
    }
}
