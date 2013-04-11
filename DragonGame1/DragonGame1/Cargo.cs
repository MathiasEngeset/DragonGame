using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonGame1
{
    [Serializable]
    public class Cargo
    {
        //Class contains alle objects needed for saving and loading.
        //***************************************************************
        public Knight mKnightSprite { get; set; }
        public Knight mKnightSprite2 { get; set; }
        public dragon mdragonSprite { get; set; }
        public dragon mdragonSprite2 { get; set; }
        public dragon mdragonSprite3 { get; set; }
        public List<GoldCoin> goldCoinList { get; set; }
        public float countdownStartime { get; set; }
    }
}
