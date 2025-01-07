using System;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// This script will basically handle the container of the chest, and we need it so we can
    /// set the position of the chest randomly
    /// </summary>
    public class ChestContainer : MonoBehaviour
    {
        private void Start()
        {
            //Get all the children of the chest container
            Transform[] children = GetComponentsInChildren<Transform>();
            
            //Loop through all the children and disable them
            foreach (Transform child in children)
            {
                if (child != transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
            
            //Enable one of the children randomly
            children[UnityEngine.Random.Range(1, children.Length)].gameObject.SetActive(true);
        }
    }
}
