using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class HighScoreData
    {
        public List<string> Names = new();
        public List<int> Scores = new ();
    }
}
