using UnityEngine;

public class ScaleBasedOnCanvas : MonoBehaviour
{
    public RectTransform obj;  

    [Header("Width percent of canvas")]
    public float widthPercentage; 
    [Header("Height percent of canvas")] 
    public float heightPercentage; 
    [Header("X percent of canvas")]
    public float x; 
    [Header("Y percent of canvas")] 
    public float y; 
    [Header("Settings test")]
    public RectTransform settingStuff; 

    void Start()
    {
        RectTransform canvasRectTransform = obj.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        
        float cWidth = canvasRectTransform.rect.width;
        float cHeight = canvasRectTransform.rect.height;
        float cWmiddle =0; //cWidth/2; 
        float cHmiddle=0;//cHeight/2;

        obj.sizeDelta = new Vector2(cWidth * widthPercentage, cHeight * heightPercentage);

        obj.anchoredPosition = new Vector2(cWidth * x+cWmiddle,  cHeight * y+cHmiddle);
        foreach(RectTransform thing in settingStuff){
            if(thing!=null){
                obj.sizeDelta = new Vector2(cWidth * widthPercentage, cHeight * heightPercentage);
                 obj.anchoredPosition = new Vector2(cWidth * x+cWmiddle,  cHeight * y+cHmiddle);   
            }
        }
    }
}
