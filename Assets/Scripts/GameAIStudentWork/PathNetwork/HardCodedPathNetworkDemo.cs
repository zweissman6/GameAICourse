using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace GameAICourse
{

    // To use this, make a new CreatePathNetwork.cs implementation of Create() that simply calls:
    // HardCodedPathNetworkDemo.Create(canvasOrigin, canvasWidth, canvasHeight, obstacles, agentRadius,
    //                                   pathNodes, out pathEdges);
    // and then returns

    public class HardCodedPathNetworkDemo
    {

        public static void Create(Vector2 canvasOrigin, float canvasWidth, float canvasHeight,
List<Obstacle> obstacles, float agentRadius, List<Vector2> pathNodes, out List<List<int>> pathEdges)
        {

            var pnc = new PathNetworkConfig(canvasOrigin, canvasWidth, canvasHeight, obstacles, agentRadius,
                pathNodes);

            var pb0 = new PathNetworkConfig(
            new Vector2(-5f, -5f),
            10f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(1.933013f, -1.482051f), new Vector2(-0.06698728f, 1.982051f), new Vector2(-0.9330127f, 1.482051f), new Vector2(1.066987f, -1.982051f), },
new Vector2[] {new Vector2(3.25f, 1.75f), new Vector2(3.25f, 3.25f), new Vector2(1.75f, 3.25f), new Vector2(1.75f, 1.75f), },
new Vector2[] {new Vector2(-1.79915f, -2.957997f), new Vector2(-1.624798f, -2.17615f), new Vector2(-2.364491f, -2.483619f), new Vector2(-2.986811f, -2.161157f), new Vector2(-3.00988f, -2.84672f), new Vector2(-3.522057f, -3.345097f), new Vector2(-2.822312f, -3.603291f), new Vector2(-2.446531f, -4.247571f), new Vector2(-2.044664f, -3.656633f), new Vector2(-1.373522f, -3.514868f), },
            },
            0.25f,
            new Vector2[] { new Vector2(-4.5f, -4.5f), new Vector2(4.5f, 4.5f), new Vector2(4.5f, -4.5f), new Vector2(-4.5f, 4.5f), new Vector2(-0.3966575f, 4.396721f), new Vector2(1.522794f, -0.1116495f), new Vector2(-3.532882f, 4.456426f), new Vector2(1.413421f, 0.2512116f), new Vector2(-2.170723f, 0.3285379f), new Vector2(-4.279467f, 0.4430504f), new Vector2(0.2952785f, 2.107143f), new Vector2(-1.385468f, 1.422808f), new Vector2(3.560646f, 3.183976f), new Vector2(1.346828f, -3.827864f), },
            new int[][] {new int[] {2, 3, 4, 6, 8, 9, 11, },
new int[] {2, 3, 4, 6, 12, },
new int[] {0, 1, 4, 5, 7, 9, 10, 12, 13, },
new int[] {0, 1, 4, 6, 8, 9, 10, 11, 13, },
new int[] {0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, },
new int[] {2, 4, 7, 10, },
new int[] {0, 1, 3, 4, 8, 9, 10, 11, 13, },
new int[] {2, 4, 5, 10, },
new int[] {0, 3, 4, 6, 9, 11, 13, },
new int[] {0, 2, 3, 4, 6, 8, 11, 13, },
new int[] {2, 3, 4, 5, 6, 7, },
new int[] {0, 3, 4, 6, 8, 9, 13, },
new int[] {1, 2, },
new int[] {2, 3, 6, 8, 9, 11, },
            });



            var pb1 = new PathNetworkConfig(
            new Vector2(-5f, -5f),
            10f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(1.933013f, -1.482051f), new Vector2(-0.06698728f, 1.982051f), new Vector2(-0.9330127f, 1.482051f), new Vector2(1.066987f, -1.982051f), },
new Vector2[] {new Vector2(3.25f, 1.75f), new Vector2(3.25f, 3.25f), new Vector2(1.75f, 3.25f), new Vector2(1.75f, 1.75f), },
new Vector2[] {new Vector2(-1.79915f, -2.957997f), new Vector2(-1.624798f, -2.17615f), new Vector2(-2.364491f, -2.483619f), new Vector2(-2.986811f, -2.161157f), new Vector2(-3.00988f, -2.84672f), new Vector2(-3.522057f, -3.345097f), new Vector2(-2.822312f, -3.603291f), new Vector2(-2.446531f, -4.247571f), new Vector2(-2.044664f, -3.656633f), new Vector2(-1.373522f, -3.514868f), },
            },
            0.25f,
            new Vector2[] { new Vector2(-4.5f, -4.5f), new Vector2(4.5f, 4.5f), new Vector2(4.5f, -4.5f), new Vector2(-4.5f, 4.5f), new Vector2(-0.3966575f, 4.396721f), new Vector2(1.522794f, -0.1116495f), new Vector2(-3.532882f, 4.456426f), new Vector2(1.413421f, 0.2512116f), new Vector2(-2.170723f, 0.3285379f), new Vector2(-4.279467f, 0.4430504f), new Vector2(0.2952785f, 2.107143f), new Vector2(-1.385468f, 1.422808f), new Vector2(3.560646f, 3.183976f), new Vector2(1.346828f, -3.827864f), },
            new int[][] {new int[] {2, 3, 4, 6, 8, 9, 11, },
new int[] {2, 3, 4, 6, 12, },
new int[] {0, 1, 4, 5, 7, 9, 10, 12, 13, },
new int[] {0, 1, 4, 6, 8, 9, 10, 11, 13, },
new int[] {0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, },
new int[] {2, 4, 7, 10, },
new int[] {0, 1, 3, 4, 8, 9, 10, 11, 13, },
new int[] {2, 4, 5, 10, },
new int[] {0, 3, 4, 6, 9, 11, 13, },
new int[] {0, 2, 3, 4, 6, 8, 11, 13, },
new int[] {2, 3, 4, 5, 6, 7, },
new int[] {0, 3, 4, 6, 8, 9, 13, },
new int[] {1, 2, },
new int[] {2, 3, 6, 8, 9, 11, },
            });

            var pb2 = new PathNetworkConfig(
            new Vector2(-5f, -5f),
            10f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(1.933013f, -1.482051f), new Vector2(-0.06698728f, 1.982051f), new Vector2(-0.9330127f, 1.482051f), new Vector2(1.066987f, -1.982051f), },
