using UnityEngine;

namespace Persistence.Game
{
    public class DeathZone : MonoBehaviour
    {
        public MainManager Manager;

        private void OnCollisionEnter(Collision other)
        {
            Destroy(other.gameObject);
            Manager.GameOver();
        }
    }
}
