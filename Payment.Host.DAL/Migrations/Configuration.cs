namespace Payment.Host.DAL.Migrations
{
    using Payment.Host.Entity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Payment.Host.DAL.Context context)
        {
            //Agrega sexos
            if (context.clsSexosBE.Count() < 2)
            {
                context.clsSexosBE.AddOrUpdate(new clsSexosBE { SexoID = 1, Fecha = DateTime.Now, Sexo = "Masculino", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsSexosBE.AddOrUpdate(new clsSexosBE { SexoID = 2, Fecha = DateTime.Now, Sexo = "Femenino", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.SaveChanges();
            }

            //Agrega Estados Civiles
            if (context.clsEstadosCivilesBE.Count() == 0)
            {
                context.clsEstadosCivilesBE.AddOrUpdate(new clsEstadosCivilesBE { EstadoCivilID = 1, Fecha = DateTime.Now, EstadoCivil = "Soltero(a)", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsEstadosCivilesBE.AddOrUpdate(new clsEstadosCivilesBE { EstadoCivilID = 2, Fecha = DateTime.Now, EstadoCivil = "Casado(a)", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.SaveChanges();
            }

            //Agrega Operadores
            if (context.clsOperadoresBE.Count() == 0)
            {
                context.clsOperadoresBE.AddOrUpdate(new clsOperadoresBE { OperadorID = 1, Fecha = DateTime.Now, Operador = "Altice", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsOperadoresBE.AddOrUpdate(new clsOperadoresBE { OperadorID = 2, Fecha = DateTime.Now, Operador = "Claro", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsOperadoresBE.AddOrUpdate(new clsOperadoresBE { OperadorID = 3, Fecha = DateTime.Now, Operador = "Tricon", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsOperadoresBE.AddOrUpdate(new clsOperadoresBE { OperadorID = 4, Fecha = DateTime.Now, Operador = "Viva", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.SaveChanges();
            }

            //Agrega Paises
            if (context.clsPaisesBE.Count() == 0)
            {
                context.clsPaisesBE.AddOrUpdate(new clsPaisesBE { PaisID = 1, Fecha = DateTime.Now, Pais = "Republica Dominicana", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.SaveChanges();
            }

            //Agrega Monedas
            if (context.clsMonedasBE.Count() == 0)
            {
                context.clsMonedasBE.AddOrUpdate(new clsMonedasBE { MonedaID = 214, Fecha = DateTime.Now, Codigo = "DOP", Moneda = "Pesos Dominicanos", PaisID = 1, Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsMonedasBE.AddOrUpdate(new clsMonedasBE { MonedaID = 840, Fecha = DateTime.Now, Codigo = "USD", Moneda = "Dolares Americanos", PaisID = 1, Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.SaveChanges();
            }

            //Agregar Tipo Referencias
            if (context.clsTipoReferenciasBE.Count() == 0)
            {
                context.clsTipoReferenciasBE.AddOrUpdate(new clsTipoReferenciasBE { TipoReferenciaID = 1, Fecha = DateTime.Now, TipoReferencia = "Personal", IsDefault = true, Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsTipoReferenciasBE.AddOrUpdate(new clsTipoReferenciasBE { TipoReferenciaID = 2, Fecha = DateTime.Now, TipoReferencia = "Comercial", IsDefault = false, Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.SaveChanges();
            }

            //Agrega Preguntas
            if (context.clsPreguntasBE.Count() < 7)
            {
                context.clsPreguntasBE.AddOrUpdate(new clsPreguntasBE { PreguntaID = 1, Fecha = DateTime.Now, Pregunta = "¿Cuál es el nombre de tu primer perro?", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsPreguntasBE.AddOrUpdate(new clsPreguntasBE { PreguntaID = 2, Fecha = DateTime.Now, Pregunta = "¿Cuál es la fecha de nacimiento de tu madre?", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsPreguntasBE.AddOrUpdate(new clsPreguntasBE { PreguntaID = 3, Fecha = DateTime.Now, Pregunta = "¿En qué ciudad naciste?", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsPreguntasBE.AddOrUpdate(new clsPreguntasBE { PreguntaID = 4, Fecha = DateTime.Now, Pregunta = "¿Cuál es tu color favorito?", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsPreguntasBE.AddOrUpdate(new clsPreguntasBE { PreguntaID = 5, Fecha = DateTime.Now, Pregunta = "¿En qué ciudad fue tu luna de miel?", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsPreguntasBE.AddOrUpdate(new clsPreguntasBE { PreguntaID = 6, Fecha = DateTime.Now, Pregunta = "¿Cuál es tu equipo de béisbol favorito?", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.clsPreguntasBE.AddOrUpdate(new clsPreguntasBE { PreguntaID = 7, Fecha = DateTime.Now, Pregunta = "¿Cuál es la marca de tu primer vehículo?", Usuario = "049-0065028-6", ModificadoPor = "049-0065028-6", FechaModificacion = DateTime.Now });
                context.SaveChanges();
            }


        }
    }
}
