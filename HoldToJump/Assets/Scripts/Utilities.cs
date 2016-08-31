using UnityEngine;
using System.Collections;

namespace Utility
{
    public class Utilities
    {
        /// <summary>
        /// Checks if the counter is equal or greater than the interval.
        /// </summary>
        /// <param name="counter">The Counter</param>
        /// <param name="interval">The interval</param>
        /// <returns>Bool acording to the codition.</returns>
        public static bool IsCounterGreaterThanInterval(float counter, float interval)
        {
            if (counter >= interval)
            {                
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activationStatus"></param>
        /// <param name="gameObject"></param>
        public static void ActivateOrDeactivateGameObject(bool activationStatus,GameObject gameObject)
        {
            gameObject.SetActive(activationStatus);
        }
    }
}
