using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Model
{
    public class Circle
    {
        public enum object_type { food, player, heartbeat, admin }

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

        private float radius;

        /// <summary>
        /// Constructor for the Circle class to be created with the 
        /// information from the server
        /// </summary>
        /// <param name="loc">an x,y coordinate for Circle's location</param>
        /// <param name="argb_color">an int representing the Circle's color</param>
        /// <param name="id">an int representing the Circle's id</param>
        /// <param name="belongs_to">an int representing the Circle's parent to be used for split function</param>
        /// <param name="type">an object_type of food, player, heartbeat, or admin</param>
        /// <param name="Name">a string representing a player's name</param>
        /// <param name="Mass">a float representing the area of a circle</param>
        public Circle(PointF loc, int argb_color, int id, int belongs_to, object_type type, string Name, float Mass)
        {
            this.loc = loc;
            this.argb_color = argb_color;
            this.id = id;
            this.belongs_to = belongs_to;
            this.type = type;
            this.Name = Name;
            this.Mass = Mass;

            radius = (float) Math.Sqrt(Mass / (Math.PI));
        }

        public static void Main(string[] args)
        {
            string message = "{ \"loc\":{ \"X\":1768.0,\"Y\":320.0},\"argb_color\":-2445240,\"id\":0,\"belongs_to\":0,\"type\":0,\"Name\":\"Player\",\"Mass\":10.0}";

            Circle testCircle = JsonConvert.DeserializeObject<Circle>(message);

            Console.WriteLine(testCircle);
        }

        #region Getters/Setters for the Circle class

        public float Radius { get => radius; set => radius = value; }

        public int CircleColor { get => argb_color; }

        public PointF Location { get => loc; }

        public int ID { get => id; }

        public object_type Type { get => type; }

        public string GetName { get => this.Name; }

        public float GetMass { get => this.Mass; set => this.Mass = value; }

        public int BelongsTo { get => this.belongs_to; }

        #endregion
    }
}
