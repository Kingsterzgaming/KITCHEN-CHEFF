using UnityEngine;
using UnityEngine.UI;

namespace YesChef.UI
{
    public class IngredientIconUI : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image deliveredImage;

        public void SetIcon(Sprite sprite)
        {
            if (iconImage == null)
                return;

            iconImage.sprite = sprite;
            iconImage.enabled = sprite != null;
        }

        public void SetDelivered(bool delivered)
        {
            if (iconImage != null)
                iconImage.enabled = !delivered;

            if (deliveredImage != null)
                deliveredImage.enabled = delivered;
        }

        public void Clear()
        {
            if (iconImage != null)
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
            }

            if (deliveredImage != null)
            {
                deliveredImage.sprite = null;
                deliveredImage.enabled = false;
            }
        }

        public void SetDeliveredSprite(Sprite sprite)
        {
            if (deliveredImage == null)
                return;

            deliveredImage.sprite = sprite;
        }
    }
}