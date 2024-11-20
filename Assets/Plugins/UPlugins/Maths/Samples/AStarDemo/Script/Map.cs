using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Aya.Test;

namespace Aya.Maths.Sample
{
    public class Map : MonoBehaviour
    {
        // public MapNode NodePrefab;
        // public Vector2Int MapSize;
        // public Vector2 CellSize;

        public Image Image;
        public Texture2D MapTexture;
        public Color PathColor;

        public Vector2Int StartPoint;
        public Vector2Int EndPoint;

        [NonSerialized] public Texture2D Texture;
        [NonSerialized] public int Width;
        [NonSerialized] public int Height;
        [NonSerialized] public MapNode[,] MapArray;
        [NonSerialized] public List<AStarNode> MapNodeList;

        public void Start()
        {
            InitMap();
        }

        public void InitMap()
        {
            Width = MapTexture.width;
            Height = MapTexture.height;
            Texture = new Texture2D(Width, Height);

            MapNodeList = new List<AStarNode>();
            MapArray = new MapNode[Width, Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var pixel = MapTexture.GetPixel(x, y);
                    Texture.SetPixel(x, y, pixel);
                    var node = new MapNode();
                    node.Init(this, x, y);
                    var gray = (pixel.r + pixel.g + pixel.b) / 3f;
                    node.ItemType = gray < 0.5f ? AStarItemType.Obstacle : AStarItemType.Normal;
                    MapNodeList.Add(node);
                    MapArray[x, y] = node;
                }
            }

            Texture.Apply();
            Refresh();

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var node = MapArray[x, y];
                    node.CacheNeighbors();
                }
            }
        }

        [NonSerialized] public IList<AStarNode> Path;

        [Button("Search")]
        public void Search()
        {
            Path = null;
            StopwatchUtil.TestOnce(() =>
            {
                var startTime = Time.realtimeSinceStartup;
                var start = MapArray[StartPoint.x, StartPoint.y];
                var end = MapArray[EndPoint.x, EndPoint.y];
                Path = AStar.Search(MapNodeList, start, end);
                var endTime = Time.realtimeSinceStartup;

                Debug.Log("Time : " + (endTime - startTime));
                if (Path != null)
                {
                    Debug.Log("Find Path : " + Path.Count);
                }
            });
        }

        [Button("Draw Path")]
        public void DrawPath()
        {
            StopwatchUtil.TestOnce(() =>
            {
                if (Path != null)
                {
                    foreach (var item in Path)
                    {
                        var node = item as MapNode;
                        if (node == null) continue;
                        Texture.SetPixel(node.X, node.Y, PathColor);
                    }
                }

                Texture.Apply();
                Refresh();
            });
        }

        public void Refresh()
        {
            Image.sprite = Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), Vector2.one * 0.5f);
        }
    }
}