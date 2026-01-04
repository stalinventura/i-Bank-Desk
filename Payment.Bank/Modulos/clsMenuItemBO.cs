using System.Windows.Controls;

namespace Payment.Bank.Modulos
{
    public class clsMenuItemBO:MenuItem
    {
        private int _idpermiso;
        public int IdPermiso
        {
            get { return _idpermiso; }
            set { _idpermiso = value; }
        }
        private bool _agregar;
        public bool Agregar
        {
            get { return _agregar; }
            set { _agregar = value; }
        }
        private bool _modificar;
        public bool Modificar
        {
            get { return _modificar; }
            set { _modificar = value; }
        }

        private bool _eliminar;
        public bool Eliminar
        {
            get { return _eliminar; }
            set { _eliminar = value; }
        }

        private bool _imprimir;
        public bool Imprimir
        {
            get { return _imprimir; }
            set { _imprimir = value; }
        }

    }
}
