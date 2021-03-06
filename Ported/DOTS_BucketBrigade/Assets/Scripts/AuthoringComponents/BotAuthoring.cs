﻿using Unity.Entities;
using UnityEngine;

public class BotAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponent<BotTag>(entity);
        dstManager.AddComponent<Position2D>(entity);
        dstManager.AddComponent<Role>(entity);
    }
}
