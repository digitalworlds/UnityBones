using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections.Generic;
using GLTFast;
using System;

public class SkeletonLoader : MonoBehaviour
{
    private const string BASE_URL = "https://raw.githubusercontent.com/digitalworlds/CURE25_Test/main/Skeletons/";
    private Dictionary<string, SkeletonData> skeletonDataDict = new Dictionary<string, SkeletonData>();
    private GameObject currentSkeleton;
    [SerializeField] private string defaultSkeletonId;

    public GameObject CurrentSkeleton => currentSkeleton;
    public SkeletonData CurrentSkeletonData => skeletonDataDict.ContainsKey(currentSkeleton?.name) ? skeletonDataDict[currentSkeleton.name] : null;

    public async Task LoadAllSkeletonsAsync()
    {
        try
        {
            string indexUrl = $"{BASE_URL}skeletons_index.json";
            string indexJson = await FetchTextAsync(indexUrl);
            if (string.IsNullOrEmpty(indexJson))
            {
                Debug.LogError("Failed to load skeletons_index.json");
                return;
            }

            SkeletonIndex index = JsonUtility.FromJson<SkeletonIndex>(indexJson);
            if (index == null || index.available_skeletons == null)
            {
                Debug.LogError("Invalid or empty skeletons_index.json");
                return;
            }

            foreach (var skeleton in index.available_skeletons)
            {
                await LoadSkeletonDataAsync(skeleton, false);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error during skeleton loading: {e.Message}");
        }
    }

    private async Task LoadSkeletonDataAsync(SkeletonEntry entry, bool destroyPrevious = true)
    {
        string jsonUrl = $"{BASE_URL}{entry.data_file}.json";
        string jsonText = await FetchTextAsync(jsonUrl);
        if (string.IsNullOrEmpty(jsonText))
        {
            Debug.LogError($"Failed to load {entry.data_file}");
            return;
        }

        SkeletonData data = JsonUtility.FromJson<SkeletonData>(jsonText);
        if (data == null)
        {
            Debug.LogError($"Failed to parse JSON for {entry.id}");
            return;
        }

        skeletonDataDict[entry.id] = data;

        string glbUrl = $"{BASE_URL}{entry.glb_path}.glb";
        await LoadGLBModelAsync(glbUrl, entry.id, destroyPrevious);
    }

    private async Task LoadGLBModelAsync(string glbUrl, string skeletonId, bool destroyPrevious)
    {
        GameObject skeletonRoot = new GameObject(skeletonId);
        var gltfImport = new GltfImport();

        byte[] glbData = await FetchBinaryAsync(glbUrl);
        if (glbData == null)
        {
            Debug.LogError($"Failed to fetch GLB from {glbUrl}");
            Destroy(skeletonRoot);
            return;
        }

        bool loadSuccess = await gltfImport.LoadGltfBinary(glbData);
        if (!loadSuccess)
        {
            Debug.LogError($"Failed to parse GLB for {skeletonId}");
            Destroy(skeletonRoot);
            return;
        }

        var instantiator = new GameObjectInstantiator(gltfImport, skeletonRoot.transform);
        bool instantiateSuccess = await gltfImport.InstantiateMainSceneAsync(instantiator);
        if (!instantiateSuccess)
        {
            Debug.LogError($"Failed to instantiate GLB for {skeletonId}");
            Destroy(skeletonRoot);
            return;
        }

        if (destroyPrevious && currentSkeleton != null) Destroy(currentSkeleton);
        currentSkeleton = skeletonRoot;
        currentSkeleton.transform.position = Vector3.zero;
        currentSkeleton.transform.rotation = Quaternion.identity;

        Renderer renderer = currentSkeleton.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            Bounds boundingBox = renderer.bounds;
            Vector3 extents = boundingBox.extents;
            float size = Mathf.Max(Mathf.Max(extents.x, extents.y), extents.z);
            if (size == 0) size = 1;
            currentSkeleton.transform.localScale = new Vector3(1f / size, 1f / size, 1f / size);
        }
        else
        {
            Debug.LogWarning($"No renderer found for {skeletonId}");
        }

        SetupBoneInteractions(skeletonId, currentSkeleton);
    }

    private async Task<string> FetchTextAsync(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Fetch failed for {url}: {request.error}");
            return null;
        }

        return request.downloadHandler.text;
    }

    private async Task<byte[]> FetchBinaryAsync(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Fetch failed for {url}: {request.error}");
            return null;
        }

        return request.downloadHandler.data;
    }

    public async Task LoadSkeletonByIdAsync(string skeletonId, bool destroyPrevious = true)
    {
        string indexUrl = $"{BASE_URL}skeletons_index.json";
        string indexJson = await FetchTextAsync(indexUrl);
        if (string.IsNullOrEmpty(indexJson))
        {
            Debug.LogError($"No data fetched from {indexUrl}");
            return;
        }

        SkeletonIndex index = JsonUtility.FromJson<SkeletonIndex>(indexJson);
        if (index == null || index.available_skeletons == null)
        {
            Debug.LogError("Failed to parse skeletons_index.json into SkeletonIndex");
            return;
        }

        var entry = index.available_skeletons.Find(s => s.id == skeletonId);
        if (entry != null)
        {
            await LoadSkeletonDataAsync(entry, destroyPrevious);
        }
        else
        {
            Debug.LogError($"Skeleton {skeletonId} not found in index");
        }
    }

    public async Task<List<string>> GetAvailableSkeletonIdsAsync()
    {
        string indexUrl = $"{BASE_URL}skeletons_index.json";
        string indexJson = await FetchTextAsync(indexUrl);
        if (string.IsNullOrEmpty(indexJson))
        {
            Debug.LogError($"No data fetched from {indexUrl}");
            return new List<string>();
        }

        SkeletonIndex index = JsonUtility.FromJson<SkeletonIndex>(indexJson);
        if (index == null || index.available_skeletons == null)
        {
            Debug.LogError("Failed to parse skeletons_index.json");
            return new List<string>();
        }

        return index.available_skeletons.ConvertAll(s => s.id);
    }

    private void SetupBoneInteractions(string skeletonId, GameObject skeletonObj)
    {
        SkeletonData data = skeletonDataDict[skeletonId];
        foreach (var bone in data.bones)
        {
            Transform boneTransform = skeletonObj.transform.Find(bone.id);
            if (boneTransform != null)
            {
                BoneInteraction boneInteraction = boneTransform.gameObject.AddComponent<BoneInteraction>();
                boneInteraction.Initialize(bone, this);
            }
            else
            {
                Debug.LogWarning($"Bone {bone.id} not found in {skeletonId}");
            }
        }
    }
}