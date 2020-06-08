using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPresetConfig : PresetConfig
{

    void Awake()
    {
        SceneConfigs.Add(SC0());
        SceneConfigs.Add(SC1());
        SceneConfigs.Add(SC2());
        SceneConfigs.Add(SC3());
        SceneConfigs.Add(SC4());
        SceneConfigs.Add(SC5());
        SceneConfigs.Add(SC6());

    }

    SceneConfig SC0()
    {

        List<ObstacleConfig> obstacleConfigs = new List<ObstacleConfig>();

        obstacleConfigs.Add(new ObstacleConfig(ObstacleType.Cube, new Vector2(0f, 0f), new Vector2(1.2f, 1.2f), 0f));
        //obstacleConfigs.Add(new ObstacleConfig(ObstacleType.Cube, Vector2.zero, 2f * Vector2.one, 45f));
        //obstacleConfigs.Add(new ObstacleConfig(ObstacleType.Cube, new Vector2(0f, 4f), new Vector2(10f, 1f), 0f));
        //obstacleConfigs.Add(new ObstacleConfig(ObstacleType.Cube, new Vector2(1.3f, -3.1f), new Vector2(1.6f, 0.3f), 0f));

        Vector2[] pathNodePositions = {
            new Vector2(-2.5f, 0f),
            new Vector2(2.5f, 0f),
            new Vector2(0f, -2.5f),
            new Vector2(0f, 2.5f),
        };

        return new SceneConfig(new Vector2(1f, 1f), new Vector2(-4f, -4f), 1f, obstacleConfigs.ToArray(), pathNodePositions, 1f, 0, 670238407);

    }

    SceneConfig SC1()
    {
        ObstacleConfig[] obstacleConfigs =
        {
            new ObstacleConfig(ObstacleType.Cube, new Vector2(0.5f, 0f), new Vector2(4f, 1f), 60f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(2.5f, 2.5f), new Vector2(1.5f, 1.5f), 90f),
            new ObstacleConfig(ObstacleType.SoftStar, new Vector2(-2.4f, -3.1f), new Vector2(0.9f, 0.9f), 40f),
        };

        Vector2[] pathNodePositions = { };

        return new SceneConfig(new Vector2(1f, 1f), new Vector2(-2f, 2f), 0.5f, obstacleConfigs, pathNodePositions, 0.5f, 10, 670238407);

    }


    SceneConfig SC2()
    {
        ObstacleConfig[] obstacleConfigs =
        {
            new ObstacleConfig(ObstacleType.Cube, new Vector2(0.5f, 0f), new Vector2(2f, 1f), 20f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(2.35f, -1.17f), new Vector2(1.5f, 1.5f), 90f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(2.35f, 3.17f), new Vector2(2.2f, 2.6f), 90f),
            new ObstacleConfig(ObstacleType.SoftStar, new Vector2(-2.4f, -2f), new Vector2(1f, 1f), 0f),
        };

        Vector2[] pathNodePositions = { };

        return new SceneConfig(new Vector2(2f, 1f), new Vector2(-2f, 2f), 0.5f, obstacleConfigs, pathNodePositions, 1f, 15, 1561408197);
    }


    SceneConfig SC3()
    {
        ObstacleConfig[] obstacleConfigs =
        {
            new ObstacleConfig(ObstacleType.Cube, new Vector2(0.1f, 4.1f), new Vector2(14f, 1f), 0f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(0.1f, 2.1f), new Vector2(14f, 0.1f), 0f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(4.5f, -4.5f), new Vector2(1.5f, 1.5f), 90f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(-4.5f, -1.5f), new Vector2(2.5f, 1.5f), 90f),
            new ObstacleConfig(ObstacleType.SoftStar, new Vector2(-4.4f, -3.6f), new Vector2(1f, 1f), 30f),
        };

        Vector2[] pathNodePositions =
            {
                new Vector2(0f, 3.1f),
                new Vector2(8f, 3.1f)
            };

        return new SceneConfig(new Vector2(2f, 1f), new Vector2(-6f, 3f), 1f, obstacleConfigs, pathNodePositions, 0.25f, 15, 1827320275);
    }



    SceneConfig SC4()
    {
        ObstacleConfig[] obstacleConfigs =
        {
            new ObstacleConfig(ObstacleType.Cube, new Vector2(-5f, -3f), new Vector2(6f, 1f), 45f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(-1.25f, 1.7f), new Vector2(6f, 1f), -45f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(1.25f, -2.7f), new Vector2(4f, 4f), 0f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(7.73f, -3.52f), new Vector2(6f, 1f), -45f),
            new ObstacleConfig(ObstacleType.SoftStar, new Vector2(7.4f, 3.9f), new Vector2(1f, 1f), 30f),
            new ObstacleConfig(ObstacleType.SoftStar, new Vector2(-8.4f, 1.9f), new Vector2(1f, 1f), 0f),
        };

        Vector2[] pathNodePositions =
            {
                new Vector2(0f, 0f),
                new Vector2(-9.5f, 0.5f),
                new Vector2(0f, -5.5f),
                new Vector2(1f, -3f),
                new Vector2(-2f, -4f),
                new Vector2(3.4f, -5.5f),
                new Vector2(0f, 4.7f),
            };


        return new SceneConfig(new Vector2(2f, 1f), new Vector2(-8f, -3.5f), 0.5f, obstacleConfigs, pathNodePositions, 0.75f, 15, 309667876);
    }



    SceneConfig SC5()
    {
        ObstacleConfig[] obstacleConfigs =
        {
            new ObstacleConfig(ObstacleType.Cube, new Vector2(0.5f, 0.5f), new Vector2(1f, 1f), 0f),
            new ObstacleConfig(ObstacleType.Cube, new Vector2(2.5f, 2.5f), new Vector2(1f, 1f), 0f),

        };

        Vector2[] pathNodePositions = { };

        return new SceneConfig(new Vector2(1f, 1f), new Vector2(-2f, 2f), 0.5f, obstacleConfigs, pathNodePositions, 1f, 10, 670238407);

    }


    SceneConfig SC6()
    {

        List<ObstacleConfig> obstacleConfigs = new List<ObstacleConfig>();

        obstacleConfigs.Add(new ObstacleConfig(ObstacleType.Cube, Vector2.zero, 2f * Vector2.one, 45f));
        obstacleConfigs.Add(new ObstacleConfig(ObstacleType.Cube, new Vector2(0f, 4f), new Vector2(10f, 1f), 0f));
        obstacleConfigs.Add(new ObstacleConfig(ObstacleType.Cube, new Vector2(1.3f, -3.1f), new Vector2(1.6f, 0.3f), 0f));

        Vector2[] pathNodePositions = {
            new Vector2(8f, 0f),
            new Vector2(4.9f, 0f),
            new Vector2(-1.65f, -4f),
            new Vector2(-1.65f, 2.7f),
            new Vector2(3f, -4f),
            new Vector2(3f, 3.2f),
            new Vector2(0f, -2.2f),
            new Vector2(-4f, 0f),
        };

        return new SceneConfig(new Vector2(1f, 1f), new Vector2(-4f, -4f), 1f, obstacleConfigs.ToArray(), pathNodePositions, 10f, 0, 670238407);

    }





}
