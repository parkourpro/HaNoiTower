using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionController : MonoBehaviour
{
    private Stack<Transform> towerA = new Stack<Transform>();
    private Stack<Transform> towerB = new Stack<Transform>();
    private Stack<Transform> towerC = new Stack<Transform>();

    public Transform towerAPosition;
    public Transform towerBPosition;
    public Transform towerCPosition;

    public float diskHeight = 1f;
    public float minDiskRadius = 2f;
    public float lerpDuration = 0.3f;
    private float heightDiskBeforeMove;
    private float heightDiskBeforeMoveOrigin = 6f;

    private int minDiskCount = GameSettings.minDiskCount;
    private int maxDiskCount = GameSettings.maxDiskCount;

    private int numberOfDisks = InstructionUIController.diskCount;

    private Vector3 initialTowerAPosition;
    private Vector3 initialTowerCPosition;
    private Vector3 initialCameraPosition;

    private InstructionUIController instructionUIController;

    private bool isInitalizingTowers = false;
    private bool isSolving = false;
    private bool isMovingDisk = false;


    private Queue<(int, int)> sequenceStep = SolveHaNoiTower.sequenceStep;


    public Material donutMaterial;
    private void Start()
    {
        //vị trí cột khi chưa thay đổi số lượng đĩa
        initialTowerAPosition = towerAPosition.position;
        initialTowerCPosition = towerCPosition.position;
        initialCameraPosition = Camera.main.transform.position;

        heightDiskBeforeMove = heightDiskBeforeMoveOrigin +  numberOfDisks / 2;

        instructionUIController = FindObjectOfType<InstructionUIController>();

        InitializeTowers(numberOfDisks);

    }


    private void Update()
    {
        //nếu số lượng đĩa bị thay đổi
        if (numberOfDisks != InstructionUIController.diskCount)
        {
            numberOfDisks = InstructionUIController.diskCount;
            heightDiskBeforeMove = heightDiskBeforeMoveOrigin + numberOfDisks / 2;
            isInitalizingTowers = true;
        }
        //thì khởi tạo lại tháp
        if (isInitalizingTowers)
        {
            InitializeTowers(numberOfDisks);
            isInitalizingTowers = false;
        }
        //lấy trạng thái giải đĩa dựa trên class UI, trạng thái được điều khiển bởi nút bấm
        isSolving = InstructionUIController.isSolving;
        // vì Coroutine là hàm bất đồng bộ nên cần có isMovingDisk để chắc chắn rằng khi đĩa di chuyển xong một
        // step thì mới bắt đầu step tiếp theo, nếu không đĩa khác sẽ bắt đầu di chuyển ở khung hình tiếp theo
        if (isSolving && !isMovingDisk)
        {
            //ban đầu chạy trò chơi thì class SolveHaNoiTower và class này chạy cùng lúc nên lúc đầu
            //lời giải rỗng, sau 1 khung hình thì mới xuất hiện lời giải, nên cần kiểm tra dương
            if(sequenceStep.Count > 0)
            {
                var step = sequenceStep.Dequeue();
                int start = step.Item1;
                int end = step.Item2;
                StartCoroutine(MoveDisk(start, end));
            }
            else
            {
                // Khi giải xong tháp, đặt về trạng thái nút resume
               instructionUIController.OnPauseButtonClick();
            }
        }

    }
    //hàm khởi tạo tháp
    void InitializeTowers(int numDisks)
    {
        ClearTower(towerA);
        ClearTower(towerB);
        ClearTower(towerC);
        instructionUIController.SetButtonsInteractable(false);
        instructionUIController.SetResumeButtonInteractable(false);
        ClearTower(towerA); //xoá towerA trước
        GameObject[] towers = GameObject.FindGameObjectsWithTag("TowerInstruction");
        //sự thay đổi của vị trí cột và camera
        float posAndScaleY = (float)numberOfDisks / 2f + 1f;
        //thay đổi vị trí 2 cột 2 bên
        Vector3 newTowerAPosition = new Vector3(initialTowerAPosition.x - posAndScaleY / 2, initialTowerAPosition.y, initialTowerAPosition.z);
        Vector3 newTowerCPosition = new Vector3(initialTowerCPosition.x + posAndScaleY / 2, initialTowerCPosition.y, initialTowerCPosition.z);
        towerAPosition.transform.position = newTowerAPosition;
        towerCPosition.transform.position = newTowerCPosition;
        //thay đổi vị trí camera
        Vector3 newCameraPosition = new Vector3(initialCameraPosition.x, initialCameraPosition.y + posAndScaleY, initialCameraPosition.z - posAndScaleY);
        Camera.main.transform.position = newCameraPosition;
        //thay đổi chiều cao cột
        foreach (GameObject tower in towers)
        {
            Vector3 position = tower.transform.position;
            position.y = posAndScaleY;
            tower.transform.position = position;

            Vector3 scale = tower.transform.localScale;
            scale.y = posAndScaleY;
            tower.transform.localScale = scale;
        }
        //tạo và di chuyển đĩa
        StartCoroutine(CreateAndMoveDisks(numDisks));

    }

    IEnumerator CreateAndMoveDisks(int numDisks)
    {
        for (int i = numDisks; i > 0; i--)
        {
            float radius = minDiskRadius + (i - 1);
            GameObject disk = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            disk.transform.localScale = new Vector3(radius, diskHeight / 2, radius);
            disk.name = "Disk" + i;
            disk.transform.position = new Vector3(towerAPosition.position.x, 9, towerAPosition.position.z);
            Renderer diskRenderer = disk.GetComponent<Renderer>();
            if (diskRenderer != null)
            {
                diskRenderer.material = donutMaterial;
            }
            towerA.Push(disk.transform);

            Vector3 targetPosition = new Vector3(towerAPosition.position.x, (numDisks - i) * diskHeight + diskHeight / 2, towerAPosition.position.z);
            float newDuration = (float)lerpDuration - (numberOfDisks - minDiskCount) * lerpDuration / (2 * (maxDiskCount - minDiskCount));
            yield return StartCoroutine(LerpPosition(disk.transform, targetPosition, newDuration));
        }
        instructionUIController.SetButtonsInteractable(true);
        instructionUIController.SetResumeButtonInteractable(true);
    }


    IEnumerator MoveDisk(int start, int end)
    {
        isMovingDisk = true;

        Stack<Transform> startTower = GetTower(start);
        Stack<Transform> endTower = GetTower(end);

        Transform moveDisk = startTower.Pop();
        Vector3 firstDestination = new Vector3(moveDisk.position.x, heightDiskBeforeMove, moveDisk.position.z);
        Vector3 secondDestination = new Vector3(GetTowerPosition(end).position.x, heightDiskBeforeMove, GetTowerPosition(end).position.z);
        Vector3 destination = new Vector3(GetTowerPosition(end).position.x, endTower.Count * diskHeight + diskHeight / 2, GetTowerPosition(end).position.z);

        float duration = lerpDuration;
        yield return StartCoroutine(LerpPosition(moveDisk, firstDestination, duration));
        yield return StartCoroutine(LerpPosition(moveDisk, secondDestination, duration));
        yield return StartCoroutine(LerpPosition(moveDisk, destination, duration));

        endTower.Push(moveDisk);
        isMovingDisk = false;
        yield return null;
    }

    IEnumerator LerpPosition(Transform obj, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = obj.position;
        float time = 0;

        while (time < duration)
        {
            obj.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        obj.position = targetPosition;
    }


    Stack<Transform> GetTower(int tower)
    {
        switch (tower)
        {
            case 1:
                return towerA;
            case 2:
                return towerB;
            case 3:
                return towerC;
            default:
                return null;
        }
    }

    Transform GetTowerPosition(int tower)
    {
        switch (tower)
        {
            case 1:
                return towerAPosition;
            case 2:
                return towerBPosition;
            case 3:
                return towerCPosition;
            default:
                return null;
        }
    }

    void ClearTower(Stack<Transform> tower)
    {
        while (tower.Count > 0)
        {
            Transform disk = tower.Pop();
            Destroy(disk.gameObject);
        }
    }
    bool CheckWin()
    {
        return towerC.Count == numberOfDisks;
    }
}
