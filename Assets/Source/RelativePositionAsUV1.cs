using UnityEngine;

namespace UnityEngine.UI
{

    [AddComponentMenu("UI/Effects/Relative Position As UV1", 16)]
    public class RelativePositionAsUV1 : BaseMeshEffect
    {
        private float localMinX;
        private float localMaxX;
        private float localMinY;
        private float localMaxY;

        private float globalMinX;
        private float globalMinY;
        private float globalMaxX;
        private float globalMaxY;

        private Vector2 _uv1;
        private Vector2 _uv2;

        public override void ModifyMesh(VertexHelper vh)
        {
            UIVertex _vert = new UIVertex();

            globalMinX = 0f;
            globalMaxX = 0f;
            globalMinY = 0f;
            globalMaxY = 0f;

            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref _vert, i);
                globalMinX = Mathf.Min(globalMinX, _vert.position.x);
                globalMinY = Mathf.Min(globalMinY, _vert.position.y);
                globalMaxX = Mathf.Max(globalMaxX, _vert.position.x);
                globalMaxY = Mathf.Max(globalMaxY, _vert.position.y);
            }

            for (int i = 0; i < vh.currentVertCount; i += 4)
            {
                vh.PopulateUIVertex(ref _vert, i);
                _uv1.x = Mathf.InverseLerp(globalMinX, globalMaxX, _vert.position.x);
                _uv1.y = Mathf.InverseLerp(globalMinX, globalMaxX, _vert.position.x);

                localMinX = Mathf.Infinity;
                localMaxX = -Mathf.Infinity;
                localMinY = Mathf.Infinity;
                localMaxY = -Mathf.Infinity;

                for (int j = i; j < i +4; j ++)
                {
                    vh.PopulateUIVertex(ref _vert, j);
                    localMinX = Mathf.Min(_vert.position.x, localMinX);
                    localMaxX = Mathf.Max(_vert.position.x, localMaxX);
                    localMinY = Mathf.Min(_vert.position.y, localMinY);
                    localMaxY = Mathf.Max(_vert.position.y, localMaxY);
                    _vert.uv1 = _uv1;
                    _uv2.x = Mathf.InverseLerp(localMinX, localMaxX, _vert.position.x);
                    _uv2.y = Mathf.InverseLerp(localMinY, localMaxY, _vert.position.y);
                    _vert.uv2 = _uv2;
                    //_vert.uv2 =  new Vector2 Mathf.InverseLerp(localMinX, localMaxX, _vert.position.x);  
                    //_vert.uv1 = new Vector2(i * 0.01f, i * 0.01f);
                    //_vert.uv2 = new Vector2((i + 1.0f) / vh.currentVertCount, (i + 1.0f) / vh.currentVertCount);
                    vh.SetUIVertex(_vert, j);
                }
            }
        }
    }
}