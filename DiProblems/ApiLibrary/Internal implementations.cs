﻿using System;

namespace ApiLibrary
{
    interface IMotor
    {
        void Start(ISpeed speed);
        void Stop();
    }

    internal class Motor : IMotor
    {
        public void Start(ISpeed speed)
        {
            Console.WriteLine("Starting engine with " + speed.KilometeresPerHour + " km/h");
        }
        public void Stop() { }
    }

    internal class Speed : ISpeed
    {
        public int KilometeresPerHour { get; set; }
    }
}