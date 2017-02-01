using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Managers;
using Space_Invaders.ObjectModel.Sprites;
using Infrastructure.ObjectModel;
using Infrastructure.Managers;
using Space_Invaders.ObjectModel.Screens;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders
{
    public class SpaceInvaders : Game
    {
        public static string k_SoundPath = "C:\\temp\\XNA_Assets\\Ex03\\Sounds\\";

        private readonly string r_GameName = "Space Invaders";

        private GraphicsDeviceManager m_GraphicsMgr;
        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return m_GraphicsMgr; }
        }
        private ScreensMananger m_ScreensManager;
        private GameScreen m_WelcomeScreen;
        private LevelTransitionScreeen m_TransitionScreen;
        private PlayGameScreen m_PlayGameScreen;
        private SoundEffectsManager m_SoundManager;
        private SoundEffectInstance m_BGMusic;
        private InputManager m_InputManager;

        public SpaceInvaders()
        {
            m_GraphicsMgr = new GraphicsDeviceManager(this);
            m_GraphicsMgr.PreferredBackBufferHeight = 600;
            m_GraphicsMgr.PreferredBackBufferWidth = 800;
            this.Content.RootDirectory = "Content";
            SpaceInvadersSettings settings = SpaceInvadersSettings.GetInstance(this);
            settings.GraphicsManager = m_GraphicsMgr;

            m_InputManager = new InputManager(this);
            m_SoundManager = SoundEffectsManager.GetInstance(this);
            m_TransitionScreen = new LevelTransitionScreeen(this, 1);
            m_PlayGameScreen = new PlayGameScreen(this, m_TransitionScreen);
            m_WelcomeScreen = new WelcomeScreen(this, m_PlayGameScreen, m_TransitionScreen);
            m_ScreensManager = new ScreensMananger(this);

            m_ScreensManager.Push(m_PlayGameScreen);
            m_ScreensManager.Push(m_TransitionScreen);
            m_ScreensManager.SetCurrentScreen(m_WelcomeScreen);
        }

        protected override void Initialize()
        {
            m_SoundManager.LoadContent();
            base.Initialize();
            this.Window.Title = r_GameName;
        }
        
        //public void Restart()
        //{
        //    Initialize();
        //    m_ScreensManager.Remove(m_PlayGameScreen);
        //    m_ScreensManager.Remove(m_WelcomeScreen);
        //    m_ScreensManager.Remove(m_TransitionScreen);
        //    m_ScreensManager.Push(m_PlayGameScreen);
        //    m_ScreensManager.Push(m_TransitionScreen);
        //    m_ScreensManager.SetCurrentScreen(m_WelcomeScreen);
        //}

        protected override void BeginRun()
        {
            base.BeginRun();
            m_BGMusic = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.BGMusic);
            m_BGMusic.IsLooped = true;
            m_BGMusic.Play();
        }

        protected override void Draw(GameTime i_GameTime)
        {
            m_GraphicsMgr.GraphicsDevice.Clear(Color.CornflowerBlue);
            
            base.Draw(i_GameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (m_InputManager.KeyPressed(Keys.OemMinus))
            {
                SpaceInvadersSettings.GetInstance(this).IsSoundOff = !SpaceInvadersSettings.GetInstance(this).IsSoundOff;
            }

            if (m_InputManager.KeyPressed(Keys.Escape))
            {
                this.Exit();
            }
        }
    }
}
