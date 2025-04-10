using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Slider rotationSliderLR;
    public Slider rotationSliderUD;
    public Slider scaleSlider;
    private float angleSliderNumLR;
    private float angleSliderNumUD;
    private float scaleSliderNum;
    void Update()
    {
        angleSliderNumLR = rotationSliderLR.value*10f;
        angleSliderNumUD = rotationSliderUD.value*10f;
        this.transform.rotation = Quaternion.Euler(angleSliderNumUD,angleSliderNumLR,0);

        scaleSliderNum = scaleSlider.value;
        Vector3 scale = new Vector3(scaleSliderNum,scaleSliderNum,scaleSliderNum);
        this.transform.localScale = scale; 
        
    }
}
