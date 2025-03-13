using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoneMatchingGame : MonoBehaviour
{
    [System.Serializable]
    public class BoneCard
    {
        public string boneName;
        public Button button;
        public bool isMatched = false;
    }

    public List<BoneCard> boneCards = new List<BoneCard>();
    private List<BoneCard> selectedCards = new List<BoneCard>();
    public int cardsToMatch = 2; // Change this to 3 or 4 if needed

    public Text resultText; // UI feedback text
    public float messageDuration = 1.5f;
    public LineRenderer lineRendererPrefab; // Prefab for the line
    private List<LineRenderer> activeLines = new List<LineRenderer>();

    void Start()
    {
        foreach (BoneCard card in boneCards)
        {
            card.button.onClick.AddListener(() => SelectCard(card));
        }
        resultText.gameObject.SetActive(false);
    }

    void SelectCard(BoneCard selectedCard)
    {
        if (selectedCard.isMatched || selectedCards.Contains(selectedCard)) return;

        selectedCards.Add(selectedCard);

        if (selectedCards.Count == cardsToMatch)
        {
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);

        string firstBoneName = selectedCards[0].boneName;
        bool allMatch = true;

        foreach (var card in selectedCards)
        {
            if (card.boneName != firstBoneName)
            {
                allMatch = false;
                break;
            }
        }

        if (allMatch)
        {
            foreach (var card in selectedCards)
            {
                card.isMatched = true;
            }
            DrawLineBetweenMatches(selectedCards);
            ShowMessage("✅ Correct Match!", Color.green);
        }
        else
        {
            ShowMessage("❌ Incorrect Match! Try Again.", Color.red);
        }

        selectedCards.Clear();
    }

    void DrawLineBetweenMatches(List<BoneCard> matchedCards)
    {
        if (matchedCards.Count < 2) return; // Need at least 2 points

        // Create a new LineRenderer for each match
        LineRenderer line = Instantiate(lineRendererPrefab, transform);
        line.positionCount = matchedCards.Count;

        for (int i = 0; i < matchedCards.Count; i++)
        {
            line.SetPosition(i, matchedCards[i].button.transform.position);
        }

        activeLines.Add(line);
    }

    void ShowMessage(string message, Color color)
    {
        resultText.text = message;
        resultText.color = color;
        resultText.gameObject.SetActive(true);
        StartCoroutine(HideMessageAfterDelay());
    }

    IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        resultText.gameObject.SetActive(false);
    }
}




