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

        private Texture2D spriteTexture;
        private Rect rectData;
        private Vector2 offsetData;
        private float xMultiplier, yMultiplier;
        
        private void Start()
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Update()
        {
            updateDirtyState();
            if (this.dirty)
            {
                recalculatePolygon();
            }
        }

        private void updateDirtyState()
        {
            if (spriteRenderer.sprite != this.lastSprite)
            {
                this.lastSprite = spriteRenderer.sprite;
                this.dirty = true;
            }
            if (spriteRenderer.sprite != null)
            {
                if (this.lastOffset != spriteRenderer.sprite.pivot)
                {
                    this.lastOffset = spriteRenderer.sprite.pivot;
                    this.dirty = true;
                }
                if (this.lastRect != spriteRenderer.sprite.rect)
                {
                    this.lastRect = spriteRenderer.sprite.rect;
                    this.dirty = true;
                }
                if (this.lastPixelsPerUnit != spriteRenderer.sprite.pixelsPerUnit)
                {
                    this.lastPixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
                    this.dirty = true;
                }
                if (this.lastFlipX != spriteRenderer.flipX)
                {
                    this.lastFlipX = spriteRenderer.flipX;
                    this.dirty = true;
                }
                if (this.lastFlipY != spriteRenderer.flipY)
                {
                    this.lastFlipY = spriteRenderer.flipY;
                    this.dirty = true;
                }
            }
            if (this.AlphaTolerance != this.lastAlphaTolerance)
            {
                this.lastAlphaTolerance = this.AlphaTolerance;
                this.dirty = true;
            }
            if (this.Scale != this.lastScale)
            {
                this.lastScale = Scale;
                this.dirty = true;
            }
            if (this.DistanceThreshold != this.lastDistanceThreshold)
            {
                this.lastDistanceThreshold = this.DistanceThreshold;
                this.dirty = true;
            }
        }
        public void recalculatePolygon()
        {
            if (spriteRenderer.sprite != null)
            {
                this.rectData = spriteRenderer.sprite.rect;
                this.offsetData = spriteRenderer.sprite.pivot;
                this.spriteTexture = spriteRenderer.sprite.texture;
                this.xMultiplier = (spriteRenderer.sprite.rect.width * 0.5f) / spriteRenderer.sprite.pixelsPerUnit;
                this.yMultiplier = (spriteRenderer.sprite.rect.height * 0.5f) / spriteRenderer.sprite.pixelsPerUnit;
                updatePolygonCollider();
            }
        }

        public void updatePolygonCollider()
        {
            if (spriteRenderer.sprite == null || spriteRenderer.sprite.texture == null)
            {
                return;
            }

            this.dirty = false;
            populateCollider(polygonCollider);
        }

        public void populateCollider(PolygonCollider2D collider)
        {
            try
            {
                int width = (int)this.rectData.width;
                int height = (int)this.rectData.height;
                UnityEngine.Color[] pixels = this.spriteTexture.GetPixels((int)this.rectData.x, (int)this.rectData.y, width, height, 0);
                List<Vertices> verts = this.geometryDetector.DetectVertices(pixels, width, this.AlphaTolerance);
                List<Vector2[]> list = new List<Vector2[]>();

                int pathIndex = 0;
                for (int i = 0; i < verts.Count; i++)
                {
                    processVertices(collider, verts[i], list, ref pathIndex);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error creating collider: " + ex);
            }
        }

        private List<Vector2[]> processVertices(PolygonCollider2D collider, Vertices v, List<Vector2[]> list, ref int pathIndex)
        {
            float flipXMultiplier = (spriteRenderer.flipX ? -1.0f : 1.0f);
            float flipYMultiplier = (spriteRenderer.flipY ? -1.0f : 1.0f);

			if (this.DistanceThreshold > 1)
			{
				v = SimplifyTools.DouglasPeuckerSimplify (v, this.DistanceThreshold);
			}
            
            collider.pathCount = pathIndex + 1;
            for (int i = 0; i < v.Count; i++)
            {
				float xValue = (2.0f * (((v[i].x - this.offsetData.x) + 0.5f) / this.rectData.width));
				float yValue = (2.0f * (((v[i].y - this.offsetData.y) + 0.5f) / this.rectData.height));
                v[i] = new Vector2(xValue * this.xMultiplier * this.Scale * flipXMultiplier, yValue * this.yMultiplier * Scale * flipYMultiplier);
            }
            
            Vector2[] arr = v.ToArray();
            collider.SetPath(pathIndex++, arr);
            list.Add(arr);

            return list;
        }
    }
}