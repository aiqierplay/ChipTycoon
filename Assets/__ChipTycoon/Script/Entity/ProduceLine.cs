using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProduceLine : EntityBase
{
    [Title("Input")]
    public float MoveSpeed;
    public Transform InputStart;
    public Transform InputEnd;
    public EntityBase InputPrefab;
    public float InputInterval;
    public int InputNum;
    public int InputMax;
    public UVScroller InputLineUv;
    [Title("Output")]
    public Transform OutputStart;
    public Transform OutputEnd;
    public EntityBase OutputPrefab;
    public float OutputInterval;
    public int OutputNum;
    public int OutputMax;
    public UVScroller OutputLineUv;
    [Title("Work")] 
    public float WorkDuration;
    public UTweenPlayer TweenWork;

    [NonSerialized] public List<EntityBase> InputList = new List<EntityBase>();
    [NonSerialized] public int InputCount;
    [NonSerialized] public List<EntityBase> OutputList = new List<EntityBase>();
    [NonSerialized] public int OutputCount;
    [NonSerialized] public Action<EntityBase> OnOutput = delegate { };

    [NonSerialized] public bool IsWorking;
    [NonSerialized] public bool IsWorkComplete;

    public void Init()
    {
        InputList.Clear();
        OutputList.Clear();
        InputCount = 0;
        OutputCount = 0;
        IsWorking = false;
        IsWorkComplete = false;
        WorkStop();
        InputLineStop();
        OutputLineStop();
        StartCoroutine(WorkCo());
    }

    public bool CheckCanInput()
    {
        return InputList.Count < InputMax;
    }

    public void AddInput()
    {
        var input = GamePool.Spawn(InputPrefab, Trans);
        input.Position = InputStart.position;
        InputList.Add(input);
    }

    public float GetInputProgress(Vector3 pos)
    {
       return (pos - InputStart.position).magnitude / (InputEnd.position - InputStart.position).magnitude;
    }

    public float GetOutProgress(Vector3 pos)
    {
        return (pos - OutputStart.position).magnitude / (OutputEnd.position - OutputStart.position).magnitude;
    }

    public void WorkStart()
    {
        if (TweenWork != null) TweenWork.Play();
    }

    public void WorkStop()
    {
        if (TweenWork != null) TweenWork.Stop();
    }

    public void InputLineStart()
    {
        if (InputLineUv != null) InputLineUv.enabled = true;
    }

    public void InputLineStop()
    {
        if (InputLineUv != null) InputLineUv.enabled = false;
    }

    public void OutputLineStart()
    {
        if (OutputLineUv != null) OutputLineUv.enabled = true;
    }

    public void OutputLineStop()
    {
        if (OutputLineUv != null) OutputLineUv.enabled = false;
    }


    public IEnumerator WorkCo()
    {
        var workTimer = 0f;
        var outputTimer = 0f;
        while (true)
        {
            // Input Move
            if (InputList.Count > 0)
            {
                var inputFirst = InputList.First();
                var inputFirstProgress = GetInputProgress(inputFirst.Position);
                if (inputFirstProgress < 1f)
                {
                    var inputDirection = (InputEnd.position - InputStart.position).normalized;
                    foreach (var input in InputList)
                    {
                        input.Position += inputDirection * MoveSpeed * DeltaTime;
                    }

                    InputLineStart();
                }
                else
                {
                    InputLineStop();
                }

                inputFirstProgress = GetInputProgress(inputFirst.Position);
                if (inputFirstProgress >= 1f && InputCount < InputNum)
                {
                    InputList.Remove(inputFirst);
                    GamePool.DeSpawn(inputFirst);
                    InputCount++;
                }
            }
            else
            {
                InputLineStop();
            }

            // Work
            var checkCanWork = InputCount >= InputNum && OutputCount < OutputNum;
            if (checkCanWork && !IsWorking)
            {
                IsWorking = true;
                IsWorkComplete = false;
                WorkStart();
            }

            if (!checkCanWork && IsWorking)
            {
                IsWorking = false;
                WorkStop();
            }

            if (IsWorking)
            {
                // Wait Work
                if (!IsWorkComplete && workTimer < WorkDuration)
                {
                    workTimer += DeltaTime;
                    if (workTimer >= WorkDuration)
                    {
                        workTimer = WorkDuration;
                        IsWorkComplete = true;
                    }
                }

                if (IsWorkComplete)
                {
                    outputTimer += DeltaTime;
                    if (outputTimer >= OutputInterval && OutputList.Count < OutputMax)
                    {
                        var output = GamePool.Spawn(OutputPrefab, Trans);
                        output.Position = OutputStart.position;
                        OutputList.Add(output);
                        OutputCount++;
                        outputTimer = 0f;

                        // Work Complete
                        if (OutputCount >= OutputNum)
                        {
                            InputCount = 0;
                            OutputCount = 0;
                            workTimer = 0f;
                            outputTimer = 0f;
                            IsWorkComplete = false;
                        }
                    }
                }
            }

            // Output
            if (OutputList.Count > 0)
            {
                var outputDirection = (OutputEnd.position - OutputStart.position).normalized;
                foreach (var output in OutputList)
                {
                    output.Position += outputDirection * MoveSpeed * DeltaTime;
                }

                var outputFirst = OutputList.First();
                var outputFirstProgress = GetOutProgress(outputFirst.Position);
                if (outputFirstProgress >= 1f)
                {
                    OutputList.Remove(outputFirst);
                    OnOutput?.Invoke(outputFirst);
                }

                OutputLineStart();
            }
            else
            {
                OutputLineStop();
            }

            yield return null;
        }
    }
}
