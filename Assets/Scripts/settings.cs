using UnityEngine;

public class settings:MonoBehaviour
{
    public GameObject Panel;
    public void OpenPanel(){
        if(Panel!=null){
            bool isActive =Panel.activeSelf;
            Panel.SetActive(!(isActive));
        }
    }

    public void startClosed(){
        if(Panel!=null){
            Panel.SetActive(false);
        }
    }
    public void startOpen(){
        if(Panel!=null){
            Panel.SetActive(true);
        }
    }
}
