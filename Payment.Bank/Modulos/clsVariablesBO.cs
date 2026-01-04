using Payment.Bank.Entity;
using System.Collections.Generic;

namespace Payment.Bank.Modulos
{
    class clsVariablesBO
    {
        //public static int VISTA = 1; //1 Creando, 2 Modificando
        public static int LenguajeID = 2;
        public static clsMenuItemBO Permiso = new clsMenuItemBO();
        public static List<clsEtiquetasBE> Etiquetas = new List<clsEtiquetasBE>();
        public static frmPrincipalView Host; 
        public static clsUsuariosBE UsuariosBE;
        public static bool IsRemoteQuery = true;
        public static bool IsCheckPrevious = true;
        public static bool GatewaySMS = false;
        public static bool CanUpdate = true;
        public static bool UpdateAutomatically = true;
    }
}
