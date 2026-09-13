using UnityEngine;
using UnityEngine.UI;

namespace YesChef.UI
{
    public class IngredientIconUI : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Image deliveredImage;

        public void Show(
            Sprite normalSprite,
            Sprite deliveredSprite,
            bool delivered)
        {
            if (iconImage != null)
            {
                iconImage.sprite = normalSprite;
                iconImage.enabled =
                    !delivered && normalSprite != null;
            }

            if (deliveredImage != null)
            {
                deliveredImage.sprite = deliveredSprite;
                deliveredImage.enabled =
                    delivered && deliveredSprite != null;
            }
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
    }
}