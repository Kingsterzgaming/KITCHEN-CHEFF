using UnityEngine;

namespace YesChef.Ingredients
{
    public class IngredientVisual : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private GameObject rawVisual;
        [SerializeField] private GameObject preparedVisual;

        public void SetPrepared(bool prepared)
        {
            if (rawVisual != null)
            {
                rawVisual.SetActive(!prepared);
            }

            if (preparedVisual != null)
            {
                preparedVisual.SetActive(prepared);
            }
        }
    }
}