new Vector2[] {new Vector2(3.25f, 1.75f), new Vector2(3.25f, 3.25f), new Vector2(1.75f, 3.25f), new Vector2(1.75f, 1.75f), },
new Vector2[] {new Vector2(-1.79915f, -2.957997f), new Vector2(-1.624798f, -2.17615f), new Vector2(-2.364491f, -2.483619f), new Vector2(-2.986811f, -2.161157f), new Vector2(-3.00988f, -2.84672f), new Vector2(-3.522057f, -3.345097f), new Vector2(-2.822312f, -3.603291f), new Vector2(-2.446531f, -4.247571f), new Vector2(-2.044664f, -3.656633f), new Vector2(-1.373522f, -3.514868f), },
            },
            0.25f,
            new Vector2[] { new Vector2(-4.5f, -4.5f), new Vector2(4.5f, 4.5f), new Vector2(4.5f, -4.5f), new Vector2(-4.5f, 4.5f), new Vector2(-0.3966575f, 4.396721f), new Vector2(1.522794f, -0.1116495f), new Vector2(-3.532882f, 4.456426f), new Vector2(1.413421f, 0.2512116f), new Vector2(-2.170723f, 0.3285379f), new Vector2(-4.279467f, 0.4430504f), new Vector2(0.2952785f, 2.107143f), new Vector2(-1.385468f, 1.422808f), new Vector2(3.560646f, 3.183976f), new Vector2(1.346828f, -3.827864f), },
            new int[][] {new int[] {2, 3, 4, 6, 8, 9, 11, },
new int[] {2, 3, 4, 6, 12, },
new int[] {0, 1, 4, 5, 7, 9, 10, 12, 13, },
new int[] {0, 1, 4, 6, 8, 9, 10, 11, 13, },
new int[] {0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, },
new int[] {2, 4, 7, 10, },
new int[] {0, 1, 3, 4, 8, 9, 10, 11, 13, },
new int[] {2, 4, 5, 10, },
new int[] {0, 3, 4, 6, 9, 11, 13, },
new int[] {0, 2, 3, 4, 6, 8, 11, 13, },
new int[] {2, 3, 4, 5, 6, 7, },
new int[] {0, 3, 4, 6, 8, 9, 13, },
new int[] {1, 2, },
new int[] {2, 3, 6, 8, 9, 11, },
            });

            var pb3 = new PathNetworkConfig(
            new Vector2(-10f, -5f),
            20f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(1.610703f, 0.1278261f), new Vector2(-0.2686825f, 0.8118665f), new Vector2(-0.6107028f, -0.1278261f), new Vector2(1.268682f, -0.8118665f), },
