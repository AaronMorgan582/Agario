using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class World
    {
        private const int HEIGHT = 5_000;
        private const int WIDTH = 5_000;
        private Dictionary<int, Circle> game_objects;
        private ILogger logger;

        public World(ILogger logger)
        {
            game_objects = new Dictionary<int, Circle>();
            this.logger = logger;
        }

        public int Width { get => WIDTH; }
        public int Height { get => HEIGHT; }

        public void Add(int ID, Circle circle)
        {
            logger.LogInformation($"Adding object with id: {ID}");
            game_objects.Add(ID, circle);
        }

        public void Remove(int ID)
        {
            logger.LogInformation($"Removing object with id: {ID}");
            game_objects.Remove(ID);
        }

        public bool Contains(int ID)
        {
            return game_objects.ContainsKey(ID);
        }

        public IEnumerable<int> Keys()
        {
            Dictionary<int, Circle>.KeyCollection id_list = game_objects.Keys;

            return id_list;
        }

        public Circle this[int ID]
        {
            get
            {
                if (game_objects.ContainsKey(ID))
                {
                    return game_objects[ID];
                }
                else
                {
                    throw new ArgumentException();
                }
            }

        }

        public IEnumerable Get_Game_Objects()
        {
            logger.LogInformation($"Object list being accessed.");

            HashSet<Circle> game_objects_list = new HashSet<Circle>();

            foreach(int ID in game_objects.Keys)
            {
                game_objects_list.Add(game_objects[ID]);
            }

            return game_objects_list;
        }


    }
}
