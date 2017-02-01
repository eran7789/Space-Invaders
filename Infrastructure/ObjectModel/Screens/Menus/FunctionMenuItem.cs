using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ObjectModel.Screens.Menus
{
    public class FunctionMenuItem : MenuItem
    {
        public delegate void MenuFunction();
        private MenuFunction m_Function;
        public MenuFunction MyFunction
        {
            get { return m_Function; }
        }

        public FunctionMenuItem(Game i_Game, MenuScreen i_Screen, string i_Description, MenuFunction i_Function) : 
            base(i_Game, i_Screen, i_Description)
        {
            m_Function = i_Function;
        }
    }
}
