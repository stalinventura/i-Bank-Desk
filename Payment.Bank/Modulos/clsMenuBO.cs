using MahApps.Metro.Controls;
using Payment.Bank.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using Payment.Bank.Entity;

namespace Payment.Bank.Modulos
{
    
    class clsMenuBO
    {
      
        public static Menu GenerarMenu()
        {
            try
            {
                Core.Manager core = new Core.Manager(); 
                Menu mnuPrincipal = new Menu();
                MenuItem mnuProducto;
                clsMenuItemBO mnuItem;
                var _Productos = core.ProductosGet(clsVariablesBO.UsuariosBE.RolID);
                var _Permisos = core.PermisosGetByRolID(clsVariablesBO.UsuariosBE.RolID).ToArray();
                               
                foreach (clsProductosBE Padre in _Productos.OrderBy(x=>x.Orden))
                {
                    mnuProducto = new MenuItem();
                    mnuProducto.Header = clsLenguajeBO.Find(Padre.Producto);

                    for (int i = 0; i <= _Permisos.Count() - 1; i++)
                    {
                        if (_Permisos[i].ProductoID == Padre.ProductoID)
                        {
                            mnuItem = new clsMenuItemBO();
                            var PermisoRol = core.PermisosRolesGetByPermisoID(clsVariablesBO.UsuariosBE.RolID, _Permisos[i].PermisoID).FirstOrDefault();

                            mnuItem.Header = clsLenguajeBO.Find(_Permisos[i].Permiso);
                            mnuItem.Name = _Permisos[i].Codigo;
                            mnuItem.ToolTip = clsLenguajeBO.Find(_Permisos[i].Descripcion);
                            //mnuItem.Agregar = PermisoRol.Agregar;
                            //mnuItem.Modificar = PermisoRol.Modificar;
                            //mnuItem.Eliminar = PermisoRol.Eliminar;
                            //mnuItem.Imprimir = PermisoRol.Imprimir;

                            foreach (var row in _Permisos[i].PermisosRoles)
                            {
                                mnuItem.Agregar = PermisoRol.Agregar;
                                mnuItem.Modificar = PermisoRol.Modificar;
                                mnuItem.Eliminar = PermisoRol.Eliminar;
                                mnuItem.Imprimir = PermisoRol.Imprimir;
                            }

                            Image img = new Image();
                            img.Height = 5;
                            img.Width = 5;
                            //if (_Permisos[i].Icon != null)
                            //{
                                //img.Source = new BitmapImage(new Uri(_Permisos[i].Icon, UriKind.Relative));
                                img.Source = new BitmapImage(new Uri("/Images/bullet.png", UriKind.Relative));                                
                                mnuItem.Icon = img;
                            //}

                            RoutedEventHandler handle = new RoutedEventHandler(clsItemClick.menuItem_Click);
                            mnuItem.AddHandler(MenuItem.ClickEvent, handle);

                            mnuItem.Height = 32;
                            mnuItem.Padding = new Thickness(0, 3, 0, 0);
                            mnuProducto.Items.Add(mnuItem);
                        }
                    }
                    mnuPrincipal.Items.Add(mnuProducto);
                }
                return mnuPrincipal;
            }
            catch { return null; }
        }

        public static TabControl GenerarTabMenu()
        {
            try
            {
                Core.Manager core = new Core.Manager();
                TabControl mnuPrincipal = new TabControl();
                TabItem mnuProducto;
                clsTilesBO mnuItem;
                WrapPanel wpHost;
                Image img;
                                
                var _Productos = core.ProductosGet(clsVariablesBO.UsuariosBE.RolID).OrderBy(x=>x.Orden);
                var _Permisos = core.PermisosGetByRolID(clsVariablesBO.UsuariosBE.RolID).ToArray();

                foreach (clsProductosBE Padre in _Productos)
                {
                    mnuProducto = new TabItem();              
                    mnuProducto.Header = clsLenguajeBO.Find(Padre.Producto);
                    mnuProducto.Style = (Style)mnuProducto.FindResource("MetroTabItem");
                    //ScrollViewer scroll = new ScrollViewer();
                    //scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                    wpHost = new WrapPanel();   
                    wpHost.Margin = new Thickness(2,40, 2, 2);
                    wpHost.Orientation = Orientation.Horizontal;
                    wpHost.HorizontalAlignment = HorizontalAlignment.Center;
                   

                    for (int i = 0; i <= _Permisos.Count() - 1; i++)
                    {
                        if (_Permisos[i].ProductoID == Padre.ProductoID)
                        {
                            mnuItem = new clsTilesBO();
                            var PermisoRol = core.PermisosRolesGetByPermisoID(clsVariablesBO.UsuariosBE.RolID, _Permisos[i].PermisoID).FirstOrDefault();

                            mnuItem.Width = 200;
                            mnuItem.Height = 120;
                            mnuItem.Background = (Brush)mnuItem.FindResource("BackgroundHeader");
                            mnuItem.Title = clsLenguajeBO.Find(_Permisos[i].Permiso);
                            mnuItem.Name = _Permisos[i].Codigo;
                            mnuItem.TiltFactor = _Permisos[i].PermisoID;

                            foreach (var row in _Permisos[i].PermisosRoles)
                            {
                                mnuItem.Agregar = PermisoRol.Agregar;
                                mnuItem.Modificar = PermisoRol.Modificar;
                                mnuItem.Eliminar = PermisoRol.Eliminar;
                                mnuItem.Imprimir = PermisoRol.Imprimir;
                            }

                            img = new Image();
                            img.Height = 60;
                            img.Width = 60;
                            if (_Permisos[i].Icon != null)
                            {
                                img.Source = new BitmapImage(new Uri(_Permisos[i].Icon, UriKind.Relative));
                                mnuItem.Content = img;
                            }

                            mnuItem.Click += clsItemClick.menuItem_Click;

                            wpHost.Children.Add(mnuItem);
                                                      

                            //RoutedEventHandler handle = new RoutedEventHandler(clsItemClick.menuItem_Click);
                            //mnuItem.AddHandler(MenuItem.ClickEvent, handle);

                            //mnuItem.Height = 32;
                            //mnuItem.Padding = new Thickness(0, 3, 0, 0);
                            //mnuProducto..Add(mnuItem);
                        }
                       
                        mnuProducto.Content = wpHost;
                    }
                    //mnuProducto.FontSize = 42;
                    //scroll.Content = mnuProducto;
                    //mnuPrincipal.Style = (Style)mnuPrincipal.FindResource("MetroTabControl");
                   
                    mnuPrincipal.Items.Add(mnuProducto);
                }
                return mnuPrincipal;
            }
            catch { return null; }
        }


    }
}
