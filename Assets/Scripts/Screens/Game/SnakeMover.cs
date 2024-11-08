using System;
using System.Collections.Generic;
using System.Linq;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace Screens.Game
{
    [AddComponentMenu("Scripts/Screens/Game/Screens.Game.SnakeMover")]
    internal class SnakeMover : MonoBehaviour
    {
        [SerializeField]
        private float moveTime;

        [SerializeField]
        private RectTransform head;

        [SerializeField]
        private Rigidbody2D headRigid;

        [SerializeField]
        private List<RectTransform> pickups;

        [SerializeField]
        private RectTransform leftUp;

        [SerializeField]
        private RectTransform rightDown;

        [SerializeField]
        private DistanceJoint2D tailPrefab;

        [SerializeField]
        private List<Sprite> snakeSkins;

        [SerializeField]
        private FloatingJoystick floatingJoystick;

        [SerializeField]
        private float snakeSpeed;
        private float _snakeSpeed;

        [SerializeField]
        private SnakeHead snakeHead;

        [SerializeField]
        private float speedUpSpeed;

        [SerializeField]
        private float speedUpTime;

        public event Action OnPlanetCollected;
        public event Action OnObstacleHinted;

        [SerializeField]
        private int startLength = 1;
        private int currentLength;

        private List<DistanceJoint2D> tails = new();
        private Sprite currentSprite;
        private Timer timer = new();

        private void Awake()
        {
            snakeHead.TouchedObstacle += SnakeHead_TouchedObstacle;
            snakeHead.TouchedPickup += SnakeHead_TouchedPickup;
            _snakeSpeed = snakeSpeed;
            timer.Duration = speedUpTime;
            timer.OnEnd += () => _snakeSpeed = snakeSpeed;
        }

        private void SnakeHead_TouchedPickup(Pickup obj)
        {
            OnPlanetCollected?.Invoke();
            AddTail();
            Destroy(obj.gameObject);
        }

        private void SnakeHead_TouchedObstacle()
        {
            OnObstacleHinted?.Invoke();
        }

        public void ResetSnake()
        {
            currentLength = startLength;
            currentSprite = snakeSkins[GameConfig.StaticData.currentSnakeSkin];
            head.GetComponent<Image>().sprite = currentSprite;
            ClearTails();
            for (int i = 0; i < currentLength; i++)
            {
                AddTail();
            }
        }

        private void ClearTails()
        {
            for (int i = 0; i < tails.Count; i++)
            {
                Destroy(tails[i].gameObject);
            }
            tails.Clear();
        }

        private void AddTail()
        {
            DistanceJoint2D tail = Instantiate(tailPrefab, transform);
            tail.connectedBody =
                tails.Count == 0 ? headRigid : tails.Last().GetComponent<Rigidbody2D>();
            (tail.transform as RectTransform).anchoredPosition3D = (
                tail.connectedBody.transform as RectTransform
            ).anchoredPosition3D;
            tail.GetComponent<Image>().sprite = currentSprite;
            tail.GetComponent<SnakeTail>().SnakeHead = head.GetComponent<SnakeHead>();
            tail.transform.SetAsFirstSibling();
            tails.Add(tail);
        }

        private bool movementEnabled = false;

        public void EnableMovement()
        {
            movementEnabled = true;
        }

        public void DisableMovement()
        {
            movementEnabled = false;
            timer.Stop();
        }

        public void AddSpeedUp()
        {
            if (timer.IsTicking)
            {
                timer.AddTime(speedUpTime);
            }
            else
            {
                _snakeSpeed = speedUpSpeed;
                timer.Start();
            }
        }

        private void Update()
        {
            timer.Update(Time.deltaTime);

            if (!movementEnabled)
            {
                return;
            }

            Vector3 newPosition =
                head.position
                + (_snakeSpeed * Time.deltaTime * (Vector3)floatingJoystick.Direction);

            newPosition.x = Mathf.Clamp(newPosition.x, leftUp.position.x, rightDown.position.x);
            newPosition.y = Mathf.Clamp(newPosition.y, rightDown.position.y, leftUp.position.y);

            head.position = newPosition;
        }
    }
}
