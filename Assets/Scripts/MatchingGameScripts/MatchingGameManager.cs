// SamiyyahRucker(MadeLove)
// Tracks correct matches and progress

using System.Collections.Generic;
using UnityEngine;

public class MatchingGameManager : MonoBehaviour
{
    // Access to manager
    public static MatchingGameManager Instance;

    // Creates a dictionry  - stores each bone’s correct target
    private Dictionary<string, Transform> referencePositions = new Dictionary<string, Transform>();

    // Can be used by other scripts
    private void Awake()
    {
        Instance = this;
    }

    // Bone Call During Setup - sets the correct location/rotation for the bone
    public void RegisterCorrectPosition(string boneId, Transform reference)
    {
        if (!referencePositions.ContainsKey(boneId))
        {
            referencePositions[boneId] = reference;
        }
    }

    // Finds that correct position later when checking matches
    public Transform GetReferenceTransform(string boneId)
    {
        return referencePositions.ContainsKey(boneId) ? referencePositions[boneId] : null;
    }
}
