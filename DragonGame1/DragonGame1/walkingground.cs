using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DragonGame1
{
    [Serializable]
    public class walkingground
    {
        public Vector2 Position = new Vector2(0, 0);
        [field:NonSerialized]
        private Texture2D SpriteTexture;

        public walkingground() { }

        public walkingground(string assetname, ContentManager theContentManager, float x, float y)
        {
            Position = new Vector2(x, y);
            LoadContent(theContentManager, assetname);

        }

        //load methode
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            SpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
        
        }

        //draw sprite to the screen
        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(SpriteTexture, Position, Color.White); 
        }

    }//end class
}
