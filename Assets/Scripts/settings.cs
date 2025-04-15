using UnityEngine;

public class settings:MonoBehaviour
{
    public GameObject Panel;
    public void OpenPanel(){
        if(Panel!=null){
            //bool isActive =Panel.activeSelf;
            //Panel.SetActive(!(isActive));
            Animator animator = Panel.GetComponent<Animator>();
            if(animator!=null){
                bool isOpen=animator.GetBool("open");

                animator.SetBool("open",!isOpen); 
            }
        }
    }

    public void OpenBonePanel(){
        if(Panel!=null){
            //bool isActive =Panel.activeSelf;
            //Panel.SetActive(!(isActive));
            Animator animator = Panel.GetComponent<Animator>();
            if(animator!=null){
                bool isOpen=animator.GetBool("boneOpen");

                animator.SetBool("boneOpen",!isOpen); 
            }
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
