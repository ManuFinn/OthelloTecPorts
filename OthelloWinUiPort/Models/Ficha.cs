using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OthelloWinUiPort.Models
{
    public class Ficha : INotifyPropertyChanged
    {
        public int x { get; set; }
        public int y { get; set; }
        private string colorFicha;

        public string ColorFicha
        {
            get { return colorFicha; }
            set { colorFicha = value; OnPropertyChanged(); }
        }

        

        private char signo;

        public char Signo
        {
            get { return signo; }
            set { signo = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
