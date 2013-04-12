using System;

namespace DragonGame1
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (DragonGame1 game = new DragonGame1())
            {
                game.Run();
            }
        }
    }
#endif
}

