using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private Stack<Transform> towerA = new Stack<Transform>();
    private Stack<Transform> towerB = new Stack<Transform>();
    private Stack<Transform> towerC = new Stack<Transform>();

    private Transform towerAPosition;
    private Transform towerBPosition;
    private Transform towerCPosition;

    public float diskHeight = 1f;
    public float minDiskRadius = 2f;

    private bool isChooseTower = false;
    private int towerChoice;
    private Transform liftedDisk = null;
    private Vector3 originalDiskPosition;

    private float heightDiskBeforeMove = 6f;
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float changeTowerSpeed = 20f;

    private bool isLifting = false;
    private bool isLowering = false;
    private bool isMoving = false;

    private Vector3 highTargetPosition;
    private Vector3 targetPosition;

    private GameUIController gameUIController;
    private GameObject pausePanel;
    private GameObject winPopup;

    public RectTransform pauseButtonRect;
    public RectTransform resumeButtonRect;
    public RectTransform homeButtonRect;

    public AudioClip liftSound; // Âm thanh khi nâng đĩa
    public AudioClip dropSound; // Âm thanh khi đặt đĩa
    public AudioClip winSound; // Âm thanh khi win
    private AudioSource audioSource; // Nguồn phát âm thanh

    public Material donutMaterial;

    void Awake()
    {
        towerAPosition = GameObject.Find("TowerA").transform;
        towerBPosition = GameObject.Find("TowerB").transform;
        towerCPosition = GameObject.Find("TowerC").transform;
        gameUIController = FindObjectOfType<GameUIController>();
        pausePanel = gameUIController.pausePanel;
        winPopup = gameUIController.winPopup;
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    void Start()
    {
        int numberOfDisks = GameSettings.numberOfDisks;
        heightDiskBeforeMove += numberOfDisks / 2;
        InitializeTowers(numberOfDisks);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&
            !isLifting&&!isLowering&&!isMoving)
        {
            Vector3 mousePos = Input.mousePosition;
            if (!RectTransformUtility.RectangleContainsScreenPoint(pauseButtonRect, mousePos) &&
            !winPopup.activeSelf&&
            !pausePanel.activeSelf)
            {
                HandleMouseClick(mousePos);
            }
        }
        if (isMoving)
        {
            //isLifting = false;
            liftedDisk.position = Vector3.Lerp(liftedDisk.position, highTargetPosition, Time.deltaTime * changeTowerSpeed);

            if (Vector3.Distance(liftedDisk.position, highTargetPosition) < 0.01f)
            {
                liftedDisk.position = highTargetPosition;
                isMoving = false;
                isLowering = true;
            }
        }

        if (liftedDisk != null && (isLifting || isLowering))
        {
            liftedDisk.position = Vector3.Lerp(liftedDisk.position, targetPosition, Time.deltaTime * moveSpeed);

            if (Vector3.Distance(liftedDisk.position, targetPosition) < 0.01f)
            {
                liftedDisk.position = targetPosition;
                if (isLowering)
                {
                    PlayDropSound();
                }
                isLifting = false;
                isLowering = false;
            }

        }
    }

    void InitializeTowers(int numDisks)
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject tower in towers)
        {
            //hiệu  chỉnh độ cao và tâm của tower
            float posAndScaleY = (float)GameSettings.numberOfDisks / 2 + 1f;

            Vector3 position = tower.transform.position;
            position.y = posAndScaleY;
            tower.transform.position = position;

            Vector3 scale = tower.transform.localScale;
            scale.y = posAndScaleY;
            tower.transform.localScale = scale;
        }

        for (int i = numDisks; i > 0; i--)
        {
            float radius = minDiskRadius + (i - 1);
            GameObject disk = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            disk.transform.localScale = new Vector3(radius, diskHeight/2, radius);
            disk.name = "Disk" + i;
            disk.transform.position = new Vector3(towerAPosition.position.x, (numDisks - i) * diskHeight + diskHeight/2, towerAPosition.position.z);
            
            Renderer diskRenderer = disk.GetComponent<Renderer>();
            if (diskRenderer != null)
            {
                diskRenderer.material = donutMaterial;
            }
            towerA.Push(disk.transform);
        }
    }

    void HandleMouseClick(Vector3 mousePosition)
    {
        float screenWidth = Screen.width;
        float regionWidth = screenWidth / 3f;

        int towerClick = Mathf.CeilToInt(mousePosition.x / regionWidth);

        if (!isChooseTower)
        {
            Stack<Transform> clickedTower = GetTower(towerClick);
            if (clickedTower.Count != 0)
            {
                towerChoice = towerClick;
                LiftTopDisk(towerChoice);
                isChooseTower = !isChooseTower;
            }
        }
        else
        {
            if (towerClick == towerChoice)
            {
                LowerDisk();
            }
            else
            {
                MoveDisk(towerChoice, towerClick);
            }
            isChooseTower = !isChooseTower;
            
        }
    }

    void LiftTopDisk(int tower)
    {
        Stack<Transform> selectedTower = GetTower(tower);

        if (selectedTower.Count > 0)
        {
            liftedDisk = selectedTower.Peek();
            originalDiskPosition = liftedDisk.position; // Store the original position
            targetPosition = new Vector3(liftedDisk.position.x, heightDiskBeforeMove, liftedDisk.position.z);
            isLifting = true;
            isLowering = false;
            PlayLiftSound();
        }
    }

    void LowerDisk()
    {
        if (liftedDisk != null)
        {
            // Use the original position if lowering back to the same tower
            targetPosition = originalDiskPosition;
            isLifting = false;
            isLowering = true;
        }
    }

    void MoveDisk(int start, int end)
    {
        Stack<Transform> startTower = GetTower(start);
        Stack<Transform> endTower = GetTower(end);

        if (startTower.Count > 0)
        {
            Transform disk = startTower.Peek();
            float diskRadius = disk.localScale.x;
            bool isEmptyTargetTower = endTower.Count == 0 ? true: false;

            //Nếu tháp chuyển đến rỗng hoặc đĩa trên cùng có bán kính lớn hơn đĩa chuyển đến
            if(isEmptyTargetTower || diskRadius<endTower.Peek().localScale.x)
            {
                endTower.Push(startTower.Pop());
                highTargetPosition = new Vector3(GetTowerPosition(end).position.x, heightDiskBeforeMove, GetTowerPosition(end).position.z);
                targetPosition = new Vector3(GetTowerPosition(end).position.x, (endTower.Count - 1) * diskHeight + diskHeight / 2, GetTowerPosition(end).position.z);
                liftedDisk = disk;
                isMoving = true;
            }
            else
            {
                LowerDisk();
            }
            
        }
        if (CheckWin())
        {
            //Debug.Log("You win");
            gameUIController.pauseButton.interactable = false;
            gameUIController.ShowWinPopup();
            PlayWinSound();

        }
    }

    Stack<Transform> GetTower(int tower)
    {
        //switch (tower)
        //{
        //    case 1:
        //        return towerA;
        //    case 2:
        //        return towerB;
        //    case 3:
        //        return towerC;
        //    default:
        //        return null;
        //}
        return tower switch
        {
            1 => towerA,
            2 => towerB,
            3 => towerC,
            _ => null,
        };
    }

    Transform GetTowerPosition(int tower)
    {
        //switch (tower)
        //{
        //    case 1:
        //        return towerAPosition;
        //    case 2:
        //        return towerBPosition;
        //    case 3:
        //        return towerCPosition;
        //    default:
        //        return null;
        //}
        return tower switch
        {
            1 => towerAPosition,
            2 => towerBPosition,
            3 => towerCPosition,
            _ => null,
        };
    }

    bool CheckWin()
    {
        return towerC.Count == GameSettings.numberOfDisks;
    }

    void PlayLiftSound()
    {
        if (liftSound != null && audioSource != null)
        {
            //Debug.Log("Playing lift sound");
            audioSource.PlayOneShot(liftSound);
        }
        //else
        //{
        //    Debug.Log("Lift sound or audio source is null");
        //}
    }

    void PlayDropSound()
    {
        if (dropSound != null && audioSource != null)
        {
            //Debug.Log("Playing drop sound");
            audioSource.PlayOneShot(dropSound);
        }
        //else
        //{
        //    Debug.Log("Drop sound or audio source is null");
        //}
    }

    void PlayWinSound()
    {
        if(winSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }

}