new Vector2[] {new Vector2(3.1f, -1.92f), new Vector2(3.1f, -0.42f), new Vector2(1.6f, -0.4199997f), new Vector2(1.6f, -1.92f), },
new Vector2[] {new Vector2(3.65f, 2.07f), new Vector2(3.65f, 4.27f), new Vector2(1.05f, 4.27f), new Vector2(1.05f, 2.07f), },
new Vector2[] {new Vector2(-1.99f, -1.45f), new Vector2(-2.4f, -0.66f), new Vector2(-2.81f, -1.45f), new Vector2(-3.57f, -1.62f), new Vector2(-3.1f, -2.22f), new Vector2(-3.18f, -3.01f), new Vector2(-2.4f, -2.73f), new Vector2(-1.62f, -3.01f), new Vector2(-1.7f, -2.22f), new Vector2(-1.23f, -1.62f), },
            },
            0.25f,
            new Vector2[] { new Vector2(-9.5f, -4.5f), new Vector2(9.5f, 4.5f), new Vector2(9.5f, -4.5f), new Vector2(-9.5f, 4.5f), new Vector2(5.893731f, -1.38094f), new Vector2(4.50082f, 2.997828f), new Vector2(-2.759432f, 0.6798368f), new Vector2(7.912167f, 1.652403f), new Vector2(-7.896657f, 1.569552f), new Vector2(6.569023f, -2.545953f), new Vector2(-1.671981f, -4.266428f), new Vector2(0.02486801f, 2.34305f), new Vector2(-9.619093f, -1.62226f), new Vector2(-2.98452f, -0.5380411f), new Vector2(7.156771f, -4.581751f), new Vector2(-8.99165f, 0.9938579f), new Vector2(-3.617111f, 4.685837f), new Vector2(3.828616f, -3.21731f), new Vector2(7.996771f, 0.5469413f), },
            new int[][] {new int[] {2, 3, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, },
new int[] {2, 4, 5, 7, 9, 14, 16, 17, 18, },
new int[] {0, 1, 4, 5, 7, 9, 10, 11, 14, 16, 17, 18, },
new int[] {0, 6, 8, 11, 12, 13, 15, 16, },
new int[] {1, 2, 5, 7, 9, 10, 11, 14, 16, 17, 18, },
new int[] {1, 2, 4, 7, 9, 14, 17, 18, },
new int[] {0, 3, 8, 11, 12, 13, 15, 16, },
new int[] {1, 2, 4, 5, 8, 9, 14, 15, 17, 18, },
new int[] {0, 3, 6, 7, 11, 12, 13, 15, 16, 18, },
new int[] {0, 1, 2, 4, 5, 7, 10, 11, 14, 16, 17, 18, },
new int[] {0, 2, 4, 9, 12, 14, 17, },
new int[] {0, 2, 3, 4, 6, 8, 9, 12, 13, 15, 16, },
new int[] {0, 3, 6, 8, 10, 11, 13, 15, 16, },
new int[] {0, 3, 6, 8, 11, 12, 15, 16, },
new int[] {0, 1, 2, 4, 5, 7, 9, 10, 17, 18, },
new int[] {0, 3, 6, 7, 8, 11, 12, 13, 16, },
new int[] {0, 1, 2, 3, 4, 6, 8, 9, 11, 12, 13, 15, },
new int[] {0, 1, 2, 4, 5, 7, 9, 10, 14, 18, },
new int[] {1, 2, 4, 5, 7, 8, 9, 14, 17, },
            });

            var pb4 = new PathNetworkConfig(
            new Vector2(-10f, -5f),
            20f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(7.1f, 4.6f), new Vector2(-6.9f, 4.6f), new Vector2(-6.9f, 3.6f), new Vector2(7.1f, 3.6f), },
