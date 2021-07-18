using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyGameSpaceInvaders
{
    class Gameplay
    {
        int score = 0;
        int timeCounter = 0;
        public int fieldWight;
        public int fieldHeight;
        public bool isStarted = false; 
        public bool isPaused = false; 
        public bool isFinished = false;
        public bool isWin = false;
        int lvl = 0;
        public Timer timer = new Timer()
        { Interval = 50 };
        public bool shoot = false;
        bool ashoot = false;
        public Player player;
        public Bullet pbullet = new Bullet(-10, -10, 10);
        public Alien specAlien = new Alien(-40, 35, -1);
        bool specAlienIsHere = false;
        int specAlienCounter = 0;
        public Bullet abullet = new Bullet(-10, -10, 5);
        Alien currentShooter = new Alien(-1, -1, -1);
        int alienStepDirection = 1;
        public List<Alien> aliens = new List<Alien>();
        public List<Wall> walls = new List<Wall>();

        public Gameplay(int wight, int height)
        {
            fieldWight = wight;
            fieldHeight = height;
            player = new Player(fieldWight / 2, fieldHeight - 35);
            var y = 70;
            var x = 25;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    aliens.Add(new Alien(x, y, j));
                    x += 40;
                }
                y += 30;
                x = 25;
            }
            y = player.Y - 30;
            x = 30;
            for (var i = 0; i < 5; i++)
            {
                walls.Add(new Wall(x, y));
                x += 100;
            }
        }

        public void ChangeWorldOnTick()
        {
            if (specAlienCounter == 300)
            {
                specAlien.Alive = true;
                specAlienCounter -= 300;
                specAlienIsHere = true;
            }
            if (specAlienIsHere && specAlienCounter % 2 == 0)
                SpecAlienMove();
            if (shoot)
                PlayerShooting();
            if (ashoot)
                AlienShooting();
            if (timeCounter % 8 == 0)
            {
                timeCounter -= 8;
                Console.WriteLine(aliens[1].X);
                AlienStandartMove();
                ashoot = true;
            }
            timeCounter += 1;
            specAlienCounter += 1;
        }

        public void Win()
        {
            isWin = true;
            isStarted = false;
        }

        public void Pause() 
        {
            isPaused = !isPaused;
        }

        public void Start()
        {
            isStarted = true;
            isPaused = false;
        }

        public void StopTimer(Timer timer)
        {
            timer.Stop();
        }

        public void Restart()
        {
            Application.Restart();
        }

        public void NextLvl()
        {
            var h = player.Health;
            timeCounter = 0;
            isStarted = true;
            isPaused = false;
            isFinished = false;
            shoot = false;
            player = new Player(fieldWight / 2, fieldHeight - 35);
            player.Health = h;
            pbullet = new Bullet(-10, -10, 10);
            specAlien = new Alien(-40, 35, -1);
            specAlienIsHere = false;
            specAlienCounter = 0;
            abullet = new Bullet(-10, -10, 5);
            currentShooter = new Alien(-1, -1, -1);
            alienStepDirection = 1;
            aliens = new List<Alien>();
            var y = 70;
            var x = 25;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    aliens.Add(new Alien(x, y, j));
                    x += 40;
                }
                y += 30;
                x = 25;
            }
            lvl++;
        }

        public List<int> FindScoreOrder()
        {
            var a = score;
            var b = new List<int>();
            while (a > 0)
            {
                b.Add(a % 10);
                a = a / 10;
            }
            b.Reverse();
            return b;
        }

        public void AlienStandartMove()
        {
            if (alienStepDirection == 1)
            {
                var maxInd = -1;
                var stepToDo = 0;
                foreach (var a in aliens)
                    if (a.index > maxInd && a.Alive)
                        maxInd = a.index;
                foreach (var a in aliens)
                    if (a.index == maxInd)
                    {
                        if (a.X + 36 < fieldWight)
                            stepToDo = 1;
                        break;
                    }
                if (stepToDo == 1)
                    foreach (var a in aliens)
                        a.Move(1);
                else
                {
                    alienStepDirection *= -1;
                    foreach (var a in aliens)
                    {
                        a.Move(2);
                        if (a.Y >= player.Y - 5)
                            isFinished = true;
                    }
                }
            }
            else
            {
                var minInd = 999;
                var stepToDo = 0;
                foreach (var a in aliens)
                    if (a.index < minInd && a.Alive)
                        minInd = a.index;
                foreach (var a in aliens)
                    if (a.index == minInd)
                    {
                        if (a.X - 4 > 0)
                            stepToDo = 1;
                        break;
                    }
                if (stepToDo == 1)
                    foreach (var a in aliens)
                        a.Move(-1);
                else
                {
                    alienStepDirection *= -1;
                    foreach (var a in aliens)
                    {
                        a.Move(2);
                        if (a.Y >= player.Y - 5)
                            isFinished = true;
                    }
                }
            }
        }

        public void SpecAlienMove()
        {
            if (0 <= specAlien.X && specAlien.X <= fieldWight)
                specAlien.Move(1);
            else if (specAlien.X >= fieldWight - 30)
            {
                specAlienIsHere = false;
                specAlien.X = -40;
            }
            else
                specAlien.X = 0;
        }

        public void PlayerShooting()
        {
            if (!pbullet.isFly)
            {
                pbullet.X = player.X + 13;
                pbullet.Y = player.Y - 5;
                pbullet.isFly = true;
            }
            else if (pbullet.Y > 30)
            {
                if (specAlien.Y <= pbullet.Y
                    && pbullet.Y <= specAlien.Y + 29
                    && specAlien.X - 5 <= pbullet.X
                    && pbullet.X <= specAlien.X + 30)
                {
                    specAlien.Alive = false;
                    score += 100;
                    StopShooting();
                }
                int aliveCount = 0;
                foreach (var a in aliens)
                {
                    if (a.Alive)
                        aliveCount += 1;
                    if (a.Y <= pbullet.Y
                    && pbullet.Y <= a.Y + 30
                    && a.X - 3 <= pbullet.X
                    && pbullet.X <= a.X + 40)
                        if (a.Alive)
                        {
                            a.Alive = false;
                            aliveCount += 1;
                            score += 10;
                            StopShooting();
                        }
                }
                if (aliveCount == 0)
                    Win();
                foreach (var wall in walls)
                    if (wall.Y <= pbullet.Y
                        && pbullet.Y <= wall.dY + 7
                        && wall.X - 7 <= pbullet.X
                        && pbullet.X <= wall.rX)
                    {
                        wall.health--;
                        if (wall.health >= 0)
                            StopShooting();
                        if (wall.health < 0)
                            wall.health = -1;
                    }
                pbullet.Fly(1);
            }
            else
                StopShooting();
        }

        public void StopShooting()
        {
            pbullet.isFly = false;
            shoot = false;
            pbullet.Clear();
        }

        public void TryMove(int direction)
        {
            if (direction == 1 && player.X < fieldWight - 16)
                player.Move(1);
            if (direction == -1 && player.X > 0)
                player.Move(-1);
        }

        public void AlienShooting()
        {
            if (currentShooter.X == -1)
            {
                var potentialShooters = new List<Alien>();
                var indexes = new List<int>();
                for (var i = aliens.Count - 1; i > 0; i--)
                    if (!indexes.Contains(aliens[i].index) && aliens[i].Alive)
                    {
                        potentialShooters.Add(aliens[i]);
                        indexes.Add(aliens[i].index);
                    }
                var rnd = new Random();
                var randInd = 0;
                if (potentialShooters.Count > 1)
                {
                    randInd = rnd.Next(0, potentialShooters.Count - 1);
                    currentShooter = potentialShooters[randInd];
                }
                else if (potentialShooters.Count == 1)
                    currentShooter = potentialShooters.First();
                else
                    return;
            }
            if (!abullet.isFly)
            {
                abullet.X = currentShooter.X + 18;
                abullet.Y = currentShooter.Y + 24;
                abullet.isFly = true;
            }
            else if (abullet.Y < fieldHeight - 3)
            {
                if (player.Y - 5 <= abullet.Y
                    && abullet.Y <= player.Y + 30
                    && player.X - 8 <= abullet.X
                    && abullet.X <= player.X + 36)
                {
                    player.Health--;
                    StopAlienShooting();
                    if (player.Health < 0)
                        isFinished = true;
                }
                foreach (var wall in walls)
                {
                    if (wall.Y - 7 <= abullet.Y
                        && abullet.Y <= wall.dY
                        && wall.X - 7 <= abullet.X
                        && abullet.X <= wall.rX)
                    {
                        wall.health--;
                        if (wall.health >= 0)
                            StopAlienShooting();
                        if (wall.health < 0)
                            wall.health = -1;
                    }
                }
                abullet.Fly(-1);
            }
            else
                StopAlienShooting();

        }

        public void StopAlienShooting()
        {
            abullet.isFly = false;
            ashoot = false;
            abullet.Clear();
            currentShooter = new Alien(-1, -1, -1);
        }

        public void CheckIsWin()
        {
            foreach (var a in aliens)
                if (a.Alive)
                    return;
            Win();
        }
    }
}