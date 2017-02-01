using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Managers;
using Space_Invaders.ObjectModel.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invaders.ObjectModel.Screens
{
    public class PlayGameScreen : GameScreen
    {
        // game readonly 
        private readonly List<Sprite> r_Enemys;

        // graphics
        private CollisionManager m_CollisionManager;
        private PlayerSpaceShip[] m_Players;
        private WallMatrix m_WallsMatrix;
        private EnemySpaceShipsMatrix m_EnemyMatrix;
        private ShootsManager m_ShootsManager;
        private SpriteFont m_Font;
        private Background m_BackGround;
        private GameScreen m_PauseScreen;
        private GameOverScreen m_GameOverScreen;
        private LevelTransitionScreeen m_FirstLevelTransitionScreen;
        private SoundEffectsManager m_SoundManager;
        private SoundEffectInstance m_GameOverSound;
        private SoundEffectInstance m_GameWonSound;
        private int m_Level;
        public int Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }
        

        public PlayGameScreen(Game i_Game, LevelTransitionScreeen i_TransitionScreen) : base(i_Game)
        {
            m_FirstLevelTransitionScreen = i_TransitionScreen;
            m_BackGround = new Background(i_Game, this, @"Sprites\BG_Space01_1024x768", 1);    
            m_ShootsManager = new ShootsManager(this.Game);
            m_SoundManager = SoundEffectsManager.GetInstance(i_Game);
            const bool k_IsFirstPlayer = true;
            
            PlayerSpaceShip[] players = new PlayerSpaceShip[2];
            players[0] = new PlayerSpaceShip(this.Game, this, m_ShootsManager, k_IsFirstPlayer);
            players[1] = new PlayerSpaceShip(this.Game, this, @"Sprites/Ship02_32x32", m_ShootsManager, !k_IsFirstPlayer);
            
            
            EnemyMotherShip motherShip = new EnemyMotherShip(this.Game, this, m_ShootsManager);
            EnemySpaceShipsMatrix spaceshipMatrix = new EnemySpaceShipsMatrix(this.Game, this, m_ShootsManager);
            WallMatrix wallMatrix = new WallMatrix(this.Game, this, m_ShootsManager);
            
            m_ShootsManager.EnemyGotShot += players[0].EnemyGotShot;
            m_ShootsManager.EnemyGotShot += players[1].EnemyGotShot;
            m_PauseScreen = new PauseScreen(this.Game);
            m_GameOverScreen = new GameOverScreen(this.Game, this, m_FirstLevelTransitionScreen);

            m_Players = players;
            r_Enemys = new List<Sprite>();
            r_Enemys.Add(motherShip);
            r_Enemys.Add(spaceshipMatrix);
            m_WallsMatrix = wallMatrix;
            m_EnemyMatrix = spaceshipMatrix;
            m_Level = 1;
            
            this.Add(m_Players[0]);
            this.Add(m_Players[1]);
            this.Add(motherShip);
            this.Add(spaceshipMatrix);
            this.Add(wallMatrix);
            this.Add(m_BackGround);

            List<PlayerSpaceShip> playersList = new List<PlayerSpaceShip>();
            playersList.Add(m_Players[0] as PlayerSpaceShip);
            playersList.Add(m_Players[1] as PlayerSpaceShip);
            m_CollisionManager = new CollisionManager(this.Game, playersList, spaceshipMatrix.Sprites, wallMatrix.Sprites);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (!SpaceInvadersSettings.GetInstance(Game).TwoPlayers)
            {
                Remove(m_Players[1]);
                foreach (Soul soul in m_Players[1].Souls)
                {
                    Remove(soul);
                }
            }
            else
            {
                if (!this.Contains(m_Players[1]))
                {
                    Add(m_Players[1]);
                    foreach (Soul soul in m_Players[1].Souls)
                    {
                        Add(soul);
                    }
                }
            }
        }

        public void Reset()
        {
            base.Initialize();
            m_Level = 1;
            resetSpritesAndManagers();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            
            m_Font = this.Game.Content.Load<SpriteFont>(@"Fonts/Consolas");
            m_GameOverSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.GameOver);
            m_GameWonSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.LevelWin);
        }


        private bool isPlayerDied()
        {
            bool isNoMorePlayers = false;
            if (SpaceInvadersSettings.GetInstance(Game).TwoPlayers)
            {
                isNoMorePlayers = m_Players[0].IsAlive == false && m_Players[1].IsAlive == false;
            }
            else
            {
                isNoMorePlayers = m_Players[0].IsAlive == false;
            }
            return isNoMorePlayers;
        }

        public override void Update(GameTime i_GameTime)
        {
            if (InputManager.ButtonPressed(eInputButtons.Back) ||
                    InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            if (InputManager.KeyPressed(Keys.P))
            {
                this.ScreensManager.SetCurrentScreen(m_PauseScreen);
            }

            base.Update(i_GameTime);
            m_ShootsManager.Update();
            m_CollisionManager.Update();
            if (m_CollisionManager.IsCollisionHappend() || isPlayerDied())
            {
                GameOver();
            }

            if (noMoreEnemys())
            {
                GameWon();
            }
        }

        private bool noMoreEnemys()
        {
            bool noMoreEnemys = true;
            foreach (Sprite enemy in r_Enemys)
            {
                if (enemy.Visible)
                {
                    noMoreEnemys = false;
                    break;
                }
            }

            return noMoreEnemys;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(
                  m_Font,
                  string.Format("P1 Score: {0}", m_Players[0].Score),
                  Vector2.Zero + new Vector2(5),
                  Color.Blue);
            if (SpaceInvadersSettings.GetInstance(Game).TwoPlayers)
            {
                m_SpriteBatch.DrawString(
                    m_Font,
                    string.Format("P2 Score: {0}", m_Players[1].Score),
                    new Vector2(5, 5 + 20),
                    Color.Green);
            }
            m_SpriteBatch.End();
        }

        public void GameOver()
        {
            m_GameOverSound.Play();
            ShowEndOfGameMessage("GAME OVER!!!");
        }

        public void GameWon()
        {
            this.State = eScreenState.Inactive;
            m_GameWonSound.Play();
            m_Level++;
            resetSpritesAndManagers();
            ScreensManager.SetCurrentScreen(new LevelTransitionScreeen(this.Game, m_Level));
        }

        private void resetSpritesAndManagers()
        {
            foreach (PlayerSpaceShip player in m_Players)
            {
                player.MoveToLevel(m_Level - 1);
            }

            foreach (Sprite sprite in r_Enemys)
            {
                sprite.MoveToLevel(m_Level - 1);
            }

            m_WallsMatrix.MoveToLevel(m_Level - 1);
            m_CollisionManager.Restart(m_Players.ToList<PlayerSpaceShip>(), m_EnemyMatrix.Sprites, m_WallsMatrix.Sprites);
            m_ShootsManager.Reset();
        }

        public void ShowEndOfGameMessage(string i_GameOverMessage)
        {
            m_GameOverScreen.Message = i_GameOverMessage;
            m_GameOverScreen.Player1Score =
                string.Format("Player 1 score is: {0}", m_Players[0].Score);
            m_GameOverScreen.Player2Score = 
                string.Format("Player 2 score is: {0}", m_Players[1].Score);
            ScreensManager.SetCurrentScreen(m_GameOverScreen);
        }
    }
}
