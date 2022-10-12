using UnityEngine;

namespace Nevelson.Utils
{
    public static class ExtCamera
    {
        /// <summary>
        /// Returns the UI Position of a world point on the selected camera
        /// </summary>
        /// <param name="worldCamera"></param>
        /// <param name="canvas"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vector2 WorldPositionToScreenSpaceCameraPosition(this Camera worldCamera, Canvas canvas, Vector3 position)
        {
            Vector2 viewport = worldCamera.WorldToViewportPoint(position);
            Ray canvasRay = canvas.worldCamera.ViewportPointToRay(viewport);
            return canvasRay.GetPoint(canvas.planeDistance);
        }
    }
}