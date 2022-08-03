using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Nevelson.Utils
{
    public class ExtGameObject_Test
    {
        [Test]
        public void Test_TryGetComponents()
        {
            GameObject go = new GameObject();
            GameObject goChild = new GameObject();
            goChild.transform.parent = go.transform;
            go.AddComponent<MockComponent>();
            go.AddComponent<MockComponent>();
            goChild.AddComponent<MockComponent>();

            bool success = go.TryGetComponents(out MockComponent[] mock);
            Assert.True(success);

            Assert.NotNull(mock);
            Assert.NotNull(mock[0]);
            Assert.NotNull(mock[1]);
            Assert.AreEqual(2, mock.Length);

            GameObject.DestroyImmediate(mock[0]);
            GameObject.DestroyImmediate(mock[1]);

            success = go.TryGetComponents(out mock);
            Assert.False(success);
            Assert.AreEqual(0, mock.Length);

            GameObject.DestroyImmediate(go);
            GameObject.DestroyImmediate(goChild);
        }

        [Test]
        public void Test_TryGetComponentsInChildren()
        {
            GameObject go = new GameObject();
            GameObject goChild = new GameObject();
            goChild.transform.parent = go.transform;
            go.AddComponent<MockComponent>();
            goChild.AddComponent<MockComponent>();

            bool success = go.TryGetComponentsInChildren(out MockComponent[] mock);
            Assert.True(success);

            bool success2 = goChild.TryGetComponentsInChildren(out MockComponent[] mock2);
            Assert.True(success2);

            Assert.NotNull(mock);
            Assert.NotNull(mock2);
            Assert.NotNull(mock[0]);
            Assert.NotNull(mock[1]);
            Assert.AreEqual(2, mock.Length);
            Assert.AreEqual(1, mock2.Length); //cause it's from the child


            GameObject.DestroyImmediate(mock[0]);

            bool success3 = go.TryGetComponentsInChildren(out MockComponent[] mock3);
            Assert.True(success3);

            bool success4 = goChild.TryGetComponentsInChildren(out MockComponent[] mock4);
            Assert.True(success4);

            Assert.AreEqual(1, mock3.Length);
            Assert.AreEqual(1, mock4.Length);
            Assert.NotNull(mock3);
            Assert.NotNull(mock4);

            GameObject.DestroyImmediate(mock[1]);

            bool success5 = go.TryGetComponentsInChildren(out MockComponent[] mock5);
            Assert.False(success5);

            bool success6 = goChild.TryGetComponentsInChildren(out MockComponent[] mock6);
            Assert.False(success6);
            Assert.AreEqual(0, mock5.Length);
            Assert.AreEqual(0, mock6.Length);

            GameObject.DestroyImmediate(go);
            GameObject.DestroyImmediate(goChild);
        }

        [Test]
        public void Test_TryGetComponentInChildren()
        {
            GameObject go = new GameObject();
            GameObject goChild = new GameObject();
            goChild.transform.parent = go.transform;
            goChild.AddComponent<MockComponent>();

            bool success = go.TryGetComponentInChildren(out MockComponent mock);
            Assert.True(success);

            bool success2 = goChild.TryGetComponentInChildren(out MockComponent mock2);
            Assert.True(success2);

            Assert.NotNull(mock);
            Assert.NotNull(mock2);
            Assert.AreEqual(mock, mock2);

            GameObject.DestroyImmediate(mock);
            bool success3 = go.TryGetComponentInChildren(out MockComponent mock3);
            Assert.False(success3);

            bool success4 = goChild.TryGetComponentInChildren(out MockComponent mock4);
            Assert.False(success4);

            Assert.Null(mock3);
            Assert.Null(mock4);

            GameObject.DestroyImmediate(go);
            GameObject.DestroyImmediate(goChild);
        }

        [Test]
        public void Test_AddCopiedComponent()
        {
            GameObject go = new GameObject();
            Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = -12;
            rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
            rb.velocity = new Vector2(345, 913);

            GameObject destinationGO = new GameObject();

            //Test returned component
            Rigidbody2D myDestinationRB = destinationGO.AddCopiedComponent(go.GetComponent<Rigidbody2D>());
            Assert.AreEqual(-12, myDestinationRB.gravityScale);
            Assert.AreEqual(RigidbodySleepMode2D.NeverSleep, myDestinationRB.sleepMode);
            Assert.AreEqual(new Vector2(345, 913), myDestinationRB.velocity);

            //Test attached component
            Rigidbody2D attachedRB = destinationGO.GetComponent<Rigidbody2D>();
            Assert.AreEqual(-12, attachedRB.gravityScale);
            Assert.AreEqual(RigidbodySleepMode2D.NeverSleep, attachedRB.sleepMode);
            Assert.AreEqual(new Vector2(345, 913), attachedRB.velocity);

            GameObject.DestroyImmediate(go);
            GameObject.DestroyImmediate(destinationGO);
        }

        [Test]
        public void Test_GetClosest_GameObject()
        {
            GameObject go = new GameObject();
            go.transform.Position2D(Vector2.zero);

            //Selects closest disregarding Zed
            GameObject go1 = new GameObject();
            go1.transform.Position2D(Vector2.left);
            GameObject go2 = new GameObject();
            go2.transform.Position2D(Vector2.right);
            GameObject go3 = new GameObject();
            go3.transform.Position2D(Vector2.left * 2);
            GameObject go4 = new GameObject();
            go4.transform.Position2D(Vector2.left * 3);
            GameObject go5 = new GameObject();
            go5.transform.Position2D(new Vector3(.5f, .2f, 10000f));
            List<GameObject> GameObjects = new List<GameObject>()
            {
               go1,
               go2,
               go3,
               go4,
               go5,
            };
            GameObject closest = go.GetClosest(GameObjects);
            Assert.AreEqual(go5, closest);

            //Selects first on list if all equal
            GameObject goEqual1 = new GameObject();
            goEqual1.transform.Position2D(Vector2.left);
            GameObject goEqual2 = new GameObject();
            goEqual2.transform.Position2D(Vector2.left);
            GameObjects.Clear();
            GameObjects = new List<GameObject>()
            {
               goEqual1,
               goEqual2,
            };
            closest = go.GetClosest(GameObjects);
            Assert.AreEqual(goEqual1, closest);

            //returns null if list empty
            GameObjects.Clear();
            closest = go.GetClosest(GameObjects);
            Assert.Null(closest);
        }

        [Test]
        public void Test_GetClosest_Transform()
        {
            GameObject go = new GameObject();
            go.transform.Position2D(Vector2.zero);

            //Selects closest disregarding Zed
            GameObject go1 = new GameObject();
            go1.transform.Position2D(Vector2.left);
            GameObject go2 = new GameObject();
            go2.transform.Position2D(Vector2.right);
            GameObject go3 = new GameObject();
            go3.transform.Position2D(Vector2.left * 2);
            GameObject go4 = new GameObject();
            go4.transform.Position2D(Vector2.left * 3);
            GameObject go5 = new GameObject();
            go5.transform.Position2D(new Vector3(.5f, .2f, 10000f));
            List<Transform> Transforms = new List<Transform>()
            {
               go1.transform,
               go2.transform,
               go3.transform,
               go4.transform,
               go5.transform,
            };
            Transform closest = go.GetClosest(Transforms);
            Assert.AreEqual(go5.transform, closest);

            //Selects first on list if all equal
            GameObject transEqual1 = new GameObject();
            transEqual1.transform.Position2D(Vector2.left);
            GameObject transEqual2 = new GameObject();
            transEqual2.transform.Position2D(Vector2.left);
            Transforms.Clear();
            Transforms = new List<Transform>()
            {
               transEqual1.transform,
               transEqual2.transform,
            };
            closest = go.GetClosest(Transforms);
            Assert.AreEqual(transEqual1.transform, closest);

            //returns null if list empty
            Transforms.Clear();
            closest = go.GetClosest(Transforms);
            Assert.Null(closest);
        }

        [Test]
        public void Test_GetClosest_Vector()
        {
            GameObject go = new GameObject();
            go.transform.Position2D(Vector2.zero);

            //Selects closest disregarding Zed
            Vector2 v1 = Vector2.left;
            Vector2 v2 = Vector2.right;
            Vector2 v3 = Vector2.left * 2;
            Vector2 v4 = Vector2.left * 3;
            Vector2 v5 = new Vector3(.5f, .2f, 10000f);
            List<Vector2> Vectors = new List<Vector2>()
            {
               v1,
               v2,
               v3,
               v4,
               v5,
            };
            Vector2 closest = go.GetClosest(Vectors);
            Assert.AreEqual(v5, closest);

            //Selects first on list if all equal
            Vector2 vEqual1 = Vector2.left;
            Vector2 vEqual2 = Vector2.left;
            Vectors.Clear();
            Vectors = new List<Vector2>()
            {
               vEqual1,
               vEqual2,
            };
            closest = go.GetClosest(Vectors);
            Assert.AreEqual(vEqual1, closest);

            //returns Vector2.zero if list empty
            Vectors.Clear();
            closest = go.GetClosest(Vectors);
            Assert.AreEqual(Vector2.zero, closest);
        }
    }
}