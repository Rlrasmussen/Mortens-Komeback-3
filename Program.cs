﻿using System;
using System.Threading;


namespace Mortens_Komeback_3
{
    static class Program
    {

        private static Mutex thereCanBeOnlyOne;

        static void Main()
        {
            const string mutexName = "uniqueMutex";
            bool isNewInstance;
            thereCanBeOnlyOne = new Mutex(true, mutexName, out isNewInstance);
            if (!isNewInstance)
            {
                return;
            }

            GameWorld.Instance.Run();

            GC.KeepAlive(thereCanBeOnlyOne);
        }
    }

}