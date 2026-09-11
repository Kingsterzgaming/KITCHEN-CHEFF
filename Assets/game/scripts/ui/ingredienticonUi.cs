using UnityEngine;
using UnityEngine.UI;

namespace YesChef.UI
{
    public class IngredientIconUI : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private Image iconImage;
        [SerializeField] private GameObject deliveredOverlay;

        public void SetIcon(Sprite sprite)
        {
            if (iconImage == null)
                return;

            iconImage.sprite = sprite;
            iconImage.enabled = sprite != null;
        }

        public void SetDelivered(bool delivered)
        {
            if (deliveredOverlay != null)
            {
                deliveredOverlay.SetActive(delivered);
            }
        }

        public void Clear()
        {
            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }

            if (deliveredOverlay != null)
            {
                deliveredOverlay.SetActive(false);
            }
        }
    }
}