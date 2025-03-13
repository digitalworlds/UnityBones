using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using GLTFast;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class BoneLoader : MonoBehaviour
{
    public static BoneLoader Instance;

    public string json_url = "https://digitalworlds.github.io/CURE25_Test/models/Callithrix/Callithrix.json";

    private string glb_url;
    private Dictionary<string, BoneInfo> boneDict;

    private Dictionary<string, GameObject> objectDict;

    private bool JSON_LOADED=false;
    private bool GLB_LOADED=false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        StartCoroutine(DownloadJson());
    }

    IEnumerator DownloadJson()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(json_url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonText = request.downloadHandler.text;
                Debug.Log("JSON Loaded: " + jsonText);

                // parse JSON
                BoneDataset data = JsonUtility.FromJson<BoneDataset>(jsonText);


                glb_url=json_url.Substring(0,json_url.LastIndexOf("/")+1)+data.url;

                LoadGLB();

                boneDict = new Dictionary<string, BoneInfo>();

                // add to dictionary 
                foreach (var entry in data.bones)
                {
                    boneDict[entry.id] = entry;
                }

                Debug.Log("LandmarkDataLoader: Loaded " + boneDict.Count + " landmarks from JSON.");
                JSON_LOADED=true;
            }
            else
            {
                Debug.LogError("Failed to load JSON: " + request.error);
            }
        }
    }

    private GameObject FindChildGameObjectRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child.gameObject;

            GameObject found = FindChildGameObjectRecursive(child, name);
            if (found != null)
                return found;
        }
        return null;
    }

    private async void LoadGLB(){
        var gltfImport = new GltfImport();

        GameObject newChild = new GameObject("bones");
        newChild.transform.parent = transform; 
        newChild.SetActive(false);

        await gltfImport.Load(glb_url);
        var instantiator = new GameObjectInstantiator(gltfImport,newChild.transform);
        var success=await gltfImport.InstantiateMainSceneAsync(instantiator);
        if (success) {
            Debug.Log("GLTF file is loaded.");
            
            // Assuming "gameObject" is your desired GameObject

        
            /*Renderer renderer = gameObject.GetComponentInChildren<Renderer>();

            Bounds boundingBox = renderer.bounds; 

            // Accessing specific values from the bounding box

            Vector3 center = boundingBox.center;

            Vector3 extents = boundingBox.extents;
            Debug.Log(extents);

            float size=Math.Max(Math.Max(extents.x,extents.y), extents.z);
            if(size==0)size=1;

            transform.localScale=new Vector3(1/size,1/size,1/size);*/


            objectDict = new Dictionary<string, GameObject>();
            foreach (var entry in boneDict)
            {
                BoneInfo bone = entry.Value;
                
                objectDict[bone.id]=FindChildGameObjectRecursive(transform,bone.id);
            }

            FindChildGameObjectRecursive(transform,"name");

            GLB_LOADED=true;

            Debug.Log(GetBoneObject("frontal"));

        }else{
            Debug.Log("ERROR: GLTF file is NOT loaded!");
        }
    }

    public Dictionary<string, BoneInfo> GetBoneDictionary(){
        return boneDict;
    }

    public BoneInfo GetBoneInfo(string id)
    {
        if (boneDict == null)
        {
            Debug.LogError("LandmarkDataLoader: Data not loaded or landmarkDict is null!");
            return null;
        }

        if (boneDict.ContainsKey(id))
        {
            return boneDict[id];
        }
        else
        {
            Debug.LogWarning("LandmarkDataLoader: No landmark found for id: " + id);
            return null;
        }
    }

    public GameObject GetBoneObject(string id)
    {
        if (objectDict == null)
        {
            Debug.LogError("ObjectDictionary: Data not loaded or landmarkDict is null!");
            return null;
        }

        if (objectDict.ContainsKey(id))
        {
            return objectDict[id];
        }
        else
        {
            Debug.LogWarning("ObjectDictionary: No landmark found for id: " + id);
            return null;
        }
        
    }

    public bool IsJSONLoaded(){return JSON_LOADED;}
    public bool IsGLBLoaded(){return GLB_LOADED;}
}