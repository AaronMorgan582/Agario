using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class World
    {
        protected const int HEIGHT = 5_000;
        protected int WIDTH = 5_000;
        private Dictionary<int, Circle> game_objects;

        public World(ILogger logger)
        {
            game_objects = new Dictionary<int, Circle>();
            logger.LogInformation("List of Circles created.");
        }

        public Dictionary<int, Circle> GetDictionary { get => game_objects; }

        public IEnumerable<int> KeyCollection(){
            Dictionary<int, Circle>.KeyCollection keys = game_objects.Keys;
            return keys;
        }
    }
}
