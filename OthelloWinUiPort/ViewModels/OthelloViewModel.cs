//using GalaSoft.MvvmLight.Command;
using IA_OthelloReversi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Input;
//using System.Windows.Media;

namespace IA_OthelloReversi.ViewModels
{
    public class OthelloViewModel:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
       public ObservableCollection<Ficha> FichasView { get; set; } = new ObservableCollection<Ficha>();
        const int ROWS = 8;
        const int COLS = 8;
        private Othello? othello;

        public Othello? Othello
        {
            get { return othello; }
            set { othello = value; OnPropertyChanged(nameof(Othello)); }
        }
        private char[,] tablero;

        public char[,] Tablero
        {
            get { return tablero; }
            set { tablero = value; OnPropertyChanged(nameof(Tablero)); }
        }

        public ICommand FichaCommand { get; set; }

        public ICommand nuevoJuego { get; set; }

        private string mensaje;

        public string Mensaje
        {
            get { return mensaje; }
            set { mensaje = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mensaje"));
            }
        }

        private string cantB;

        public string CantidadBlancas
        {
            get { return cantB; }
            set
            {
                cantB = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CantidadBlancas"));
            }
        }

        private string cantN;

        public string CantidadNegras
        {
            get { return cantN; }
            set
            {
                cantN = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CantidadNegras"));
            }
        }

        public OthelloViewModel()
        {
            Othello = new Othello('W', 4, 0, 0);
            GenerarPartida();
            //FichaCommand = new RelayCommand<Button>(ejecutarActualizacion);
            //nuevoJuego = new RelayCommand(GenerarPartida);
        }

        //private void ejecutarActualizacion(Button obj)
        //{
        //    throw new NotImplementedException();
        //}
        public void GenerarPartida()
        {
            FichasView.Clear();
            Othello.player.resetBoard();
            Tablero = Othello.player.printBoard();
            for (int i = 0; i < COLS; i++)
            {
                for (int j = 0; j < ROWS; j++)
                {
                    FichasView.Insert((i + 1) * j, new Ficha()
                    {
                        ColorFicha = Tablero[i, j] == 'W' ? "White" : Tablero[i, j] == 'B' ? "Black" : "Transparent",
                        x = j, y = i,
                        Signo = Tablero[i, j] == 'W' ? 'W' : Tablero[i, j] == 'B' ? 'B' : 'S' 
                    });
                }
            }
            Contador();
            OnPropertyChanged(nameof(FichasView));
        }

        public void actualizar(bool IsMe,int column,int row)
        {
            Tablero = Othello.player.printBoard();
            if (Othello!=null)
            {
                FichasView.Clear();
                Tablero=Othello.CapturarSiguienteMovidaJugadorHumano(IsMe,$"{column} {row}");
                OnPropertyChanged(nameof(Othello));
                for (int i = 0; i < COLS; i++)
                {
                    for (int j = 0; j < ROWS; j++)
                    {
                        FichasView.Insert((i + 1) * j, new Ficha()
                        {
                            x=j,y=i,
                            ColorFicha = Tablero[i, j] == 'W' ? "White" : Tablero[i, j] == 'B' ? "Black" : "Transparent",
                             Signo = Tablero[i, j] == 'W' ? 'W' : Tablero[i, j] == 'B' ? 'B' : 'S' 
                        });
                    }
                }
            }
            Mensaje = "Turno del jugador.";
            OnPropertyChanged(nameof(FichasView));
        }

        public void Contador()
        {
            CantidadBlancas = FichasView.Count(x => x.Signo == 'W').ToString();
            CantidadNegras = FichasView.Count(x => x.Signo == 'B').ToString();
        }
 
        public void Prueba(Ficha ficha)
        {
           
            if (Othello.player.getLegal(Tablero,'B').Any(x=>x.x==ficha.x&&x.y==ficha.y))
            {
                if (ficha.ColorFicha == "Transparent" && ficha.Signo == 'S')
                {
                    ficha.Signo = 'B';
                    ficha.ColorFicha = "Black";
                    Mensaje = "Turno de la IA";
                    actualizar(false,ficha.x, ficha.y);
                    Contador();
                    OnPropertyChanged(nameof(FichasView));
                }
                else { Mensaje = "Movimiento invalido."; }
            }
            else if(Othello.player.getLegal(Tablero, 'B').Count == 0&& Othello.player.getLegal(Tablero, 'W').Count == 0|| int.Parse(CantidadBlancas) + int.Parse(CantidadNegras)==64)
            {
                var ganador = int.Parse(CantidadBlancas) > int.Parse(CantidadNegras) ? "BLANCAS" : "NEGRAS";
                //MessageBox.Show($"Han ganado las {ganador}");
            }
            else if (Othello.player.getLegal(Tablero, 'B').Count==0&& Othello.player.getLegal(Tablero, 'W').Any(x => x.x == ficha.x && x.y == ficha.y))
            {
                var movimiento=Othello.player.makeMove();
                Mensaje = "Ficha Blanca IA";
                ficha.Signo = 'W';
                ficha.ColorFicha = "White";
                String[] xy = movimiento.Split(" "); 
                actualizar(true, int.Parse(xy[0]), int.Parse(xy[1]));
                Contador();
                OnPropertyChanged(nameof(FichasView));
            }
          
            else { Mensaje = "Movimiento invalido."; }
            Contador();
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }





    }
}
