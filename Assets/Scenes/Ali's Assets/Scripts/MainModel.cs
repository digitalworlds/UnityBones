using UnityEngine;

public class MainModel : MonoBehaviour
{
    private bool initialized=false; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!initialized && BoneLoader.Instance.IsGLBLoaded())
        {

            //For all bones in my dictionary
            foreach(var entry in BoneLoader.Instance.GetBoneDictionary())
            {
                BoneInfo bone=entry.Value;
                //To access the bone full name: bone.name
                //To access the bone facts: bone.fact

                //Get the bone
                GameObject newInstance=BoneLoader.Instance.GetNewBoneInstance(bone.id);
                newInstance.transform.parent=transform;
                newInstance.transform.localPosition=new Vector3(0,0,100); 
            }

            

            initialized=true;
        }


    }
}
