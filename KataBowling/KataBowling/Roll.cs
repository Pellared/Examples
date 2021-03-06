﻿namespace KataBowling
{
    public class Roll
    {
        public Roll(int points, int bonusRollCount)
        {
            Points = points;
            BonusRollCount = bonusRollCount;
        }

        public Roll(int points)
            : this(points, 0)
        {
        }

        public int Points { get; private set; }

        public int BonusRollCount { get; private set; }

        public bool IsStrike
        {
            get
            {
                return Points == 10;
            }
        }
    }
}
