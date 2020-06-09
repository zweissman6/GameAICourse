using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAICourse
{


    public class CreateNavMesh
    {

        public static string StudentAuthorName = "George P. Burdell ← Not your name, change it!";

        // Create(): Creates a navmesh and pathnetwork (associated with navmesh) 
        // canvasOrigin: bottom left corner of navigable region in world coordinates
        // canvasWidth: width of navigable region in world dimensions
        // canvasHeight: height of navigable region in world dimensions
        // obstacles: a list of Polygons that are obstacles in the scene
        // agentRadius: the radius of the agent
        // origTriangles: out param of the triangles that are used for navmesh generation
        //          These triangles are passed out for visualization.
        // navmeshPolygons: out param of the convex polygons of the navmesh (list). 
        //          These polys are passed out for visualization
        // pathNodes: a list of graph nodes, centered on each navmeshPolygon
        // pathEdges: graph adjacency list for each graph node. cooresponding index of pathNodes to match
        //      node with its edge list. All nodes must have an edge list (no null list)
        //      entries in each edge list are indices into pathNodes

        // NOTES:
        // Normally we would want to return a graph of navmeshPolygons with additional
        // data such as which edges are boundaries, which are are portals (and where they go to),
        // etc. However, for the purposes of this assignment you only return the pathNetwork as
        // well as the unconnected navmeshPolygons and the original triangles you formed.
        // 

        public static void Create(
            Vector2 canvasOrigin, float canvasWidth, float canvasHeight,
            List<Polygon> obstacles, float agentRadius,
            out List<Polygon> origTriangles, 
            out List<Polygon> navmeshPolygons,
            out List<Vector2> pathNodes, 
            out List<List<int>> pathEdges)
        {

            // Some basic initialization
            pathEdges = new List<List<int>>();
            pathNodes = new List<Vector2>();

            origTriangles = new List<Polygon>();
            navmeshPolygons = null;

            // This is a special dictionary for tracking polygons that share
            // edges. It is going to be used to determine which triangles can be merged
            // into larger convex polygons. Later, it will also be used for generating
            // the pathNetwork on top of the navmesh
            AdjacentPolygons adjPolys = new AdjacentPolygons();

            // Holds the complex set of polys representing obstacle boundaries
            // Any time you need to test against obstacles, use offsetObstPolys
            // instead of obstacles
            List<Polygon> offsetObstPolys;

            // This creates a complex set of polygons representing the obstacle boundaries.
            // It's built with a 3rd party library called Clipper. In addition
            // to finding the union of obstacle boundaries, and clipping against the canvas, 
            // it also performs expansion for agentOffset
            Utils.GenerateOffsetNavSpace(canvasOrigin, canvasWidth, canvasHeight,
               agentRadius, obstacles, out offsetObstPolys);

            // Obtain all the vertices that are going to be used to form our triangles
            List<Vector2> obstacleVertices;
            Utils.AllVerticesFromPolygons(offsetObstPolys, out obstacleVertices);

            // Let's also add the four corners of the canvas (with offset)
            var A = canvasOrigin + new Vector2(agentRadius, agentRadius);
            var B = canvasOrigin + new Vector2(0f, canvasHeight) + new Vector2(agentRadius, -agentRadius);
            var C = canvasOrigin + new Vector2(canvasWidth, canvasHeight) + new Vector2(-agentRadius, -agentRadius);
            var D = canvasOrigin + new Vector2(canvasWidth, 0f) + new Vector2(-agentRadius, agentRadius);

            obstacleVertices.Add(A);
            obstacleVertices.Add(B);
            obstacleVertices.Add(C);
            obstacleVertices.Add(D);


            // ******************** PHASE 0 - Change your name string ************************
            // TODO set your name above

            //********************* PHASE I - Brute force triangle formation *****************
  
            // In this phase, some scaffolding is provided for you. Your goal to to produce
            // triangles that will serve as the foundation of your navmesh. You will use
            // a brute force method of evaluating all combinations of three vertices to see
            // if a valid triangle is formed. This includes checking for degenerate triangles, 
            // triangles that intersect obstacle boundaries, and triangles that intersect
            // triangles you already made. There is also a special test to see if triangles
            // break adjacency (described later).

            // Iterate through combinations of obstacle vertices that can form triangle
            // candidates.
            for (int i = 0; i < obstacleVertices.Count - 2; ++i)
            {

                for (int j = i; j < obstacleVertices.Count - 1; ++j)
                {

                    for (int k = j; k < obstacleVertices.Count; ++k)
                    {
                        // These are vertices for a candidate triangle
                        // that we hope to form
                        var V1 = obstacleVertices[i];
                        var V2 = obstacleVertices[j];
                        var V3 = obstacleVertices[k];

                        // TODO If you completed all of the triangle generation above, 
                        // you can just return from the Create() method here to test what you have
                        // accomplished so far. The originalTriangles
                        // will be visualized as translucent yellow polys. Since they are translucent,
                        // any accidental tri overlaps will be a darker shade of yellow. (Useful
                        // for debugging.)
                        // Also, navmeshPolygons is initially just the tris. Those are visualized 
                        // as a blue outline. Note that the blue lineweight is very thin for better 
                        // debugging of small polys


                        // ********************* PHASE II - Merge Triangles *****************************
                        // 
                        // This phase involves merging triangles into larger convex polygons for the sake
                        // of effeciency. If you like, you can temporarily skip to phase 3 and come back
                        // later.
                        // 
                        // TODO Next up, you need to merge triangles into larger convex polygons where
                        // possible. The greedy strategy you will use involves examining adjacent
                        // tris and seeing if they can be merged into one new convex tri.
                        // 
                        // At the beginning of this process, you should make a copy of adjPolys. Continue
                        // reading below to see why. You can copy like this: 
                        // newAdjPolys = new AdjacentPolygons(adjPolys);
                        // 
                        // Iterate through adjPolys.Keys (type:CommonPolygonEdge) and get the value 
                        // (type:CommonPolygons) for each key. This structure identifies only one polygon
                        // if the edge is a boundary (.IsBarrier), but otherwise .AB and .BA references 
                        // the adjacent polys. You can also get the .CommonEdge (with vertices .A and .B).
                        // (The AB/BA refers to orientation of the common edge AB within each poly 
                        // relative to the winding of the polygon.)
                        // If you have two polygons AB and BA (NOT .IsBarrier), then use 
                        // Utils.MergePolygons() to create a new polygon candidate. You need to 
                        // check poly.IsConvex() to decide if it's valid. 
                        // If it is valid, then you need to remove the common edge (and merged polys)
                        // from your adjPolys dictionary and also add the new, larger convex poly. 
                        // And further, you need all the other common edges of the two old merged polys 
                        // to be updated with the merged version.
                        // You actually want to perform the dictionary operations on "newAdjPolys" that
                        // you created above. This is because you never want to add/remove items
                        // to a data structure that you are iterating over. A slightly more efficient
                        // alternative would be to make dedicated add and delete lists and apply them
                        // after enumeration is complete.
                        // The removal of a common edge can be accomplished with newAdjPolys.Remove().
                        // You can add the new merged polygon and update all old poly references with
                        // a single method call:
                        // AddPolygon(Polygon p, Polygon replacePolyA, Polygon replacePolyB)
                        // Similar to the updates to newAdjPolys, you also want to remove old polys
                        // and add the new poly to navMeshPolygons.
                        // When your loop is finished, don't forget to set adjPolys to newAdjPolys.

                        // TODO At this point you can visualize a single pass of the merging (e.g. test your
                        // code). After that, wrap it all in a loop that tries successive passes of
                        // merges, tracking how many successful merges occur. Your loop should terminate
                        // when no merges are successful.


                        // *********************** PHASE 3 - Path Network from NavMesh *********************

                        // The last step is to create a PathNetwork from your navMesh
                        // This will involve iterating over the keys of adjPolys so you can get the 
                        // CommonPolygons values.
                        //
                        // Issues you need to address are:
                        // 1.) Calculate centroids of each polygon to be your pathNodes
                        // 2.) Implement a method for mapping adjacent polygons to a pathNode index
                        //     so that you can populate pathEdges.
                        //
                        // For 1.), poly.GetCentroid() will calculate the Vector2 position for you
                        // 2.) is a bit more challenging. I recommend the use of a Dictionary
                        // with a Polygon as key and the value is an int (for the pathNode index).
                        // This dictionary can be populated with pathNode indices by iterating over
                        // navmeshPolygons (List<Polygon>). This loop is also a good time to populate
                        // pathNodes with the Vector2 centroids and prime the pathEdges with empty lists.
                        // If you have resolved dev issues 1.) and 2.), you can then work with adjPolys
                        // to create your edges!


                        // ***************************** FINAL **********************************************
                        // Once you have completed everything, you will probably find that the code
                        // is very slow. This is largely due to the use of complex polygons. 
                        // A production solution would instead work with only convex polygons (by
                        // performing some sort of convex decomposition such as a triangulation).
                        // 
                        // If computation time is making testing difficult for you, first focus on the 
                        // most simple maps. The default config should already be one of the easiest.
                        // Additionally, you might consider temporarily changing:
                        // ClipperHelper.ClipperExpand() to JoinType.jtMiter insead of JoinType.jtRound.
                        // That change makes the scene have fewer vertices (no rounded offsets)

                    } // for
                } // for
            } // for

            // Priming the navmeshPolygons for next steps, and also allow visualization
            navmeshPolygons = new List<Polygon>(origTriangles);

            // TODO If you completed all of the triangle generation above, 
            // you can just return from the Create() method here to test what you have
            // accomplished so far. The originalTriangles
            // will be visualized as translucent yellow polys. Since they are translucent,
            // any accidental tri overlaps will be a darker shade of yellow. (Useful
            // for debugging.)
            // Also, navmeshPolygons is initially just the tris. Those are visualized 
            // as a blue outline. Note that the blue lineweight is very thin for better 
            // debugging of small polys


            // ********************* PHASE II - Merge Triangles *****************************
            // 
            // This phase involves merging triangles into larger convex polygons for the sake
            // of effeciency. If you like, you can temporarily skip to phase 3 and come back
            // later.
            // 
            // TODO Next up, you need to merge triangles into larger convex polygons where
            // possible. The greedy strategy you will use involves examining adjacent
            // tris and seeing if they can be merged into one new convex tri.
            // 
            // At the beginning of this process, you should make a copy of adjPolys. Continue
            // reading below to see why. You can copy like this: 
            // newAdjPolys = new AdjacentPolygons(adjPolys);
            // 
            // Iterate through adjPolys.Keys (type:CommonPolygonEdge) and get the value 
            // (type:CommonPolygons) for each key. This structure identifies only one polygon
            // if the edge is a boundary (.IsBarrier), but otherwise .AB and .BA references 
            // the adjacent polys. You can also get the .CommonEdge (with vertices .A and .B).
            // (The AB/BA refers to orientation of the common edge AB within each poly 
            // relative to the winding of the polygon.)
            // If you have two polygons AB and BA (NOT .IsBarrier), then use 
            // Utils.MergePolygons() to create a new polygon candidate. You need to 
            // check poly.IsConvex() to decide if it's valid. 
            // If it is valid, then you need to remove the common edge (and merged polys)
            // from your adjPolys dictionary and also add the new, larger convex poly. 
            // And further, you need all the other common edges of the two old merged polys 
            // to be updated with the merged version.
            // You actually want to perform the dictionary operations on "newAdjPolys" that
            // you created above. This is because you never want to add/remove items
            // to a data structure that you are iterating over. A slightly more efficient
            // alternative would be to make dedicated add and delete lists and apply them
            // after enumeration is complete.
            // The removal of a common edge can be accomplished with newAdjPolys.Remove().
            // You can add the new merged polygon and update all old poly references with
            // a single method call:
            // AddPolygon(Polygon p, Polygon replacePolyA, Polygon replacePolyB)
            // Similar to the updates to newAdjPolys, you also want to remove old polys
            // and add the new poly to navMeshPolygons.
            // When your loop is finished, don't forget to set adjPolys to newAdjPolys.

            // TODO At this point you can visualize a single pass of the merging (e.g. test your
            // code). After that, wrap it all in a loop that tries successive passes of
            // merges, tracking how many successful merges occur. Your loop should terminate
            // when no merges are successful.


            // *********************** PHASE 3 - Path Network from NavMesh *********************

            // The last step is to create a PathNetwork from your navMesh
            // This will involve iterating over the keys of adjPolys so you can get the 
            // CommonPolygons values.
            //
            // Issues you need to address are:
            // 1.) Calculate centroids of each polygon to be your pathNodes
            // 2.) Implement a method for mapping adjacent polygons to a pathNode index
            //     so that you can populate pathEdges.
            //
            // For 1.), poly.GetCentroid() will calculate the Vector2 position for you
            // 2.) is a bit more challenging. I recommend the use of a Dictionary
            // with a Polygon as key and the value is an int (for the pathNode index).
            // This dictionary can be populated with pathNode indices by iterating over
            // navmeshPolygons (List<Polygon>). This loop is also a good time to populate
            // pathNodes with the Vector2 centroids and prime the pathEdges with empty lists.
            // If you have resolved dev issues 1.) and 2.), you can then work with adjPolys
            // to create your edges!


            // ***************************** FINAL **********************************************
            // Once you have completed everything, you will probably find that the code
            // is very slow. This is largely due to the use of complex polygons. 
            // A production solution would instead work with only convex polygons (by
            // performing some sort of convex decomposition such as a triangulation).
            // 

        } // Create()



        class AdjacentPolygons : Dictionary<CommonPolygonEdge, CommonPolygons>
        {
            public AdjacentPolygons() : base()
            {

            }

            public AdjacentPolygons(AdjacentPolygons ap) : base(ap)
            {

            }

            public void AddPolygon(Polygon p)
            {
                AddPolygon(p, null, null);
            }

            public void AddPolygon(Polygon p, Polygon replacePolyA, Polygon replacePolyB)
            {
                if (p == null)
                    return;

                var pts = p.getPoints();
                for (int i = 0, j = pts.Length - 1; i < pts.Length; j = i++)
                {
                    var cpe = new CommonPolygonEdge(pts[j], pts[i]);

                    if (!this.ContainsKey(cpe))
                    {
                        this.Add(cpe, new CommonPolygons(cpe, p));
                    }
                    else
                    {
                        int clearSpots = 0;

                        if (replacePolyA != null)
                        {
                            if (this[cpe].AB == replacePolyA)
                            {
                                this[cpe].ClearABPolygon();
                                ++clearSpots;
                            }
                            else if (this[cpe].BA == replacePolyA)
                            {
                                this[cpe].ClearBAPolygon();
                                ++clearSpots;
                            }

                        }
                        else
                            ++clearSpots;

                        if (replacePolyB != null)
                        {
                            if (this[cpe].AB == replacePolyB)
                            {
                                this[cpe].ClearABPolygon();
                                ++clearSpots;
                            }
                            else if (this[cpe].BA == replacePolyB)
                            {
                                this[cpe].ClearBAPolygon();
                                ++clearSpots;
                            }

                        }
                        else
                            ++clearSpots;

                        if (clearSpots <= 0)
                        {
                            Debug.LogError("Failed to add poly");
                        }
                        else
                        {
                            this[cpe].Add(p);
                        }
                    }
                }
            }

        } //class


    }

}