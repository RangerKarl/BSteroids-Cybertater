using Godot;
using System;

namespace BSteroids.Scripts.Game
{
    public partial class Ui : CanvasLayer
    {
        [Export]
        Label _gameOverLabel;
        [Export]
        Label _scoreLabel;

        AsteroidSpawner _asteroidSpawner;
        PlayerMovement _player;

        public override void _Ready()
        {
            // _gameOverLabel = GetNode<Label>("GameOverLabel");
            // _scoreLabel = GetNode<Label>("ScoreLabel");

            foreach (var i in GetTree().Root.GetChildren())
            {
                GD.Print(i.Name);
            }

            _asteroidSpawner = GetNode<AsteroidSpawner>("/root/Main/AsteroidSpawner");
            _asteroidSpawner.PointsUpdated += SetScore;
            _player = GetNode<PlayerMovement>("/root/Main/Player");
            _player.PlayerDeath += ShowGameOver;
        }

        private void SetScore(int score)
        {
            _scoreLabel.Text =  String.Format($"{score}"); 
        }

        private void ShowGameOver()
        {
            _gameOverLabel.Show();
            _player.PlayerDeath -= ShowGameOver;
            _player = null;
        }
    }   
}
