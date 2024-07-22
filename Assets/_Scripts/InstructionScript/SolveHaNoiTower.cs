using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveHaNoiTower : MonoBehaviour
{ 
    public static Queue<(int, int)> sequenceStep = new Queue<(int, int)>();

    private int numberOfDisks = InstructionUIController.diskCount;

    void Start()
    {
        solveHanoiTower(numberOfDisks, 1, 2, 3);
    }

    void Update()
    {
        if (numberOfDisks != InstructionUIController.diskCount)
        {
            numberOfDisks = InstructionUIController.diskCount;
            sequenceStep.Clear();
            solveHanoiTower(numberOfDisks, 1, 2, 3);
        }
    }

    private void solveHanoiTower(int n, int start, int middle, int end)
    {
        if (n == 1)
        {
            //Debug.Log($"Move disk from {start} to {end}");
            sequenceStep.Enqueue((start, end));
        }
        else
        {
            solveHanoiTower(n - 1, start, end, middle);
            solveHanoiTower(1, start, middle, end);
            solveHanoiTower(n - 1, middle, start, end);
        }

    }
}
