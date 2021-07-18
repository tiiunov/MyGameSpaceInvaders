using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MyGameSpaceInvaders
{
    [TestFixture]
    class Tests
    {
        [Test]
        public void TestPlayer()
        {
            var expectedCord = 200;
            var player = new Player(228, 1488);
            Assert.AreEqual(3, player.Health);
            Assert.AreEqual(1488, player.X);
            for (var i = 0; i < 3; i++)
                player.Move(1);
            for (var i = 0; i < 13; i++)
                player.Move(-1);
            Assert.AreEqual(expectedCord, player.X);
        }

        [Test]
        public void TestBullet()
        {
            var bullet = new Bullet(250, 250, 10);
            var expSleepX = -10;
            var expSleepY = -10;
            var expY = 200;
            for (var i = 0; i < 228; i++)
                bullet.Fly(-1);
            for (var i = 0; i < 223; i++)
                bullet.Fly(1);
            Assert.AreEqual(expY, bullet.Y);
            bullet.Clear();
            Assert.AreEqual(expSleepX, bullet.X);
            Assert.AreEqual(expSleepY, bullet.Y);
        }

        [Test]
        public void TestAlien()
        {
            var alien = new Alien(50, 100, -1);
            var expX = 300;
            var expY = 300;
            for (var i = 0; i < 3; i++)
                alien.Move(-1);
            for (var i = 0; i < 53; i++)
                alien.Move(1);
            for (var i = 0; i < 20; i++)
                alien.Move(2);
            Assert.AreEqual(expX, alien.X);
            Assert.AreEqual(expY, alien.Y);
        }

        [Test]
        public void TestWall()
        {
            var wall = new Wall(400, 400);
            wall.health -= 6;
            Assert.AreEqual(4, wall.health);
        }
    }
}
