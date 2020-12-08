using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.MazeGenerator._scriptRA
{
    public struct MazeInfo
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public float FloorX { get; set; }
        public float FloorZ { get; set; }

        private float grandeurX;
        public float GrandeurX
        {
            get
            {
                return Columns * FloorX;
            }
        }

        private float grandeurZ;
        public float GrandeurZ
        {
            get
            {
                return Rows * FloorZ;
            }
        }

        private Vector3 v3Center;
        public Vector3 V3Center
        {
            get
            {
                return new Vector3(GrandeurX/2, 0, GrandeurZ/2);
            }
        }

    }
}
