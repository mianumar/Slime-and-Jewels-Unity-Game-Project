using UnityEngine;

namespace Assets_From_Client.Platformer_Art_Pack.Scripts
{
    public class Camera : MonoBehaviour
    {
        public GameObject player;
        public float offset;
        void Update()
        {
            transform.position = new Vector2 (player.transform.position.x, player.transform.position.y + offset);
        }
    }
}
