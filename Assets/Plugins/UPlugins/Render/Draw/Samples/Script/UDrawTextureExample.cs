using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Aya.Render.Draw.Samples
{
    public class UDrawTextureExample : MonoBehaviour
    {
        public Image Image;

        public Texture2D Texture { get; set; }

        public void Awake()
        {
            Texture = new Texture2D(1280, 720);
        }

        public void Start()
        {
            Texture.BeginDraw();

            Texture.Clear(Color.clear);

            Texture.DrawPoint(100, 50, Color.red, 1);
            Texture.DrawLine(new Vector2(100, 150), new Vector2(200, 200), Color.red);
            Texture.DrawTriangle(new Vector2(150, 250), new Vector3(100, 300), new Vector3(200, 350), Color.red);
            Texture.DrawRectangle(new Vector2(100, 450), new Vector2(150, 100), Color.red);
            Texture.DrawCircle(new Vector2(150, 650), 50, Color.red);

            Texture.DrawPoint(300, 50, Color.red, 5);
            Texture.DrawLine(new Vector2(300, 150), new Vector2(400, 200), Color.red, 5);
            Texture.FillTriangle(new Vector2(350, 250), new Vector3(300, 300), new Vector3(400, 350), Color.red);
            Texture.FillRectangle(new Vector2(300, 450), new Vector2(150, 100), Color.red);
            Texture.FillCircle(new Vector2(350, 650), 50, Color.red);

            var pList = new Vector2[]
            {
                new Vector2(100, 0),
                new Vector2(159, 181),
                new Vector2(5, 69),
                new Vector2(195, 69),
                new Vector2(41, 181),
            };
            for (var i = 0; i < pList.Length; i++)
            {
                pList[i] += new Vector2(500, 200);
            }

            Texture.FillPolygon(pList, Color.red);

            Texture.FillWithBorder(0, 400, Color.green, Color.red);

            Texture.EndDraw();

            Image.sprite = Sprite.Create(Texture, new Rect(0, 0, Texture.width, Texture.height), Vector2.zero);
        }

        public void Update()
        {

        }
    }
}
