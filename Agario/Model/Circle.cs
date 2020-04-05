using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Model
{
    public class Circle
    {
        [JsonProperty]
        private int id_number;
        [JsonProperty]
        private PointF position;
        [JsonProperty]
        private Color circle_color;
        [JsonProperty]
        private string circle_name;
        [JsonProperty]
        private int mass;
        [JsonProperty]
        private object obj_type;

        private double radius;

        //{"loc":{"X":1768.0,"Y":320.0},"argb_color":-2445240,"id":0,"belongs_to":0,"type":0,"Name":"","Mass":10.0}

        //public Circle(PointF loc, Color argb_color, int id, int belongs_to, object type, string Name, int Mass)
        public Circle(string loc, string argb_color, string id, string belongs_to, string type, string Name, string Mass)
        {
            //this.position = loc;
            //circle_color = argb_color;
            //id_number = id;
            //obj_type = type;
            //circle_name = Name;
            //this.mass = Mass;
            //radius = (mass / (2 * Math.PI));
        }

    }
}
