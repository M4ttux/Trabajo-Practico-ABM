using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Linq;

namespace EFInstituto {
    class Carrera {
        public int CarreraId { get; set; }
        public string CarreraNombre { get; set; }
        public string CarreraModalidad { get; set; }
        public ICollection<Materia> Materias { get; set; } // (1 a n)
    }
    class Materia {
        [Key]
        public int MateriaId { get; set; }
        public string MateriaNombre { get; set; }
        public string MateriaDivision { get; set; }
        public int CarreraId { get; set; }
        public Carrera Carrera { get; set; }
        public Maestro Maestro { get; set; }
        public ICollection<Alumno> Alumnos { get; set; }
    }
    class Maestro {
        [Key]
        public int MaestroId { get; set; }
        public string MaestroNombre { get; set; }
        public string MaestroApellido { get; set; }
        public int MateriaId { get; set; } 
        public Materia Materia { get; set; } // un maestro esta asignado a una materia (1 a 1)
    }
    class Alumno {
        [Key]
        public int AlumnoId { get; set; }
        public string AlumnoNombre { get; set; }
        public string AlumnoApellido { get; set; }
        public int MateriaId { get; set; }
        public Materia Materia { get; set; }
    }

    class EFInstitutoContext : DbContext {
        public DbSet<Carrera> Carrera { get; set; }
        public DbSet<Materia> Materia { get; set; }
        public DbSet<Maestro> Maestro { get; set; }
        public DbSet<Alumno> Alumno { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-VVIR09N;Initial catalog=TPabm;Integrated Security=true");
        }
    }

    class Program {
        #region Menu Principal
        static void Main(string[] args) {
            using (EFInstitutoContext context = new EFInstitutoContext()) {
                int opcion;
                do {
                    ShowHeader("MENÚ PRINCIPAL");
                    Console.Write(
                        " 1.\t Añadir datos. \n" +
                        " 2.\t Mostar lista de datos. \n" +
                        " 3.\t Actualizar datos. \n" +
                        " 4.\t Borrar datos. \n" +
                        " 0.\t Salir. \n");
                    Console.WriteLine("-------------------------------\n");
                    Console.Write(" Ingrese Opcion: ");
                    opcion = int.Parse(Console.ReadLine());
                    switch (opcion) {
                        case 1:
                            CrearDatos(context);
                            break;
                        case 2:
                            MostrarDatos(context);
                            break;
                        case 3:
                            ModificarDatos(context);
                            break;
                        case 4:
                            BorrarDatos(context);
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                        default:
                            MostrarOpcion("Opcion Invalida");
                            break;
                    }
                } while (opcion != 0);
                /// <summary>
                /// Metodo para la creación de los la primera vez que se conecta con la DDBB.
                /// </summary>
                /// <param name="context">Una instancia de la clase DBContext</param>
            }
        }
        #endregion

