using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.ComponentModel;

namespace Payment.Bank.Model
{
   

    public class Denominaciones : INotifyPropertyChanged
    {
        private decimal _denominacion;
        private int _cantidad;
        private int _denominacionID;

        public decimal Denominacion
        {
            get => _denominacion;
            set
            {
                if (_denominacion != value)
                {
                    _denominacion = value;
                    OnPropertyChanged(nameof(Denominacion));
                    OnPropertyChanged(nameof(SubTotal));
                }
            }
        }

        public int DenominacionID
        {
            get => _denominacionID;
            set
            {
                if (_denominacionID != value)
                {
                    _denominacionID = value;
                    OnPropertyChanged(nameof(Denominacion));
                }
            }
        }

        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    OnPropertyChanged(nameof(Cantidad));
                    OnPropertyChanged(nameof(SubTotal));
                }
            }
        }

        public decimal SubTotal => Denominacion * Cantidad;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