new Vector2[] {new Vector2(7.1f, 2.15f), new Vector2(-6.9f, 2.15f), new Vector2(-6.9f, 2.05f), new Vector2(7.1f, 2.05f), },
new Vector2[] {new Vector2(5.25f, -5.25f), new Vector2(5.25f, -3.75f), new Vector2(3.75f, -3.75f), new Vector2(3.75f, -5.25f), },
new Vector2[] {new Vector2(-3.75f, -2.75f), new Vector2(-3.75f, -0.2500001f), new Vector2(-5.25f, -0.2499998f), new Vector2(-5.25f, -2.75f), },
new Vector2[] {new Vector2(-3.76993f, -3.328686f), new Vector2(-3.73f, -2.439526f), new Vector2(-4.480071f, -2.918686f), new Vector2(-5.223249f, -2.68591f), new Vector2(-5.116218f, -3.440526f), new Vector2(-5.5805f, -4.084685f), new Vector2(-4.765f, -4.232199f), new Vector2(-4.2295f, -4.864686f), new Vector2(-3.903782f, -4.140525f), new Vector2(-3.19675f, -3.85591f), },
            },
            0.5f,
            new Vector2[] { new Vector2(0f, 3.1f), new Vector2(8f, 3.1f), new Vector2(-9f, -4f), new Vector2(9f, 4f), new Vector2(9f, -4f), new Vector2(-9f, 4f), new Vector2(-8.645997f, 2.471271f), new Vector2(-8.139081f, -1.076054f), new Vector2(9.445614f, 0.8404679f), new Vector2(8.943146f, -2.898737f), new Vector2(3.178513f, -4.479975f), new Vector2(3.492153f, -3.075505f), new Vector2(-3.020164f, 0.1221399f), new Vector2(-7.186646f, -1.813538f), new Vector2(-7.799368f, -3.436613f), new Vector2(-2.86811f, -1.052815f), new Vector2(5.864737f, -0.5011826f), new Vector2(-3.169502f, 1.500627f), new Vector2(7.70927f, 1.100941f), new Vector2(-6.34202f, -1.816667f), new Vector2(-0.5530987f, -0.6429667f), },
            new int[][] {new int[] {1, },
new int[] {0, 3, 4, 8, 9, 18, },
new int[] {5, 6, 7, 13, 14, 19, },
new int[] {1, 4, 8, 9, 18, },
new int[] {1, 3, 8, 9, 12, 15, 16, 17, 18, 20, },
new int[] {2, 6, 7, 13, 14, 19, },
new int[] {2, 5, 7, 13, 14, 19, },
new int[] {2, 5, 6, 13, 14, 17, 19, },
new int[] {1, 3, 4, 9, 11, 12, 15, 16, 17, 18, 20, },
new int[] {1, 3, 4, 8, 11, 12, 15, 16, 17, 18, 20, },
new int[] {12, 15, 17, 20, },
new int[] {8, 9, 12, 15, 16, 17, 18, 20, },
new int[] {4, 8, 9, 10, 11, 15, 16, 17, 18, 20, },
new int[] {2, 5, 6, 7, 14, 19, },
new int[] {2, 5, 6, 7, 13, 19, },
new int[] {4, 8, 9, 10, 11, 12, 16, 17, 18, 20, },
new int[] {4, 8, 9, 11, 12, 15, 17, 18, 20, },
new int[] {4, 7, 8, 9, 10, 11, 12, 15, 16, 18, 20, },
new int[] {1, 3, 4, 8, 9, 11, 12, 15, 16, 17, 20, },
new int[] {2, 5, 6, 7, 13, 14, },
new int[] {4, 8, 9, 10, 11, 12, 15, 16, 17, 18, },
            });

            var pb5 = new PathNetworkConfig(
            new Vector2(-10f, -5f),
            20f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(-2.525126f, -4.767767f), new Vector2(-6.767767f, -0.5251262f), new Vector2(-7.474874f, -1.232233f), new Vector2(-3.232233f, -5.474874f), },
