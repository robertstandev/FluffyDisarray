using System;
using System.Collections.Generic;
using UnityEngine;


namespace RobertStan.ColliderUpdater
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteInEditMode]
    public class ColliderUpdater : MonoBehaviour
    {
        [SerializeField][Range(0, 254)]private byte AlphaTolerance = 20;
        private byte lastAlphaTolerance;

        [SerializeField][Range(0, 64)]private int DistanceThreshold = 1;
        private int lastDistanceThreshold;

        [SerializeField][Range(0.5f, 2.0f)]private float Scale = 1.0f;
        private float lastScale;

        private SpriteRenderer spriteRenderer;
        private PolygonCollider2D polygonCollider;
        private bool dirty;
        private Sprite lastSprite;
        private Rect lastRect = new Rect();
        private Vector2 lastOffset = new Vector2(-99999.0f, -99999.0f);
        private float lastPixelsPerUnit;
        private bool lastFlipX, lastFlipY;
        private readonly TextureConverter geometryDetector = new TextureConverter();
        private void Start()
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            UpdateDirtyState();
            if (dirty)
            {
                RecalculatePolygon();
            }
        }

        private void UpdateDirtyState()
        {
            if (spriteRenderer.sprite != lastSprite)
            {
                lastSprite = spriteRenderer.sprite;
                dirty = true;
            }
            if (spriteRenderer.sprite != null)
            {
                if (lastOffset != spriteRenderer.sprite.pivot)
                {
                    lastOffset = spriteRenderer.sprite.pivot;
                    dirty = true;
                }
                if (lastRect != spriteRenderer.sprite.rect)
                {
                    lastRect = spriteRenderer.sprite.rect;
                    dirty = true;
                }
                if (lastPixelsPerUnit != spriteRenderer.sprite.pixelsPerUnit)
                {
                    lastPixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
                    dirty = true;
                }
                if (lastFlipX != spriteRenderer.flipX)
                {
                    lastFlipX = spriteRenderer.flipX;
                    dirty = true;
                }
                if (lastFlipY != spriteRenderer.flipY)
                {
                    lastFlipY = spriteRenderer.flipY;
                    dirty = true;
                }
            }
            if (AlphaTolerance != lastAlphaTolerance)
            {
                lastAlphaTolerance = AlphaTolerance;
                dirty = true;
            }
            if (Scale != lastScale)
            {
                lastScale = Scale;
                dirty = true;
            }
            if (DistanceThreshold != lastDistanceThreshold)
            {
                lastDistanceThreshold = DistanceThreshold;
                dirty = true;
            }
        }
        public void RecalculatePolygon()
        {
            if (spriteRenderer.sprite != null)
            {
                ColliderData cd = new ColliderData();
                cd.AlphaTolerance = AlphaTolerance;
                cd.DistanceThreshold = DistanceThreshold;
                cd.Rect = spriteRenderer.sprite.rect;
                cd.Offset = spriteRenderer.sprite.pivot;
                cd.Texture = spriteRenderer.sprite.texture;
                cd.XMultiplier = (spriteRenderer.sprite.rect.width * 0.5f) / spriteRenderer.sprite.pixelsPerUnit;
                cd.YMultiplier = (spriteRenderer.sprite.rect.height * 0.5f) / spriteRenderer.sprite.pixelsPerUnit;
                UpdatePolygonCollider(ref cd);
            }
        }

        public void UpdatePolygonCollider(ref ColliderData cd)
        {
            if (spriteRenderer.sprite == null || spriteRenderer.sprite.texture == null)
            {
                return;
            }

            dirty = false;
            PopulateCollider(polygonCollider, ref cd);
        }

        public void PopulateCollider(PolygonCollider2D collider, ref ColliderData cd)
        {
            try
            {
                int width = (int)cd.Rect.width;
                int height = (int)cd.Rect.height;
                int x = (int)cd.Rect.x;
                int y = (int)cd.Rect.y;
                UnityEngine.Color[] pixels = cd.Texture.GetPixels(x, y, width, height, 0);
                List<Vertices> verts = geometryDetector.DetectVertices(pixels, width, cd.AlphaTolerance);
                int pathIndex = 0;
                List<Vector2[]> list = new List<Vector2[]>();

                for (int i = 0; i < verts.Count; i++)
                {
                    ProcessVertices(collider, verts[i], list, ref cd, ref pathIndex);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error creating collider: " + ex);
            }
        }

        private List<Vector2[]> ProcessVertices(PolygonCollider2D collider, Vertices v, List<Vector2[]> list, ref ColliderData cd, ref int pathIndex)
        {
            Vector2 offset = cd.Offset;
            float flipXMultiplier = (spriteRenderer.flipX ? -1.0f : 1.0f);
            float flipYMultiplier = (spriteRenderer.flipY ? -1.0f : 1.0f);

			if (cd.DistanceThreshold > 1)
			{
				v = SimplifyTools.DouglasPeuckerSimplify (v, cd.DistanceThreshold);
			}
                collider.pathCount = pathIndex + 1;
                for (int i = 0; i < v.Count; i++)
                {
					float xValue = (2.0f * (((v[i].x - offset.x) + 0.5f) / cd.Rect.width));
					float yValue = (2.0f * (((v[i].y - offset.y) + 0.5f) / cd.Rect.height));
                    v[i] = new Vector2(xValue * cd.XMultiplier * Scale * flipXMultiplier, yValue * cd.YMultiplier * Scale * flipYMultiplier);
                }
                Vector2[] arr = v.ToArray();
                collider.SetPath(pathIndex++, arr);
                list.Add(arr);
            return list;
        }
    }
    public struct ColliderData
    {
        public Texture2D Texture;

        public Rect Rect;

        public Vector2 Offset;

        public float XMultiplier;
        public float YMultiplier;

        public byte AlphaTolerance;

        public int DistanceThreshold;

        public override int GetHashCode()
        {
            int h = Texture.GetHashCode();
            if (h == 0)
            {
                h = 1;
            }
            return h * (int)(Rect.GetHashCode() * XMultiplier * YMultiplier * AlphaTolerance * Mathf.Max(DistanceThreshold, 1));
        }

        public override bool Equals(object obj)
        {
            if (obj is ColliderData)
            {
                ColliderData cd = (ColliderData)obj;
                return Texture == cd.Texture && Rect == cd.Rect &&
                    XMultiplier == cd.XMultiplier && YMultiplier == cd.YMultiplier &&
                    AlphaTolerance == cd.AlphaTolerance && DistanceThreshold == cd.DistanceThreshold;
            }
            return false;
        }
    }
}