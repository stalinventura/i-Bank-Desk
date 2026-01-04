using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Payment.Bank.Modulos
{ 
    
    public class clsValidacionesBO
    {

        public static String PhoneFormat(string Telefono)
        {
            String PhoneFormat ="";
            try
            {
                Telefono = Telefono.Replace("(", "").Replace(")", "").Replace("-", "");
                if (Telefono.Length < 10)
                {
                    PhoneFormat = Telefono;
                }
                else
                {
                    if (Telefono.Length == 10)
                    {
                        PhoneFormat = "(" + Telefono.Substring(0, 3) + ")" + Telefono.Substring(3, 3) + "-" + Telefono.Substring(6, 4);// + Telefono.Substring(10);
                    }
                    else 
                    {
                        PhoneFormat = "(" + Telefono.Substring(0, 3) + ")" + Telefono.Substring(3, 3) + "-" + Telefono.Substring(6, 4);// +Telefono.Substring(10);
                    }
                }
                return PhoneFormat;
            }
            catch
            {
                return "";
            }
        }

        public static String DocumentFormat(string Documento)
        {
            String DocumentFormat = "";
            try
            {
                Documento = Documento.Replace("-", "");
                if (Documento.Length < 10)
                {
                    DocumentFormat = Documento;
                }
                else
                {
                    if (Documento.Length == 11)
                    {
                        DocumentFormat = Documento.Substring(0, 3) + "-" + Documento.Substring(3, 7) + "-" + Documento.Substring(10,1);
                    }
                    else
                    {
                        DocumentFormat = Documento.Substring(0, 3) + "-" + Documento.Substring(3, 7) + "-" + Documento.Substring(10, 1);
                    }
                }
                return DocumentFormat;
            }
            catch
            {
                return "";
            }
        }

        public static byte[] FileToByte(string filePath)
        {
            try
            {
                using (FileStream _File = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] _ByteArray = new byte[_File.Length];
                    int bytesRead = 0;
                    int BytesToRead = (int)_File.Length;

                    while (BytesToRead > 0)
                    {
                        int read = _File.Read(_ByteArray, bytesRead, BytesToRead);
                        if (read == 0)
                            break;
                        BytesToRead -= read;
                        bytesRead += read;
                    }

                    return _ByteArray;
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Can not find the file ");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void ByteToFile(byte[] _ByteArray, string _FileName)
        {
            try
            {
                using (FileStream _File = new FileStream(_FileName, FileMode.Create, FileAccess.Write))
                {
                    _File.Write(_ByteArray, 0, _ByteArray.Length);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Can not find the file " + _FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public static bool Validar(Grid grid)
        {
            bool resultado = true;
            try
            {
                foreach (System.Windows.Controls.Control Obj in grid.Children)
                {
                    if (Obj is System.Windows.Controls.ComboBox && resultado == true)
                    {
                        System.Windows.Controls.ComboBox ctl = (System.Windows.Controls.ComboBox)Obj;
                        if ((int)ctl.SelectedValue == -1)
                        {
                            ctl.Focus();
                            resultado = false;
                            break;
                        }
                    }
                    if (Obj is System.Windows.Controls.TextBox && resultado == true)
                    {
                        System.Windows.Controls.TextBox ctl = (System.Windows.Controls.TextBox)Obj;
                        if (String.IsNullOrEmpty(ctl.Text))
                        {
                            ctl.Focus();
                            resultado = false;
                            break;
                        }
                        else
                        {
                            if (!Regex.IsMatch(ctl.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                            {
                                if (Obj.Name == "txtEmail" || Obj.Name == "txtCorreoElectronico" || Obj.Name == "txtCorreo")
                                {
                                    ctl.Focus();
                                    resultado = false;
                                    break;
                                }
                            }
                        }
                    }
                    if (Obj is System.Windows.Controls.PasswordBox && resultado == true)
                    {
                        System.Windows.Controls.PasswordBox ctl = (System.Windows.Controls.PasswordBox)Obj;
                        if (String.IsNullOrEmpty(ctl.Password))
                        {
                            ctl.Focus();
                            resultado = false;
                           break;
                        }
                    }
                }
                return resultado;
            }
            catch
            {
                return resultado;
            }
        }

        public static void Limpiar(Grid grid)
        {
            try
            {
                foreach (System.Windows.Controls.Control Obj in grid.Children)
                {
                    if (Obj is System.Windows.Controls.ComboBox)
                    {
                        System.Windows.Controls.ComboBox ctl = (System.Windows.Controls.ComboBox)Obj;
                        if ((int)ctl.SelectedValue != -1)
                        {
                            ctl.SelectedValue = -1;
                        }
                    }
                    if (Obj is System.Windows.Controls.TextBox)
                    {
                        System.Windows.Controls.TextBox ctl = (System.Windows.Controls.TextBox)Obj;
                        if (!String.IsNullOrEmpty(ctl.Text))
                        {
                            ctl.Text = "";
                        }
                    }
                }
            }
            catch
            {}
        }

        public static void Enable(Grid grid)
        {
            try
            {
                foreach (System.Windows.Controls.Control Obj in grid.Children)
                {
                    if (Obj is System.Windows.Controls.ComboBox)
                    {
                        System.Windows.Controls.ComboBox ctl = (System.Windows.Controls.ComboBox)Obj;
                        if ((int)ctl.SelectedValue != -1)
                        {
                            ctl.IsEnabled = true;
                        }
                    }
                    if (Obj is System.Windows.Controls.TextBox)
                    {
                        System.Windows.Controls.TextBox ctl = (System.Windows.Controls.TextBox)Obj;
                        if (!String.IsNullOrEmpty(ctl.Text))
                        {
                            ctl.IsEnabled = true;
                        }
                    }
                }
            }
            catch
            { }
        }

        public static void Disable(Grid grid)
        {
            try
            {
                foreach (System.Windows.Controls.Control Obj in grid.Children)
                {
                    if (Obj is System.Windows.Controls.ComboBox)
                    {
                        System.Windows.Controls.ComboBox ctl = (System.Windows.Controls.ComboBox)Obj;
                        if ((int)ctl.SelectedValue != -1)
                        {
                            ctl.IsEnabled = false;
                        }
                    }
                    if (Obj is System.Windows.Controls.TextBox)
                    {
                        System.Windows.Controls.TextBox ctl = (System.Windows.Controls.TextBox)Obj;
                        if (!String.IsNullOrEmpty(ctl.Text))
                        {
                            ctl.IsEnabled = false;
                        }
                    }
                }
            }
            catch
            { }
        }
    }
}
