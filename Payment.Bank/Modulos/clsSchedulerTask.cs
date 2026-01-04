using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Bank.Modulos
{
    public class clsSchedulerTask
    {
        public static void Create(string TaskName)
        {

            try
            {
                if (TaskExist(TaskName))
                {
                    return;
                }

                // 1. Verificar si corre como administrador
                if (!IsAdministrator())
                {
                    Console.WriteLine("Reiniciando como administrador...");

                    var exeName = Process.GetCurrentProcess().MainModule.FileName;

                    var startInfo = new ProcessStartInfo(exeName)
                    {
                        UseShellExecute = true,
                        Verb = "runas" // Forzar ejecución como admin
                    };

                    try
                    {
                        Process.Start(startInfo);
                    }
                    catch
                    {
                        Console.WriteLine("El usuario canceló la elevación.");
                    }

                    return; // Salir del proceso actual
                }

                Console.WriteLine("Ejecutando como administrador ✔");

                using (TaskService ts = new TaskService())
                {
                    // Leer XML
                    string xml = System.IO.File.ReadAllText(Environment.CurrentDirectory + "/Task/i-task.xml");

                    // Crear definición a partir del XML
                    TaskDefinition td = ts.NewTask();
                    td.XmlText = xml;

                    // Registrar tarea (si existe la sobrescribe)
                    ts.RootFolder.RegisterTaskDefinition(TaskName, td, TaskCreation.CreateOrUpdate,
                        userId: null, // usuario actual
                        password: null,
                        logonType: TaskLogonType.InteractiveToken);
                }

                Console.WriteLine("Tarea creada/actualizada correctamente.");
            }
            catch(Exception ex) 
            {
                String path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var filePath = Path.Combine(path, "Tarea.txt");

                string contenido = ex.Message;


                    File.WriteAllText(filePath, contenido);

              
            }

        }


        static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        static bool TaskExist(string TaskName)
        {
            using (TaskService ts = new TaskService())
            {
                // Verificar si ya existe
                Microsoft.Win32.TaskScheduler.Task existingTask = ts.GetTask(TaskName);
                if (existingTask != null)
                {
                    Console.WriteLine($"⚠ La tarea '{TaskName}' ya existe.");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}







