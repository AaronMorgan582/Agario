using System;
using System.Drawing;

namespace Model
{
    public class Circle
    {
        private int id_number;
        private PointF position;
        private Color circle_color;
        private string circle_name;
        private int mass;
        private object obj_type;

        private double radius;

        public Circle(PointF position, Color color, int id, int belongs_to, object type, string name, int mass )
        {
            this.position = position;
            circle_color = color;
            id_number = id;
            obj_type = type;
            circle_name = name;
            this.mass = mass;
            radius = (mass / (2 * Math.PI));
        }
                
    }
}
