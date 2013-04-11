using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonGame1
{
    [Serializable]
    public class Cargo
    {
        //Klasse som samler alle spill objectene og lagrer dem til fil.
        //***************************************************************
        public Knight mKnightSprite { get; set; }
        public dragon mdragonSprite { get; set; }
        public dragon mdragonSprite2 { get; set; }
        public dragon mdragonSprite3 { get; set; }
        public List<GoldCoin> goldCoinList { get; set; }
    }
}
