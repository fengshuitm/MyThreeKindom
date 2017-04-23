using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CreativeSpore.SuperTilemapEditor
{
    [System.Flags]
    public enum eAutotilingMode
    {
        /// <summary>
        /// Autotiling with brushes of the same type
        /// </summary>
        Self = 1,
        /// <summary>
        /// Autotiling  with tiles of different brush or any tile not empty
        /// </summary>
        Other = 1 << 1,
        /// <summary>
        /// Checks the Group Autotiling Mask to see if the relation between this brush and the neighbor brush is checked to do the autotiling
        /// </summary>
        Group = 1 << 2,
    }

    public class TilesetBrush : ScriptableObject, IBrush
    {
        public Tileset Tileset;
        public ParameterContainer Params = new ParameterContainer();
        public int Group { get { return m_group; } }
        public eAutotilingMode AutotilingMode { get { return m_autotilingMode; } }

        [SerializeField]
        private int m_group = 0;
        
        [SerializeField]
        private eAutotilingMode m_autotilingMode = eAutotilingMode.Self;

        public bool AutotileWith(int selfBrushId, int otherBrushId)
        {
            if( 
                ((AutotilingMode & eAutotilingMode.Self) != 0 && selfBrushId == otherBrushId ) ||
                ((AutotilingMode & eAutotilingMode.Other) != 0 && otherBrushId != selfBrushId && otherBrushId != (Tileset.k_TileDataMask_BrushId >> 16))
            )
            {
                return true;
            }
            else if((AutotilingMode & eAutotilingMode.Group) != 0)
            {
                TilesetBrush brush = Tileset.FindBrush(otherBrushId);
                if(brush)
                {
                    return Tileset.GetGroupAutotiling(Group, brush.Group);
                }
                else if (otherBrushId == Tileset.k_BrushId_Default)
                {
                    return Tileset.GetGroupAutotiling(Group, 0); //with normal tiles, use default group (0)
                }
            }
            return false;
        }

        protected static bool s_refreshingLinkedBrush = false; //avoid infinite loop
        public uint RefreshLinkedBrush(Tilemap tilemap, int gridX, int gridY, uint tileData)
        {
            if (s_refreshingLinkedBrush) return tileData;

            int brushId = Tileset.GetBrushIdFromTileData(tileData);
            TilesetBrush brush = Tileset.FindBrush(brushId);
            if (brush)
            {
                s_refreshingLinkedBrush = true;
                tileData = brush.Refresh(tilemap, gridX, gridY, tileData);
                s_refreshingLinkedBrush = false;
            }
            return tileData;
        }

        #region IBrush

        public virtual uint PreviewTileData()
        {
            return Tileset.k_TileData_Empty;
        }

        public virtual uint OnPaint(TilemapChunk chunk, int chunkGx, int chunkGy, uint tileData)
        {
            return tileData;
        }

        public virtual void OnErase(TilemapChunk chunk, int chunkGx, int chunkGy, uint tileData)
        {
            ;
        }

        public virtual uint Refresh(Tilemap tilemap, int gridX, int gridY, uint tileData)
        {
            return tileData;
        }

        public virtual bool IsAnimated()
        {
            return false;
        }

        public virtual Rect GetAnimUV()
        {
            int tileId = (int)(PreviewTileData() & Tileset.k_TileDataMask_TileId);
            return Tileset && tileId != Tileset.k_TileId_Empty ? Tileset.Tiles[tileId].uv : default(Rect);
        }

        public virtual int GetAnimFrameIdx()
        {
            return 0;
        }

        Vector2[] m_uvWithFlags = new Vector2[4];
        int m_lastFrameToken;
        public virtual Vector2[] GetAnimUVWithFlags()
        {
            if (GetAnimFrameIdx() == m_lastFrameToken)
                return m_uvWithFlags;
            else
                m_lastFrameToken = GetAnimFrameIdx();

            uint tileData = GetAnimTileData();
            Rect tileUV = GetAnimUV();

            bool flipH = (tileData & Tileset.k_TileFlag_FlipH) != 0;
            bool flipV = (tileData & Tileset.k_TileFlag_FlipV) != 0;
            bool rot90 = (tileData & Tileset.k_TileFlag_Rot90) != 0;

            //NOTE: xMinMax and yMinMax is opposite if width or height is negative
            float u0 = tileUV.xMin + Tileset.AtlasTexture.texelSize.x;// *InnerPadding;
            float v0 = tileUV.yMin + Tileset.AtlasTexture.texelSize.y;// *InnerPadding;
            float u1 = tileUV.xMax - Tileset.AtlasTexture.texelSize.x;// *InnerPadding;
            float v1 = tileUV.yMax - Tileset.AtlasTexture.texelSize.y;// *InnerPadding;

            if (flipV)
            {
                float v = v0;
                v0 = v1;
                v1 = v;
            }
            if (flipH)
            {
                float u = u0;
                u0 = u1;
                u1 = u;
            }

            if (rot90)
            {
                m_uvWithFlags[0] = new Vector2(u1, v0);
                m_uvWithFlags[1] = new Vector2(u1, v1);
                m_uvWithFlags[2] = new Vector2(u0, v0);
                m_uvWithFlags[3] = new Vector2(u0, v1);
            }
            else
            {
                m_uvWithFlags[0] = new Vector2(u0, v0);
                m_uvWithFlags[1] = new Vector2(u1, v0);
                m_uvWithFlags[2] = new Vector2(u0, v1);
                m_uvWithFlags[3] = new Vector2(u1, v1);
            }
            return m_uvWithFlags;
        }

        public virtual uint GetAnimTileData()
        {
            return PreviewTileData();
        }

        public virtual uint[] GetSubtiles(Tilemap tilemap, int gridX, int gridY, uint tileData)
        {
            return null;
        }

        #endregion
    }
}