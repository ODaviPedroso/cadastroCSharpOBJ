using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace objetosUdemy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseDeDados baseDeDados = new BaseDeDados("BaseDeDados.Xml");
            InterfaceGrafica meuprograma = new InterfaceGrafica(baseDeDados);
            meuprograma.IniciaInterface();
        }
    }
}
