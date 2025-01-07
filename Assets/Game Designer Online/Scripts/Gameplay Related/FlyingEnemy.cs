using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// This script is attached to the FlyingEnemy prefab. This will be used to control the basic movements
    /// of the enemy and also handle the animations that the enemy will play. This simple enemy is just
    /// going to move from one point to the other and then back again.
    /// </summary>
    public class FlyingEnemy : MonoBehaviour
    {
        #region Movement Related Functions and Animation control

        /// <summary>
        /// This function will be used to pick a random animator controller from the list of animator controllers
        /// </summary>
        private void PickARandomRuntimeAnimatorController()
        {
            //This will pick a random animator controller from the list of animator controllers
            var randomIndex = Random.Range(0, enemyAnimatorControllers.Count);
            
            //This will set the animator controller of the movingAvatarAnimator to the random animator controller
            movingAvatarAnimator.runtimeAnimatorController = enemyAnimatorControllers[randomIndex];
        }
        
        /// <summary>
        /// This function will be used to move the enemy from one point to the other and go through
        /// the entire movementPoints list
        /// </summary>
        private void MoveEnemy()
        {
            //Checking the distance between the enemy and the current movement point
            var distanceBetweenEnemyAndMovementPoint = Vector2.Distance(rigidbodyToMove.position,
                movementPoints[_currentMovementIndex].position);
            
            //Calculate the current angle between the enemy and the current movement point
            var angleBetweenEnemyAndMovementPoint = 
                Mathf.Atan2(rigidbodyToMove.position.y - movementPoints[_currentMovementIndex].position.y, 
                    rigidbodyToMove.position.x - movementPoints[_currentMovementIndex].position.x) 
                * Mathf.Rad2Deg;
            
            //If the angle is less than 90 degrees, we will not flip the sprite
            if (angleBetweenEnemyAndMovementPoint < 90f && angleBetweenEnemyAndMovementPoint > -90f)
            {
                //Checking the name of the current animator
                string currentAnimatorName = movingAvatarAnimator.runtimeAnimatorController.name;
                
                //This if and else statement is a solution for the animation of some of the sprites that
                //are flipped and are not configured properly
                if (currentAnimatorName.Contains("bat") || currentAnimatorName.Contains("vulture"))
                {
                    movingAvatarSpriteRenderer.flipX = true;
                }
                else
                {
                    movingAvatarSpriteRenderer.flipX = false;
                }
            }
            else
            {
                //Checking the name of the current animator
                string currentAnimatorName = movingAvatarAnimator.runtimeAnimatorController.name;
                
                //This if and else statement is a solution for the animation of some of the sprites that
                //are flipped and are not configured properly
                if (currentAnimatorName.Contains("bat") || currentAnimatorName.Contains("vulture"))
                {
                    movingAvatarSpriteRenderer.flipX = false;
                }
                else
                {
                    movingAvatarSpriteRenderer.flipX = true;
                }
            }
            
            //If the distance is greater than 0.5f, then the enemy will move towards the current movement point
            //using moveTowards
            if (distanceBetweenEnemyAndMovementPoint > 0.5f)
            {
                rigidbodyToMove.position = Vector2.MoveTowards(rigidbodyToMove.position,
                    movementPoints[_currentMovementIndex].position, 
                    movementSpeed * Time.deltaTime);
            }
            else
            {
                //If the enemy has reached the current movement point, then we will increment the current movement
                //index by 1
                _currentMovementIndex++;
                
                //If the current movement index is greater than the number of movement points, then we will
                //set the current movement index to 0
                if (_currentMovementIndex >= movementPoints.Count)
                {
                    _currentMovementIndex = 0;
                }
            }
        }
        
        [Header("Movement and Animation Related Functions")]
        //Movement points
        [SerializeField]
        private List<Transform> movementPoints;

        /// <summary>
        /// This is a reference to the rigibody that we have to move
        /// </summary>
        [SerializeField] private Rigidbody2D rigidbodyToMove;

        /// <summary>
        /// This is a reference to the sprite renderer of the MovingAvatar that is a child of this gameObject
        /// </summary>
        [SerializeField] private SpriteRenderer movingAvatarSpriteRenderer;

        /// <summary>
        /// This is a reference to the animators that will be used to animate the enemy
        /// </summary>
        [SerializeField] private List<RuntimeAnimatorController> enemyAnimatorControllers;

        /// <summary>
        /// This is a reference to the animator that will be used to animate the enemy
        /// </summary>
        [SerializeField] private Animator movingAvatarAnimator;

        /// <summary>
        /// This will set the movement speed of the enemy
        /// </summary>
        [SerializeField] private float movementSpeed;
        
        /// <summary>
        /// This needs to be set in the inspector. When this variable is true, it will tell the game
        /// to set a random movement speed to the flying enemy at the start of the game
        /// </summary>
        [SerializeField] private bool setRandomSpeedAtStart = false;

        /// <summary>
        /// This will tell the game what movement point the enemy should move towards right now
        /// </summary>
        private int _currentMovementIndex = 0;

        #endregion

        #region Unity Functions

        private void Start()
        {
            //Picking a random animator controller
            PickARandomRuntimeAnimatorController();
            
            //Setting up some of the movement related variables
            _currentMovementIndex = 0;

            //We will set a random speed at start if this variable is true
            if (setRandomSpeedAtStart == true)
            {
                movementSpeed = Random.Range(0.5f, 1.5f);
            }
        }

        private void FixedUpdate()
        {
            MoveEnemy();
        }

        #endregion
    }
}
