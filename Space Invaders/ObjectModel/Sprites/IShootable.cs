using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Space_Invaders.Managers;

namespace Space_Invaders.ObjectModel.Sprites
{
    public interface IShootable
    {
        ShootsManager ShootsManager
        {
            get;
            set;
        }
    }
}
