using UnityEngine;
using TMPro;

public class BoneInfoUIManager : MonoBehaviour
{
    public static BoneInfoUIManager Instance;

    public GameObject boneInfoPanel;
    public TextMeshProUGUI boneNameText;
    public TextMeshProUGUI boneFactText;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (boneInfoPanel != null)
        {
            boneInfoPanel.SetActive(false);
            Debug.Log("📦 BoneInfoUIManager initialized — panel hidden on start.");
        }
    }

    public void ShowInfo(string name, string fact)
    {
        Debug.Log($"🧠 Showing bone info: {name} - {fact}");

        if (boneNameText != null)
            boneNameText.text = name;

        if (boneFactText != null)
            boneFactText.text = fact;

        if (boneInfoPanel != null)
        {
            boneInfoPanel.SetActive(true);
            boneInfoPanel.transform.SetAsLastSibling(); // bring to front
        }
        else
        {
            Debug.LogError("❌ boneInfoPanel is null in ShowInfo");
        }
    }

    public void HideInfo()
    {
        if (boneInfoPanel != null)
        {
            boneInfoPanel.SetActive(false);
            Debug.Log("🔕 Bone info panel hidden.");
        }
    }
}
