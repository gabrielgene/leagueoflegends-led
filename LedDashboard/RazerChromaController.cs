﻿using RazerChroma.Net;
using RazerChromaFrameEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LedDashboard
{
    class RazerChromaController : LightController
    {
        public static RazerChromaController Create()
        {
            return new RazerChromaController();
        }

        private KeyboradFrame keyboardFrame;
        private NativeRazerApi api;

        private RazerChromaController() // TODO: Dispose afterr a while if no data is received
        {
            Init();
        }

        private void Init()
        {
            try
            {
                if (api == null) api = new NativeRazerApi();
                if (keyboardFrame == null) keyboardFrame = new KeyboradFrame(api);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error initializing Razer Chroma controller. Is Razer Synapse installed?");
                throw new InvalidOperationException("Error initializing Razer Chroma controller. Is Razer Synapse installed?", e);
            }
        }

        public void SendData(int ledCount, byte[] colorArray)
        {
            if (!enabled) return;
            List<Point> points = new List<Point>();
            for (int i = 0; i < ledCount; i++)
            {
                Color c = Color.FromArgb(colorArray[i * 3], colorArray[i * 3 + 1], colorArray[i * 3 + 2]);
                int x = (int)Utils.Scale(i, 0, ledCount, 0, 22);
                points.Clear();
                points.Add(new Point(x, 0));
                points.Add(new Point(x, 1));
                points.Add(new Point(x, 2));
                points.Add(new Point(x, 3));
                points.Add(new Point(x, 4));
                points.Add(new Point(x, 5));
                keyboardFrame.SetKeys(points, c);
            }
            keyboardFrame.Update();
            
        }

        bool enabled = true;
        public void SetEnabled(bool enabled)
        {
            if (this.enabled == enabled) return;
            if(!enabled)
            {
                if (api != null) api.Dispose();
                api = null;
                keyboardFrame = null;
            } else
            {
                Init();
            }
            this.enabled = enabled;
        }

        public void Dispose()
        {
            if (api != null) api.Dispose();
            api = null;
            keyboardFrame = null;
        }

        public bool IsEnabled()
        {
            return enabled;
        }
    }
}