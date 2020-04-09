﻿using Microsoft.Extensions.Logging;
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
        private Dictionary<int, Circle> circle_list;


        public World(ILogger logger)
        {
            circle_list = new Dictionary<int, Circle>();
            logger.LogInformation("List of Circles created.");
        }
    }
}
