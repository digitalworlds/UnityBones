// SamiyyahRucker(MadeLove)
// Displays win Ul and match count

using UnityEngine;
using UnityEngine.UI;

public class PiecesPlacedTracker : MonoBehaviour
{
    public static PiecesPlacedTracker instance;

    private int totalBonesToPlace = 0;
    private int bonesPlaced = 0;

    [Header("Optional UI")]
    public Text progressText;
    public GameObject winScreen;

    void Awake() // ensure theire is only one tracker
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start() // counts how many bones are in the JSON
    {
        // automatically count how many bones were scattered
        totalBonesToPlace = BoneLoader.Instance.GetBoneDictionary().Count;

        if (progressText != null)
            progressText.text = $"0 / {totalBonesToPlace} Bones Placed";

        if (winScreen != null)
            winScreen.SetActive(false);
    }

    public void PiecesPlacedCounter()
    {
        bonesPlaced++;

        Debug.Log($"✅ Placed: {bonesPlaced} / {totalBonesToPlace}");

        if (progressText != null)
            progressText.text = $"{bonesPlaced} / {totalBonesToPlace} Bones Placed";

        if (bonesPlaced >= totalBonesToPlace)
        {
            Debug.Log("🎉 All bones placed! You win!");

            if (winScreen != null)
                winScreen.SetActive(true);
        }
    }
}
