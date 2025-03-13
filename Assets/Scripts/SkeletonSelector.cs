using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;

public class SkeletonSelector : MonoBehaviour
{
    [SerializeField] private SkeletonLoader skeletonLoader; // Assign in Inspector
    [SerializeField] private TMP_Dropdown skeletonDropdown; // Changed to TMP_Dropdown

    private async void Start()
    {
        if (skeletonLoader == null)
        {
            Debug.LogError("SkeletonLoader not assigned in Inspector!");
            return;
        }

        if (skeletonDropdown == null)
        {
            Debug.LogError("Dropdown not assigned in Inspector!");
            return;
        }

        // Fetch available skeleton IDs
        List<string> skeletonIds = await skeletonLoader.GetAvailableSkeletonIdsAsync();
        if (skeletonIds.Count == 0) return;

        // Populate dropdown
        skeletonDropdown.ClearOptions();
        skeletonDropdown.AddOptions(skeletonIds);
        Debug.Log($"Dropdown populated with {skeletonIds.Count} skeletons: {string.Join(", ", skeletonIds)}");

        // Load the first skeleton by default (optional)
        if (skeletonIds.Count > 0)
        {
            await skeletonLoader.LoadSkeletonByIdAsync(skeletonIds[0]);
            skeletonDropdown.value = 0; // Set dropdown to first option
        }

        // Add listener for dropdown changes
        skeletonDropdown.onValueChanged.AddListener(OnSkeletonSelected);
    }

    private async void OnSkeletonSelected(int index)
    {
        string selectedId = skeletonDropdown.options[index].text;
        Debug.Log($"Skeleton selected from dropdown: {selectedId}");
        await skeletonLoader.LoadSkeletonByIdAsync(selectedId);
    }
}