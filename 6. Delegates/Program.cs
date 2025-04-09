using System;

namespace Delegates
{

    public delegate void LevelUpEventHandler(LevelUpEventArgs args);  //Delegate definiton

    public class LevelUpEventArgs : EventArgs
    {
        public int NewLevel { get; }
        public int XP { get; }

        public LevelUpEventArgs(int newLevel, int xp)
        {
            NewLevel = newLevel;
            XP = xp;
        }
    }

    public class Player
    {
        private int _xp;
        private int _level;

        public event LevelUpEventHandler? LevelUpEvent;

        public int XP => _xp;
        public int Level => _level;

        public void GainXP(int amount)
        {
            _xp += amount;
            Console.WriteLine($"Gained {amount} XP. Total XP: {_xp}");

            CheckLevelUp();
        }

        private void CheckLevelUp()
        {

            int newLevel = _xp / 100;
            if (newLevel > _level)
            {
                _level = newLevel;
                OnLevelUp(new LevelUpEventArgs(_level, _xp));
            }
        }


        protected void OnLevelUp(LevelUpEventArgs args)
        {
            LevelUpEvent?.Invoke(args);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Player player = new Player();

            player.LevelUpEvent += PlayerLevelUp;


            for (int i = 0; i < 5; i++) 
            {
                player.GainXP(50); 
            }
        }


        private static void PlayerLevelUp(LevelUpEventArgs args)
        {
            Console.WriteLine($"Congratulations! You've reached level {args.NewLevel} with {args.XP} XP!");
        }
    }
}
