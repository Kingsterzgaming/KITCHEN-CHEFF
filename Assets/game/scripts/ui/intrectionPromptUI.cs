using TMPro;
using UnityEngine;
using YesChef.Player;
using YesChef.Stations;

namespace YesChef.UI
{
    public class InteractionPromptUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerController player;

        [Header("UI")]
        [SerializeField] private GameObject promptPanel;
        [SerializeField] private TMP_Text promptText;

        private void Update()
        {
            if (player == null)
            {
                HidePrompt();
                return;
            }

            if (!player.HasInteractable)
            {
                HidePrompt();
                return;
            }

            ShowPrompt();
        }

        private void ShowPrompt()
        {
            if (promptPanel != null)
            {
                promptPanel.SetActive(true);
            }

            if (promptText != null)
            {
                promptText.text = "E - Interact";
            }
        }

        private void HidePrompt()
        {
            if (promptPanel != null)
            {
                promptPanel.SetActive(false);
            }
        }
    }
}