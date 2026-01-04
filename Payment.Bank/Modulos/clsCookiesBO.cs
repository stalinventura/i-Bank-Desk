using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.Modulos
{
    class clsCookiesBO
    {
        public static void CorreosCreate(String Correo)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "Correo.txt");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(Correo);
                }
            }
            catch { }
        }

        public static void Key(String Key)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "Key.txt");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(Key);
                }
            }
            catch { }
        }


        public static String GetKey()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "Key.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static String CorreosGet()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "Correo.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return line;
                    }
                    return line;
                }
            }
            catch { return null; }
        }

        public static void PrintApplicacion(bool value)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintApplicacion.txt");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(value);
                }
            }
            catch { }
        }

        public static bool getPrintApplicacion()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintApplicacion.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return bool.Parse(line);
                    }
                    return bool.Parse(line);
                }
            }
            catch { return true; }
        }

        public static void DeletePrintApplicacion()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintApplicacion.txt");
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            catch { }
        }

        public static void PrintContract(bool value)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintContract.txt");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(value);
                }
            }
            catch { }
        }

        public static bool getPrintContract()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintContract.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return bool.Parse(line);
                    }
                    return bool.Parse(line);
                }
            }
            catch { return true; }
        }

        public static void DeletePrintContract()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintContract.txt");
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            catch { }
        }

        public static void PrintReceipt(bool value)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintReceipt.txt");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(value);
                }
            }
            catch { }
        }

        public static bool getPrintReceipt()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintReceipt.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return bool.Parse(line);
                    }
                    return bool.Parse(line);
                }
            }
            catch { return true; }
        }

        public static void DeletePrintReceipt()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "PrintReceipt.txt");
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            catch { }
        }

        public static void CorreosDelete()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "Correo.txt");
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            catch { }
        }

        public static void LenguajeID(int LenguajeID)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file =  Path.Combine(path, "Lenguaje.txt");            
                using (StreamWriter sw = new StreamWriter(file))
                {
                   sw.Write(LenguajeID);
                }
            }
            catch { }
        }

        public static int LenguajeID()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "Lenguaje.txt");
                using (StreamReader sr = new StreamReader(file))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        return int.Parse(line);
                    }
                    return int.Parse(line);
                }
            }
            catch { return 1; }
        }

        public static void DeleteLenguajeID()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var file = Path.Combine(path, "Lenguaje.txt");
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            catch { }
        }
    }
}