new Vector2[] {new Vector2(0.5177668f, 4.174874f), new Vector2(-3.724874f, -0.06776702f), new Vector2(-3.017767f, -0.7748737f), new Vector2(1.224874f, 3.467767f), },
new Vector2[] {new Vector2(3.25f, -0.7f), new Vector2(-0.75f, -0.7f), new Vector2(-0.75f, -4.7f), new Vector2(3.25f, -4.7f), },
new Vector2[] {new Vector2(9.497766f, -1.045126f), new Vector2(5.255126f, -5.287767f), new Vector2(5.962233f, -5.994874f), new Vector2(10.20487f, -1.752233f), },
new Vector2[] {new Vector2(8.03007f, 4.171314f), new Vector2(8.07f, 5.060474f), new Vector2(7.31993f, 4.581314f), new Vector2(6.576751f, 4.81409f), new Vector2(6.683783f, 4.059474f), new Vector2(6.219501f, 3.415314f), new Vector2(7.035f, 3.267802f), new Vector2(7.5705f, 2.635314f), new Vector2(7.896218f, 3.359474f), new Vector2(8.60325f, 3.64409f), },
new Vector2[] {new Vector2(-7.99f, 2.45f), new Vector2(-8.4f, 3.24f), new Vector2(-8.809999f, 2.45f), new Vector2(-9.57f, 2.28f), new Vector2(-9.099999f, 1.68f), new Vector2(-9.179999f, 0.89f), new Vector2(-8.4f, 1.17f), new Vector2(-7.62f, 0.89f), new Vector2(-7.7f, 1.68f), new Vector2(-7.23f, 2.28f), },
            },
            0.25f,
            new Vector2[] { new Vector2(0f, 0f), new Vector2(-9.5f, 0.5f), new Vector2(0f, -5.5f), new Vector2(1f, -3f), new Vector2(-2f, -4f), new Vector2(3.4f, -5.5f), new Vector2(0f, 4.7f), new Vector2(-9.5f, -4.5f), new Vector2(9.5f, 4.5f), new Vector2(9.5f, -4.5f), new Vector2(-9.5f, 4.5f), new Vector2(7.928774f, 0.8195872f), new Vector2(-6.467965f, 2.319695f), new Vector2(7.462442f, -1.443398f), new Vector2(4.735389f, -4.001899f), new Vector2(-8.724384f, 4.410263f), new Vector2(-4.999627f, -4.593158f), new Vector2(2.679596f, 4.12856f), new Vector2(-3.280698f, -1.463628f), new Vector2(-7.923193f, 4.079276f), new Vector2(-4.973148f, 3.204439f), new Vector2(-6.916506f, -2.732321f), new Vector2(5.311093f, -3.102075f), new Vector2(4.198704f, 0.9872608f), new Vector2(-7.121594f, -3.757473f), new Vector2(-4.837159f, -0.4880481f), },
            new int[][] {new int[] {11, 17, 18, 23, },
new int[] {7, 16, 21, 24, 25, },
new int[] {5, },
new int[] {},
new int[] {12, 18, 19, 25, },
new int[] {2, },
new int[] {10, 12, 15, 17, 19, 20, 25, },
new int[] {1, 12, 16, 21, 24, },
new int[] {11, 13, 14, 22, },
new int[] {},
new int[] {6, 12, 15, 19, 20, },
new int[] {0, 8, 13, 14, 17, 22, 23, },
new int[] {4, 6, 7, 10, 15, 18, 19, 20, 25, },
new int[] {8, 11, 14, 17, 22, 23, },
new int[] {8, 11, 13, 17, 22, 23, },
new int[] {6, 10, 12, 18, 19, 20, },
new int[] {1, 7, 21, 24, },
new int[] {0, 6, 11, 13, 14, 18, 22, 23, },
new int[] {0, 4, 12, 15, 17, 19, 25, },
new int[] {4, 6, 10, 12, 15, 18, 20, 25, },
new int[] {6, 10, 12, 15, 19, 25, },
new int[] {1, 7, 16, 24, },
new int[] {8, 11, 13, 14, 17, 23, },
new int[] {0, 11, 13, 14, 17, 22, },
new int[] {1, 7, 16, 21, },
new int[] {1, 4, 6, 12, 18, 19, 20, },
            });

            var pb6 = new PathNetworkConfig(
            new Vector2(-5f, -5f),
            10f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(1f, 1f), new Vector2(0f, 1f), new Vector2(0f, 0f), new Vector2(1f, 0f), },
