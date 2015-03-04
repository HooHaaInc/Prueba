using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{

    class Plataforma
    {
        bool peligro;
        Rectangle plataforma;

        Plataforma(Rectangle plataforma, bool peligro = false) {
            this.peligro = peligro;
            this.plataforma = plataforma;
        }
    }

   
}
