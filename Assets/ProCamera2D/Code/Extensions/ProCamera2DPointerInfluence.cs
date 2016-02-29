﻿using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
    public class ProCamera2DPointerInfluence : BasePC2D
    {
        public static string ExtensionName = "Pointer Influence";

        public float MaxHorizontalInfluence = 3f;
        public float MaxVerticalInfluence = 2f;

        public float InfluenceSmoothness = .2f;

        Vector2 _influence;
        Vector2 _velocity;

        override public void OnReset()
        {
            _influence = Vector2.zero;
            _velocity = Vector2.zero;
        }

        override protected void OnPreMoveUpdate(float deltaTime)
        {
            ApplyInfluence();
        }

        void ApplyInfluence()
        {
            var mousePosViewport = ProCamera2D.GameCamera.ScreenToViewportPoint(Input.mousePosition);

            var mousePosViewportH = mousePosViewport.x.Remap(0, 1, -1, 1);
            var mousePosViewportV = mousePosViewport.y.Remap(0, 1, -1, 1);

            var hInfluence = mousePosViewportH * MaxHorizontalInfluence;
            var vInfluence = mousePosViewportV * MaxVerticalInfluence;

            _influence = Vector2.SmoothDamp(_influence, new Vector2(hInfluence, vInfluence), ref _velocity, InfluenceSmoothness);

            ProCamera2D.ApplyInfluence(_influence);
        }
    }
}
