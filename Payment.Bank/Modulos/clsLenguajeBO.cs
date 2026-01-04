
using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using Payment.Bank.Modulos;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Payment.Bank.Modulos
{
    public class clsLenguajeBO
    {
        public static void Load(Grid grid)
        {
            try
            {
                foreach (UIElement Obj in grid.Children)
                {
                    if (Obj is ToggleSwitch)
                    {
                        ToggleSwitch ctl = (ToggleSwitch)Obj;                        
                        ctl.OnLabel = Find(ctl.OnLabel.ToString());
                        ctl.OffLabel = Find(ctl.OffLabel.ToString());
                    }

                    if (Obj is DataGrid)
                    {
                        DataGrid ctl = (DataGrid)Obj;
                        foreach (DataGridColumn column in ctl.Columns)
                        {
                            column.Header = Find(column.Header.ToString());
                        }
                    }

                    if (Obj is TextBlock)
                    {
                        TextBlock ctl = (TextBlock)Obj;
                        ctl.Text = Find(ctl.Text);
                    }

                    if (Obj is Expander)
                    {
                        Expander ctl = (Expander)Obj;
                        ctl.Header = Find(ctl.Header.ToString());
                    }

                    if (Obj is CheckBox)
                    {
                        CheckBox ctl = (CheckBox)Obj;
                        ctl.Content = Find(ctl.Content.ToString());
                    }
                                       
                    if (Obj is RadioButton)
                    {
                        RadioButton ctl = (RadioButton)Obj;
                        ctl.Content = Find(ctl.Name);
                    }

                    try
                    {
                        if (Obj is GroupBox)
                        {
                            GroupBox ctl = (GroupBox)Obj;
                            ctl.Header = Find(ctl.Header.ToString());
                        }
                    }
                    catch { }

                }
            }
            catch (Exception ex){clsMessage.ErrorMessage(ex.Message, clsLenguajeBO.Find("msgTitle"));}
        }

       public static string Find(string Name)
        {
            try
            {
                String Texto = Name;
                foreach (clsEtiquetasBE Etiqueta in clsVariablesBO.Etiquetas)
                {
                    if (Name == Etiqueta.Nombre && Etiqueta.LenguajeID == clsVariablesBO.LenguajeID)
                    { 
                        Texto = Etiqueta.Texto;
                    }
                }
                return Texto;
            }
            catch(Exception ex) { return ex.Message; }
        }

        public static string FindReverse(string Name)
        {
            try
            {
                String Texto = Name;
                foreach (clsEtiquetasBE Etiqueta in clsVariablesBO.Etiquetas)
                {
                    if (Name == Etiqueta.Texto)
                    {
                        Texto = Etiqueta.Nombre;
                    }
                }
                return Texto;
            }
            catch (Exception ex) { return ex.Message; }
        }

    }
}
