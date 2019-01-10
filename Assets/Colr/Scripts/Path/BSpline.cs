using UnityEngine;
using System;

namespace Habtic.Games.Colr
{
    public class BSpline
    {

        public Vector3[] Points { get { return _points; } }

        private Vector3[] _points;      // Control points
        private int[] _nV;              // Node vector
        private int n = 2;              // Degree of the curve

        public BSpline(params Vector3[] pts)
        {
            _points = new Vector3[pts.Length];
            Array.Copy(pts, _points, pts.Length);

            // Initialize node vector.
            _nV = new int[_points.Length + n + 1];
            CreateNodeVector();
        }

        private void CreateNodeVector()
        {
            int knots = 0;

            for (int i = 0; i < _nV.Length; i++) // n+m+1 = nr of nodes
            {
                if (i > n)
                {
                    if (i <= _points.Length)
                    {

                        _nV[i] = ++knots;
                    }
                    else
                    {
                        _nV[i] = knots;
                    }
                }
                else
                {
                    _nV[i] = knots;
                }
            }
        }

        // Recursive De Boor's algorithm.
        private Vector3 DeBoor(int r, int i, float u)
        {
            if (r == 0)
            {
                //i = Mathf.Clamp(i, 0, _points.Length - 1);
                return _points[i];
            }
            else
            {
                float pre = (u - _nV[i + r]) / (_nV[i + n + 1] - _nV[i + r]); // Precalculation
                return ((DeBoor(r - 1, i, u) * (1 - pre)) + (DeBoor(r - 1, i + 1, u) * (pre)));
            }
        }

        public Vector3 GetPoint(float t)
        {
            float u = _nV[n + _points.Length] * Mathf.Clamp01(t);
            int i = Mathf.FloorToInt(u);
            return DeBoor(n, i, u);
        }

        public float ApproximateDistance()
        {
            float distance = 0f;

            for (int i = 1; i < _points.Length; i++)
            {
                distance += Vector2.Distance(_points[i - 1], _points[i]);
            }

            return distance;
        }

        public void DrawGizmos()
        {

        }
    }
}
