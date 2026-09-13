using UnityEngine;

namespace YesChef.Core
{
    public class HighScoreManager : MonoBehaviour
    {
        private const string HighScoreKey = "YesChef.HighScore";

        public int HighScore
        {
            get
            {
                return Mathf.Max(
                    0,
                    PlayerPrefs.GetInt(HighScoreKey, 0)
                );
            }
        }

        public bool TrySetHighScore(int score)
        {
            if (score <= HighScore)
                return false;

            PlayerPrefs.SetInt(
                HighScoreKey,
                score
            );

            PlayerPrefs.Save();

            return true;
        }

        public void ResetHighScore()
        {
            PlayerPrefs.DeleteKey(HighScoreKey);
            PlayerPrefs.Save();
        }
    }
}