﻿using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Model
{
    public class Circle
    {
        public enum object_type {food, player, heartbeat, admin}

        [JsonProperty]
        private PointF loc;

        [JsonProperty]
        private int argb_color;

        [JsonProperty]
        private int id;

        [JsonProperty]
        private int belongs_to;

        [JsonProperty]
        private object_type type;

        [JsonProperty]
        private string Name;

        [JsonProperty]
        private float Mass;

        private double radius;

        public Circle(PointF loc, int argb_color, int id, int belongs_to, object_type type, string Name, float Mass)
        {
            this.loc = loc;
            this.argb_color = argb_color;
            this.id = id;
            this.belongs_to = belongs_to;
            this.type = type;
            this.Name = Name;
            this.Mass = Mass;

            radius = (Mass / (2 * Math.PI));
        }

        public static void Main(string[] args)
        {
            string message = "{ \"loc\":{ \"X\":1768.0,\"Y\":320.0},\"argb_color\":-2445240,\"id\":0,\"belongs_to\":0,\"type\":0,\"Name\":\"Player\",\"Mass\":10.0}";

            Circle testCircle = JsonConvert.DeserializeObject<Circle>(message);

            Console.WriteLine(testCircle);
        }

        public double Radius { get => radius; }

        public int CircleColor { get => argb_color; }
    }
}