        #region Menu CrearDatos
        public static void CrearDatos(EFInstitutoContext context) {
            int opcion;
            do {
                ShowHeader("MENÚ AÑADIR DATOS");
                Console.Write(
                    " 1.\t Añadir CARRERA. \n" +
                    " 2.\t Añadir MATERIA. \n" +
                    " 3.\t Añadir MAESTRO. \n" +
                    " 4.\t Añadir ALUMNO. \n" +
                    " 0.\t Menu principal. \n");
                Console.WriteLine("-------------------------------\n");
                Console.Write(" Ingrese Opcion: ");
                opcion = int.Parse(Console.ReadLine());
                switch (opcion) {
                    case 1:
                        CrearCarrera(context);
                        break;
                    case 2:
                        CrearMateria(context);
                        break;
                    case 3:
                        CrearMaestro(context);
                        break;
                    case 4:
                        CrearAlumno(context);
                        break;
                    case 0:
                        Console.Clear();
                        break;
                    default:
                        MostrarOpcion("Opcion Invalida");
                        break;
                }
            } while (opcion != 0);
        }
        #endregion
        #region CrearCarrera
        public static void CrearCarrera(EFInstitutoContext context) {
            ShowHeader("Nueva: >>> Carrera <<<");
            Carrera Carrera = new Carrera();
            Console.Write(" \tNombre de la Carrera: ");
            Carrera.CarreraNombre = Console.ReadLine();
            Console.Write(" \tModalidad de la Carrera (Presencial | Virtual): ");
            Carrera.CarreraModalidad = Console.ReadLine();
            context.Carrera.Add(Carrera);
            context.SaveChanges();
            MostrarOpcion(" DATO CREADO");
        }
        #endregion
        #region CrearMateria
        public static void CrearMateria(EFInstitutoContext context) {
            try {
                ShowHeader("Nueva: >>> MATERIA <<<");
                Materia Materia = new Materia();
                Console.Write(" \tNombre de la Materia: ");
                Materia.MateriaNombre = Console.ReadLine();
                Console.Write(" \tDivision de la Materia: ");
                Materia.MateriaDivision = Console.ReadLine();
                Console.Write(" \tID de Carrera: ");
                Materia.CarreraId = int.Parse(Console.ReadLine());
                context.Materia.Add(Materia);
                context.SaveChanges();
                MostrarOpcion(" DATO CREADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region CrearMaestro
        public static void CrearMaestro(EFInstitutoContext context) {
            try {
                ShowHeader("Nuevo: >>> MAESTRO <<<");
                Maestro maestro = new Maestro();
                Console.Write(" \tNombre del Maestro: ");
                maestro.MaestroNombre = Console.ReadLine();
                Console.Write(" \tApellido del Maestro: ");
                maestro.MaestroApellido = Console.ReadLine();
                Console.Write(" \tID de Materia: ");
                maestro.MateriaId = int.Parse(Console.ReadLine());
                context.Maestro.Add(maestro);
                context.SaveChanges();
                MostrarOpcion(" DATO CREADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region CrearAlumno
        public static void CrearAlumno(EFInstitutoContext context) {
            try {
                ShowHeader("Nuevo: >>> ALUMNO <<<");
                Alumno alumno = new Alumno();
                Console.Write(" \tNombre del Alumno: ");
                alumno.AlumnoNombre = Console.ReadLine();
                Console.Write(" \tApellido del Alumno: ");
                alumno.AlumnoApellido = Console.ReadLine();
                Console.Write(" \tID de Materia: ");
                alumno.MateriaId = int.Parse(Console.ReadLine());
                context.Alumno.Add(alumno);
                context.SaveChanges();
                MostrarOpcion(" DATO CREADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion

        #region Menu MostrarDatos
        public static void MostrarDatos(EFInstitutoContext context) {
            int opcion;
            do {
                ShowHeader("MENÚ MOSTRAR DATOS");
                Console.Write(
                    " 1.\t Mostrar CARRERA. \n" +
                    " 2.\t Mostrar MATERIA. \n" +
                    " 3.\t Mostrar MAESTRO. \n" +
                    " 4.\t Mostrar ALUMNO. \n" +
                    " 5.\t Mostrar LISTA GENERAL. \n" +
                    " 0.\t Menu principal. \n");
                Console.WriteLine("-------------------------------\n");
                Console.Write(" Ingrese Opcion: ");
                opcion = int.Parse(Console.ReadLine());
                switch (opcion) {
                    case 1:
                        MostrarCarrera(context);
                        break;
                    case 2:
                        MostrarMateria(context);
                        break;
                    case 3:
                        MostrarMaestro(context);
                        break;
                    case 4:
                        MostrarAlumno(context);
                        break;
                    case 5:
                        MostrarGeneral(context);
                        break;
                    case 0:
                        Console.Clear();
                        break;
                    default:
                        MostrarOpcion("Opcion Invalida");
                        break;
                }
            } while (opcion != 0);
        }
        #endregion
        #region MostrarCarrera
        public static void MostrarCarrera(EFInstitutoContext context) {
            ShowHeader("Lista >>> CARRERA <<<");
            IQueryable<Carrera> carreras = from carr in context.Carrera select carr;
            Console.WriteLine(" ID \t NOMBRE \t MODALIDAD\n------------------------------------");
            foreach (Carrera carr in carreras) {
                Console.WriteLine($@" {carr.CarreraId} {carr.CarreraNombre} {carr.CarreraModalidad}");
            }
            Console.WriteLine("------------------------------------\n");
            Console.WriteLine("Pulse cualquier tecla para continuar");
            Console.ReadKey();
        }
        #endregion
        #region MostrarMateria
        public static void MostrarMateria(EFInstitutoContext context) {
            ShowHeader("Lista >>> MATERIAS <<<");
            List<Materia> materias = (from mate in context.Materia select mate).ToList();
            Console.WriteLine(" ID \t NOMBRE \t DIVISION \t CARRERA NOMBRE\n------------------------------------");
            foreach (Materia mate in materias) {
                Console.WriteLine($@" {mate.MateriaId} {mate.MateriaNombre} {mate.MateriaDivision} {context.Carrera.Where(x => x.CarreraId == mate.CarreraId).FirstOrDefault().CarreraNombre}");
            }
            Console.WriteLine("-------------------------------\n");
            Console.WriteLine("Pulse cualquier tecla para continuar");
            Console.ReadKey();
        }
        #endregion
        #region MostrarMaestro
        public static void MostrarMaestro(EFInstitutoContext context) {
            ShowHeader("Lista >>> MAESTROS <<<");
            List<Maestro> maestros = (from maes in context.Maestro select maes).ToList();
            Console.WriteLine(" ID \t NOMBRE \t APELLIDO \t MATERIA NOMBRE\n------------------------------------");
            foreach (Maestro maes in maestros) {
                Console.WriteLine($@" {maes.MaestroId} {maes.MaestroNombre} {maes.MaestroApellido} {context.Materia.Where(x => x.MateriaId == maes.MateriaId).FirstOrDefault().MateriaNombre}");
            }
            Console.WriteLine("-------------------------------\n");
            Console.WriteLine("Pulse cualquier tecla para continuar");
            Console.ReadKey();
        }
        #endregion
        #region MostrarAlumno
        public static void MostrarAlumno(EFInstitutoContext context) {
            ShowHeader("Lista >>> ALUMNOS <<<");
            List<Alumno> alumnos = (from alum in context.Alumno select alum).ToList();
            Console.WriteLine(" ID \t NOMBRE \t APELLIDO \t MATERIA NOMBRE\n------------------------------------");
            foreach (Alumno alum in alumnos) {
                Console.WriteLine($@" {alum.AlumnoId} {alum.AlumnoNombre} {alum.AlumnoApellido} {context.Materia.Where(x => x.MateriaId == alum.MateriaId).FirstOrDefault().MateriaNombre}");
            }
            Console.WriteLine("-------------------------------\n");
            Console.WriteLine("Pulse cualquier tecla para continuar");
            Console.ReadKey();
        }
        #endregion
        #region MostrarGeneral
        public static void MostrarGeneral(EFInstitutoContext context) {
            ShowHeader("Lista >>> GENERAL <<<");
            MostrarCarrera(context);
            MostrarMateria(context);
            MostrarMaestro(context);
            MostrarAlumno(context);
        }
        #endregion

        #region Menu ModificarDatos
        public static void ModificarDatos(EFInstitutoContext context) {
            int opcion;
            do {
                ShowHeader("MENÚ MODIFICAR DATOS");
                Console.Write(
                    " 1.\t Modificar CARRERA. \n" +
                    " 2.\t Modificar MATERIA. \n" +
                    " 3.\t Modificar MAESTRO. \n" +
                    " 4.\t Modificar ALUMNO. \n" +
                    " 0.\t Menu principal. \n");
                Console.WriteLine("-------------------------------\n");
                Console.Write(" Ingrese Opcion: ");
                opcion = int.Parse(Console.ReadLine());
                switch (opcion) {
                    case 1:
                        ModificarCarrera(context);
                        break;
                    case 2:
                        ModificarMateria(context);
                        break;
                    case 3:
                        ModificarMaestro(context);
                        break;
                    case 4:
                        ModificarAlumno(context);
                        break;
                    case 0:
                        Console.Clear();
                        break;
                    default:
                        MostrarOpcion("Opcion Invalida");
                        break;
                }
            } while (opcion != 0);
        }
        #endregion
        #region ModificarCarrera
        public static void ModificarCarrera(EFInstitutoContext context) {
            try {
                ShowHeader("Modificar: >>> CARRERA <<<");
                Console.WriteLine(" Lista de CARRERA: \n");
                Carrera carrera = new Carrera();
                MostrarCarrera(context);
                Console.Write(" Id de la Carrera: ");
                int id = int.Parse(Console.ReadLine());
                carrera = context.Carrera.Where(x => x.CarreraId == id).FirstOrDefault();
                Console.Write(" \tNombre de la Carrera: ");
                carrera.CarreraNombre = Console.ReadLine();
                Console.Write(" \tModalidad de la Carrera (Presencial | Virtual): ");
                carrera.CarreraModalidad = Console.ReadLine();
                context.SaveChanges();
                MostrarOpcion(" DATO MODIFICADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region ModificarMateria
        public static void ModificarMateria(EFInstitutoContext context) {
            try {
                ShowHeader("Modificar: >>> MATERIA <<<");
                Console.WriteLine(" Lista de Materia: \n");
                Materia materia = new Materia();
                MostrarMateria(context);
                Console.Write(" Id de la Materia: ");
                int id = int.Parse(Console.ReadLine());
                materia = context.Materia.Where(x => x.MateriaId == id).FirstOrDefault();
                Console.Write(" \tNombre de la Materia: ");
                materia.MateriaNombre = Console.ReadLine();
                Console.Write(" \tDivision de la Materia: ");
                materia.MateriaDivision = Console.ReadLine();
                Console.Write(" \tID de Carrera: ");
                materia.CarreraId = int.Parse(Console.ReadLine());
                context.SaveChanges();
                MostrarOpcion(" DATO MODIFICADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region ModificarMaestro
        public static void ModificarMaestro(EFInstitutoContext context) {
            try {
                ShowHeader("Modificar: >>> MAESTRO <<<");
                Console.WriteLine(" Lista de Maestro: \n");
                Maestro maestro = new Maestro();
                MostrarMaestro(context);
                Console.Write(" Id del Maestro: ");
                int id = int.Parse(Console.ReadLine());
                maestro = context.Maestro.Where(x => x.MaestroId == id).FirstOrDefault();
                Console.Write(" \tNombre del Maestro: ");
                maestro.MaestroNombre = Console.ReadLine();
                Console.Write(" \tApellido del Maestro: ");
                maestro.MaestroApellido = Console.ReadLine();
                Console.Write(" \tID de Materia: ");
                maestro.MateriaId = int.Parse(Console.ReadLine());
                context.SaveChanges();
                MostrarOpcion(" DATO MODIFICADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region ModificarAlumno
        public static void ModificarAlumno(EFInstitutoContext context) {
            try {
                ShowHeader("Modificar: >>> Alumno <<<");
                Console.WriteLine(" Lista de Alumno: \n");
                Alumno alumno = new Alumno();
                MostrarAlumno(context);
                Console.Write(" Id del Alumno: ");
                int id = int.Parse(Console.ReadLine());
                alumno = context.Alumno.Where(x => x.AlumnoId == id).FirstOrDefault();
                Console.Write(" \tNombre del Alumno: ");
                alumno.AlumnoNombre = Console.ReadLine();
                Console.Write(" \tApellido del Alumno: ");
                alumno.AlumnoApellido = Console.ReadLine();
                Console.Write(" \tID de Materia: ");
                alumno.MateriaId = int.Parse(Console.ReadLine());
                context.SaveChanges();
                MostrarOpcion(" DATO MODIFICADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null)
                {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion

        #region Menu BorrarDatos
        public static void BorrarDatos(EFInstitutoContext context) {
            int opcion;
            do {
                ShowHeader("MENÚ BORRAR DATOS");
                Console.Write(
                    " 1.\t Borrar CARRERA. \n" +
                    " 2.\t Borrar MATERIA. \n" +
                    " 3.\t Borrar MAESTRO. \n" +
                    " 4.\t Borrar ALUMNO. \n" +
                    " 0.\t Menu principal. \n");
                Console.WriteLine("-------------------------------\n");
                Console.Write(" Ingrese Opcion: ");
                opcion = int.Parse(Console.ReadLine());
                switch (opcion) {
                    case 1:
                        BorrarCarrera(context);
                        break;
                    case 2:
                        BorrarMateria(context);
                        break;
                    case 3:
                        BorrarMaestro(context);
                        break;
                    case 4:
                        BorrarAlumno(context);
                        break;
                    case 0:
                        Console.Clear();
                        break;
                    default:
                        MostrarOpcion("Opcion Invalida");
                        break;
                }
            } while (opcion != 0);
        }
        #endregion
        #region BorrarCarrera
        public static void BorrarCarrera(EFInstitutoContext context) {
            try {
                ShowHeader("Borrar: >>> CARRERA <<<");
                Console.WriteLine(" Lista de Carrera: \n");
                Carrera Carrera = new Carrera();
                MostrarCarrera(context);
                Console.Write(" Id de la Carrera: ");
                int id = int.Parse(Console.ReadLine());
                Carrera = context.Carrera.Where(x => x.CarreraId == id).FirstOrDefault();
                context.Carrera.Remove(Carrera);
                context.SaveChanges();
                MostrarOpcion(" DATO BORRADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region BorrarMateria
        public static void BorrarMateria(EFInstitutoContext context) {
            try {
                ShowHeader("Borrar: >>> MATERIA <<<");
                Console.WriteLine(" Lista de Materia: \n");
                Materia Materia = new Materia();
                MostrarMateria(context);
                Console.Write(" Id de la Materia: ");
                int id = int.Parse(Console.ReadLine());
                Materia = context.Materia.Where(x => x.MateriaId == id).FirstOrDefault();
                context.Materia.Remove(Materia);
                context.SaveChanges();
                MostrarOpcion(" DATO BORRADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region BorrarMaestro
        public static void BorrarMaestro(EFInstitutoContext context) {
            try {
                ShowHeader("Borrar: >>> MAESTRO <<<");
                Console.WriteLine(" Lista de Maestro: \n");
                Maestro Maestro = new Maestro();
                MostrarMaestro(context);
                Console.Write(" Id del Maestro: ");
                int id = int.Parse(Console.ReadLine());
                Maestro = context.Maestro.Where(x => x.MaestroId == id).FirstOrDefault();
                context.Maestro.Remove(Maestro);
                context.SaveChanges();
                MostrarOpcion(" DATO BORRADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion
        #region BorrarAlumno
        public static void BorrarAlumno(EFInstitutoContext context) {
            try {
                ShowHeader("Borrar: >>> ALUMNO<<<");
                Console.WriteLine(" Lista de Alumno: \n");
                Alumno Alumno = new Alumno();
                MostrarAlumno(context);
                Console.Write(" Id del Alumno: ");
                int id = int.Parse(Console.ReadLine());
                Alumno = context.Alumno.Where(x => x.AlumnoId == id).FirstOrDefault();
                context.Alumno.Remove(Alumno);
                context.SaveChanges();
                MostrarOpcion(" DATO BORRADO");
            }
            catch (Exception error) {
                Console.WriteLine("\n\n" + error.Message);
                if (error.InnerException != null) {
                    Console.WriteLine(error.InnerException.Message);
                    Environment.Exit(0);
                }
            }
        }
        #endregion

        /// <summary>
        /// Muestra titulo
        /// </summary>
        /// <param name="titulo"></param>
        private static void ShowHeader(string titulo) {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(9, 0);
            Console.WriteLine(titulo);
            Console.ResetColor();
            Console.WriteLine();
        }
        private static void MostrarOpcion(string titulo) {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(titulo);
            Console.ResetColor();
            Thread.Sleep(1200);
            Console.WriteLine();
        }
    }
}