new Vector2[] {new Vector2(3f, 3f), new Vector2(2f, 3f), new Vector2(2f, 2f), new Vector2(3f, 2f), },
            },
            0.25f,
            new Vector2[] { new Vector2(-4.5f, -4.5f), new Vector2(4.5f, 4.5f), new Vector2(4.5f, -4.5f), new Vector2(-4.5f, 4.5f), new Vector2(-0.3966575f, 4.396721f), new Vector2(1.522794f, -0.1116495f), new Vector2(-3.532882f, 4.456426f), new Vector2(1.31626f, -1.775975f), new Vector2(1.413421f, 0.2512116f), new Vector2(-2.170723f, 0.3285379f), new Vector2(-4.279467f, 0.4430504f), new Vector2(0.2952785f, 2.107143f), new Vector2(-1.385468f, 1.422808f), new Vector2(3.560646f, 3.183976f), },
            new int[][] {new int[] {2, 3, 4, 5, 6, 7, 9, 10, 11, 12, },
new int[] {2, 3, 4, 6, 10, 13, },
new int[] {0, 1, 4, 5, 7, 8, 9, 10, 13, },
new int[] {0, 1, 4, 6, 9, 10, 11, 12, 13, },
new int[] {0, 1, 2, 3, 6, 9, 10, 11, 12, 13, },
new int[] {0, 2, 7, 8, },
new int[] {0, 1, 3, 4, 9, 10, 11, 12, 13, },
new int[] {0, 2, 5, 8, 9, 10, },
new int[] {2, 5, 7, },
new int[] {0, 2, 3, 4, 6, 7, 10, 11, 12, },
new int[] {0, 1, 2, 3, 4, 6, 7, 9, 11, 12, },
new int[] {0, 3, 4, 6, 9, 10, 12, },
new int[] {0, 3, 4, 6, 9, 10, 11, },
new int[] {1, 2, 3, 4, 6, },
            });

            var pb7 = new PathNetworkConfig(
            new Vector2(-5f, -5f),
            10f, 10f,
            new Vector2[][] {
new Vector2[] {new Vector2(1.414214f, -8.940697E-08f), new Vector2(1.192093E-07f, 1.414214f), new Vector2(-1.414214f, 8.940697E-08f), new Vector2(-1.192093E-07f, -1.414214f), },
new Vector2[] {new Vector2(5f, 4.5f), new Vector2(-5f, 4.5f), new Vector2(-5f, 3.5f), new Vector2(5f, 3.5f), },
new Vector2[] {new Vector2(2.1f, -2.95f), new Vector2(0.4999999f, -2.95f), new Vector2(0.4999999f, -3.25f), new Vector2(2.1f, -3.25f), },
            },
            0.5f,
            new Vector2[] { new Vector2(8f, 0f), new Vector2(4.9f, 0f), new Vector2(-1.65f, -4f), new Vector2(-1.65f, 2.7f), new Vector2(3f, -4f), new Vector2(3f, 3.2f), new Vector2(0f, -2.2f), new Vector2(-4f, 0f), },
            new int[][] {new int[] {},
new int[] {},
new int[] {4, 6, 7, },
new int[] {7, },
new int[] {2, },
new int[] {},
new int[] {2, 7, },
new int[] {2, 3, 6, },
            });


            if (pnc.Equals(pb0))
                pathEdges = pb0.pathEdges;
            else if (pnc.Equals(pb1))
                pathEdges = pb1.pathEdges;
            else if (pnc.Equals(pb2))
                pathEdges = pb2.pathEdges;
            else if (pnc.Equals(pb3))
                pathEdges = pb3.pathEdges;
            else if (pnc.Equals(pb4))
                pathEdges = pb4.pathEdges;
            else if (pnc.Equals(pb5))
                pathEdges = pb5.pathEdges;
            else if (pnc.Equals(pb6))
                pathEdges = pb6.pathEdges;
            else if (pnc.Equals(pb7))
                pathEdges = pb7.pathEdges;
            else
                pathEdges = new List<List<int>>();
        }



        public class PathNetworkConfig : IEquatable<PathNetworkConfig>
        {
            public Vector2 canvasOrigin { get; private set; }
            public float canvasWidth { get; private set; }
            public float canvasHeight { get; private set; }
            public List<Obstacle> obstacles { get; private set; }
            public Vector2[][] obstaclePoints { get; private set; }
            public float agentRadius { get; private set; }
            public List<Vector2> pathNodes { get; private set; }
            public Vector2[] pathNodePoints { get; private set; }
            public List<List<int>> pathEdges { get; private set; }
            public int[][] pathEdgePoints { get; private set; }


            public PathNetworkConfig(Vector2 canvasOrigin, float canvasWidth, float canvasHeight,
                List<Obstacle> obstacles, float agentRadius, List<Vector2> pathNodes)
            {
                this.canvasOrigin = canvasOrigin;
                this.canvasWidth = canvasWidth;
                this.canvasHeight = canvasHeight;
                this.obstacles = obstacles;
                this.agentRadius = agentRadius;
                this.pathNodes = pathNodes;

                Vector2[][] obstaclePoints = new Vector2[obstacles.Count][];

                for (int i = 0; i < obstaclePoints.Length; ++i)
                {
                    var pts = obstacles[i].GetPoints();
                    obstaclePoints[i] = new Vector2[pts.Length];

                    for (int j = 0; j < pts.Length; ++j)
                    {
                        obstaclePoints[i][j] = pts[j];
                    }
                }

                this.obstaclePoints = obstaclePoints;

                this.pathNodePoints = pathNodes.ToArray();

            }

            public PathNetworkConfig(Vector2 canvasOrigin, float canvasWidth, float canvasHeight,
                    Vector2[][] obstaclePoints, float agentRadius, Vector2[] pathNodePoints)
            {
                this.canvasOrigin = canvasOrigin;
                this.canvasWidth = canvasWidth;
                this.canvasHeight = canvasHeight;

                this.obstaclePoints = obstaclePoints;

                List<Obstacle> obstacles = new List<Obstacle>();

                for (int i = 0; i < obstaclePoints.Length; ++i)
                {
                    var go = new GameObject("obstacle", typeof(Obstacle));
                    go.SetActive(false);
                    var ob = go.GetComponent<Obstacle>();
                    ob.SetPoints(obstaclePoints[i]);
                    obstacles.Add(ob);
                }

                this.obstacles = obstacles;
                this.agentRadius = agentRadius;

                this.pathNodePoints = pathNodePoints;

                List<Vector2> pathNodes = new List<Vector2>(pathNodePoints);

                this.pathNodes = pathNodes;
                this.pathEdges = pathEdges;
            }

            public PathNetworkConfig(Vector2 canvasOrigin, float canvasWidth, float canvasHeight,
                Vector2[][] obstaclePoints, float agentRadius, Vector2[] pathNodePoints,
                int[][] pathEdgePoints) :
                this(canvasOrigin, canvasWidth, canvasHeight, obstaclePoints, agentRadius, pathNodePoints)
            {

                this.pathEdgePoints = pathEdgePoints;

                List<List<int>> pathEdges = new List<List<int>>();

                for (int i = 0; i < pathEdgePoints.Length; ++i)
                {
                    List<int> edges = new List<int>(pathEdgePoints[i]);
                    pathEdges.Add(edges);
                }
                this.pathEdges = pathEdges;
            }

            bool pathNodePointsEqual(Vector2[] o)
            {
                var oo = this.pathNodePoints;
                if (oo == null && o == null)
                    return true;

                if ((oo != null && o == null) || (oo == null && o != null))
                    return false;

                if (oo.Length != o.Length)
                    return false;

                for (int i = 0; i < oo.Length; ++i)
                {
                    if (o[i] != oo[i])
                        return false;
                }

                return true;
            }

            bool obstaclesEqual(Vector2[][] o)
            {
                var oo = this.obstaclePoints;
                if (oo == null && o == null)
                    return true;

                if ((oo != null && o == null) || (oo == null && o != null))
                    return false;

                if (oo.Length != o.Length)
                    return false;

                for (int i = 0; i < oo.Length; ++i)
                {
                    if (oo[i] == null && o[i] == null)
                        continue;

                    if ((oo[i] != null && o[i] == null) || (oo[i] == null && o[i] != null))
                        return false;

                    if (oo[i].Length != o[i].Length)
                        return false;

                    for (int j = 0; j < oo[i].Length; ++j)
                    {
                        if (oo[i][j] != o[i][j])
                            return false;
                    }
                }

                return true;
            }

            public bool Equals(PathNetworkConfig p)
            {
                //we don't compare obstacleEdges because that is what we are trying to find
                if (this.canvasOrigin != p.canvasOrigin ||
                    this.canvasWidth != p.canvasWidth ||
                    this.canvasHeight != p.canvasHeight ||
                    this.agentRadius != p.agentRadius ||
                    //!this.pathNodePoints.SequenceEqual(p.pathNodePoints) ||
                    !pathNodePointsEqual(p.pathNodePoints) ||
                    !obstaclesEqual(p.obstaclePoints)
                        )
                    return false;

                return true;
            }


            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                var nl = System.Environment.NewLine;
                sb.Append("var pb = new PathNetworkConfig(" + nl);
                sb.Append($"new Vector2({this.canvasOrigin.x}f, {this.canvasOrigin.y}f), " + nl);
                sb.Append($"{this.canvasWidth}f, {this.canvasHeight}f, " + nl);

                sb.Append($"new Vector2[][] {{" + nl);
                foreach (var op in this.obstaclePoints)
                {
                    sb.Append($"new Vector2[] {{");

                    foreach (var v in op)
                    {
                        sb.Append($"new Vector2({v.x}f, {v.y}f), ");
                    }
                    //sb.Append(nl);
                    sb.Append($"}}, " + nl);
                }
                sb.Append($"}}, " + nl);

                sb.Append($"{this.agentRadius}f, " + nl);

                sb.Append($"new Vector2[] {{");
                foreach (var v in this.pathNodePoints)
                {
                    sb.Append($"new Vector2({v.x}f, {v.y}f), ");
                }
                //sb.Append(nl);
                sb.Append($"}}, " + nl);

                sb.Append($"new int[][] {{");

                foreach (var pe in this.pathEdgePoints)
                {
                    sb.Append($"new int[] {{");
                    foreach (var p in pe)
                    {
                        sb.Append($"{p}, ");
                    }
                    //sb.Append(nl);
                    sb.Append($"}}, " + nl);
                }

                sb.Append($"}});" + nl);

                return sb.ToString();
            }
        }




    }

}