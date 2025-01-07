using System;
using System.Collections;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// This script will be attached to the InteractableTile Prefab. This will basically help the game create
    /// the 'Mario' like tiles that can be destroyed by the player to reveal a gem or an item.
    /// </summary>
    public class InteractableTile : MonoBehaviour
    {
        /// <summary>
        /// This is a reference to the graphic that will move when the player hits the BottomTrigger
        /// </summary>
        [SerializeField] private Transform graphicToMove;

        /// <summary>
        /// When the player hits this GameObject, we will deactivate it. This will be the trigger that will create
        /// </summary>
        [SerializeField] private GameObject bottomTrigger;

        /// <summary>
        /// Whatever items needs to be moved up, please pass it here through the inspector.
        /// </summary>
        [SerializeField] private Transform itemToMoveUp;

        /// <summary>
        /// When this is true, it will tell the game to completely replace the graphic with the graphic that needs
        /// to be replaced. This is actually required for the graphic at level 5, as the question mark is actually
        /// attached to the graphic itself instead of being a separate graphic.
        /// </summary>
        [SerializeField] private bool completelyReplaceTheGraphic = false;

        /// <summary>
        /// This is a reference to the question mark that should be attached to all of the intractable tiles
        /// </summary>
        [SerializeField] private GameObject referenceToQuestionMarkGameObject;

        /// <summary>
        /// This is a reference to the graphic that we need to replace. This should be a graphic with a
        /// question mark on it. This will usually be used only for level 5, where the question mark is actually
        /// attached to the graphic itself.
        /// </summary>
        [SerializeField] private SpriteRenderer graphicToReplace;
        
        /// <summary>
        /// This is going to tell the game which graphic the graphicToMove should be replaced with.
        /// </summary>
        [SerializeField] private Sprite graphicToReplaceWith;

        /// <summary>
        /// When this turns true, we will tell the game that the player has hit the bottom trigger and this should
        /// not be triggered again.
        /// </summary>
        private bool _hasTriggered = false;

        /// <summary>
        /// This function will be called when the player hits the bottom trigger. This will move the graphic up and
        /// then down again. It will also deactivate the bottom trigger, and it will also make the gem the item
        /// that needs to be moved up to show and exceed.
        /// </summary>
        /// <returns></returns>
        public IEnumerator Routine_MoveBlockAndReturnToOriginalPosition()
        {
            //Setting up a Vector3 variable to store the original position of the graphic
            Vector3 originalPosition = graphicToMove.position;
            
            //Setting up a new Vector3 variable to store the position where the graphic needs to move to
            Vector3 newPosition = new Vector2(originalPosition.x, originalPosition.y + 0.25f);
            
            //Storing the position of the item that needs to be moved up and adding 0.5f to it
            //in the y value
            Vector3 itemToMoveUpPosition = itemToMoveUp.position;
            itemToMoveUpPosition.y += 1.5f;
            
            //While loop to graphicToMove and itemToMoveUp to the newPosition and itemToMoveUpPosition
            while (graphicToMove.position != newPosition)
            {
                //Moving the graphic up
                graphicToMove.position = Vector2.MoveTowards(graphicToMove.position, 
                    newPosition, 0.15f);
                
                //Moving the item up
                itemToMoveUp.position = Vector2.MoveTowards(itemToMoveUp.position, 
                    itemToMoveUpPosition, 0.5f);
                
                //Waiting for 0.02 seconds
                yield return new WaitForSeconds(0.02f);
            }
            
            //While loop to move the ItemToMoveUp to the itemToMoveUpPosition if it hasn't
            //reached it yet, and we will also move the graphicToMove back to its original position
            while (itemToMoveUp.position != itemToMoveUpPosition)
            {
                //Moving the item up
                itemToMoveUp.position = Vector2.MoveTowards(itemToMoveUp.position, 
                    itemToMoveUpPosition, 0.5f);
                
                //Moving the graphic down
                graphicToMove.position = Vector2.MoveTowards(graphicToMove.position, 
                    originalPosition, 0.15f);
                
                //Waiting for 0.02 seconds
                yield return new WaitForSeconds(0.02f);
            }
            
            //While loop to move the graphicToMove back to its original position
            while (graphicToMove.position != originalPosition)
            {
                //Moving the graphic down
                graphicToMove.position = Vector2.MoveTowards(graphicToMove.position, 
                    originalPosition, 0.15f);
                
                //Waiting for 0.02 seconds
                yield return new WaitForSeconds(0.02f);
            }
        }
        
        /// <summary>
        /// This should run alongside the routine above. This will activate the collider of the item
        /// that needs to be moved up.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_ActivateTheColliderOfTheItemToMoveUp()
        {
            yield return new WaitForSeconds(0.5f);
            
            //Activating the collider of the item that needs to be moved up
            itemToMoveUp.GetComponent<Collider2D>().enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //When the collision with the player is detected
            if (other.CompareTag("Player"))
            {
                //If the trigger has already been triggered, we will return
                if (_hasTriggered == true) return;

                //Setting this trigger to true so that it does not get triggered again
                _hasTriggered = true;
                
                //Turning the bottom trigger off
                bottomTrigger.SetActive(false);

                //Starting the co-routine to move the block and return to its original position
                StartCoroutine("Routine_MoveBlockAndReturnToOriginalPosition");
                
                // Starting the co-routine to activate the collider of the item that needs to be moved up
                StartCoroutine("Routine_ActivateTheColliderOfTheItemToMoveUp");

                print("Player has hit the bottom trigger!");
                
                // Handle the case where the graphic needs to be completely replaced
                if (completelyReplaceTheGraphic == true)
                {
                    // Running this only if a value is present in the graphicToReplaceWith
                    if (graphicToReplaceWith != null)
                    {
                        // We will run this only if the graphicToReplace is not null
                        if (graphicToReplace != null)
                        {

                            // We will replace the graphic with the graphic that needs to be replaced
                            graphicToReplace.sprite = graphicToReplaceWith;
                        }
                    }
                }
                else
                {
                    // We will run this only if the referenceToQuestionMarkGameObject is not null
                    if (referenceToQuestionMarkGameObject != null)
                    {
                        // We will deactivate the reference to the question mark game object
                        referenceToQuestionMarkGameObject.SetActive(false);
                    }
                }
            }
        }
        
        /// <summary>
        /// This is going to be the function that will deactivate the collider of the item that needs to be moved up.
        /// </summary>
        private void DeactivateTheColliderOfTheItemToMoveUp()
        {
            //Deactivating the collider of the item that needs to be moved up
            itemToMoveUp.GetComponent<Collider2D>().enabled = false;
        }

        private void Start()
        {
            DeactivateTheColliderOfTheItemToMoveUp();
        }
    }
}