﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmarkAI.POCO
{
        public class Prediction
        {
            public double probability { get; set; }
            public string tagId { get; set; }
            public string tagName { get; set; }
        }

        public class CustomVision
        {
            public string id { get; set; }
            public string project { get; set; }
            public string iteration { get; set; }
            public DateTime created { get; set; }
            public IList<Prediction> predictions { get; set; }
        }
}
