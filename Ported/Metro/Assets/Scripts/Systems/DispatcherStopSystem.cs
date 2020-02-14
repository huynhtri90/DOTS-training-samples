﻿using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class DispatcherStopSystem : JobComponentSystem
{
    public BitArray m_PathStopBits;
    public NativeArray<int2> m_PathIndices;

    public const float TIME_TO_WAIT = 5;

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var pathData = m_PathStopBits;
        var pathIndices = m_PathIndices;
        var dt = Time.DeltaTime;

        var outputDeps = Entities.ForEach((ref DispatcherStopComponent stopComponent, ref MovementDerivatives movement, in PathMoverComponent pathMoverComponent) =>
        {
            int currentPoint = pathMoverComponent.CurrentPointIndex;

            if (stopComponent.LeaveTime <= 0 && currentPoint != stopComponent.LastPoint)
            {
                int2 range = pathIndices[pathMoverComponent.m_TrackIndex];

                bool atStop = pathData.IsBitSet(currentPoint + range.x);

                if (atStop)
                {
                    stopComponent.LeaveTime = TIME_TO_WAIT;
                    stopComponent.LastPoint = currentPoint;
                    movement.Acceleration = 0;
                }
            }
            else
            {
                stopComponent.LeaveTime -= dt;

                if(stopComponent.LeaveTime <= 0.1f)
                {
                    movement.Acceleration = 20;
                }
            }
        }).Schedule(inputDependencies);

        return outputDeps;
    }
}