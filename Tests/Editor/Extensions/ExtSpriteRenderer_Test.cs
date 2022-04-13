using NUnit.Framework;
using UnityEngine;

namespace Nevelson.Utils
{
    public class ExtSpriteRenderer_Test
    {
        [Test]
        public void Test_OrientZeroPreferRight()
        {
            GameObject go = new GameObject("Object");
            go.transform.position = Vector3.zero;
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            Assert.False(spriteRenderer.flipX);

            spriteRenderer.OrientZeroPreferRight(Vector2.zero);
            Assert.False(spriteRenderer.flipX);

            spriteRenderer.OrientZeroPreferRight(Vector2.right);
            Assert.False(spriteRenderer.flipX);

            spriteRenderer.OrientZeroPreferRight(Vector2.left);
            Assert.True(spriteRenderer.flipX);
            GameObject.DestroyImmediate(go);
        }

        [Test]
        public void Test_OrientNoDirZeroPreference()
        {
            GameObject go = new GameObject("Object");
            go.transform.position = Vector3.zero;
            SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
            Assert.False(spriteRenderer.flipX);

            spriteRenderer.OrientNoDirZeroPreference(Vector2.zero);
            Assert.False(spriteRenderer.flipX);

            spriteRenderer.OrientNoDirZeroPreference(Vector2.right);
            Assert.False(spriteRenderer.flipX);

            spriteRenderer.OrientNoDirZeroPreference(Vector2.left);
            Assert.True(spriteRenderer.flipX);

            spriteRenderer.OrientNoDirZeroPreference(Vector2.zero);
            Assert.True(spriteRenderer.flipX);
            GameObject.DestroyImmediate(go);
        }
    }
}