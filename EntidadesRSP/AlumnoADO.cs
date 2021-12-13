using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace EntidadesRSP
{
    public class AlumnoADO : Alumno
    {
        #region Atributos
        private static SqlConnection connection;
        private static SqlCommand command;
        #endregion

        #region Constructores
        static AlumnoADO()
        {
            SqlConnectionStringBuilder strConexion = new SqlConnectionStringBuilder();
            strConexion.DataSource = @"DIEGO-PC\SQLEXPRESS";
            strConexion.InitialCatalog = @"utn_fra_alumnos";
            strConexion.IntegratedSecurity = true;

            command = new SqlCommand();
            connection = new SqlConnection(strConexion.ToString());
            command.Connection = connection;
            command.CommandType = System.Data.CommandType.Text;
        }

        public AlumnoADO(Alumno alu) 
                        : base(alu.Dni, alu.Apellido, alu.Nombre, alu.Nota)
        {

        }

        #endregion

        #region Metodos

        public static List<Alumno> ObtenerTodos()
        {
            List<Alumno> list = new List<Alumno>();

            try
            {
                command.Parameters.Clear();

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                command.CommandText = $"SELECT * FROM alumnos";
               
                SqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    // Obtengo los datos.
                    int dni = Convert.ToInt32(dataReader["dni"]);
                    string nombre = dataReader["nombre"].ToString();
                    string apellido = dataReader["apellido"].ToString();
                    double nota = Convert.ToDouble(dataReader["nota"]);

                    // Creo el objecto con los datos.
                    Alumno alumni = new Alumno(dni, apellido, nombre, nota);

                    // Agrego a la lista.
                    list.Add(alumni);
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            finally
            {
                connection.Close();
            }

            return list;
        }
        public bool Agregar()
        {
            bool ret = true;

            try
            {
                command.Parameters.Clear();

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                command.CommandText = $"INSERT INTO alumnos (dni, nombre, apellido, nota)   " +
                    $"                  VALUES (@dni,@nombre,@apellido,@nota) ";

                command.Parameters.AddWithValue("@dni", this.Dni);
                command.Parameters.AddWithValue("@nombre", this.Nombre);
                command.Parameters.AddWithValue("@apellido", this.Apellido);
                command.Parameters.AddWithValue("@nota", this.Nota);

                command.ExecuteNonQuery();

            }

            catch (Exception)
            {
                ret = false;
            }

            finally
            {
                connection.Close();
            }

            return ret;
        }
        public bool Eliminar()
        {
            bool ret = true;
            int dni = this.Dni;
            try
            {
                command.Parameters.Clear();
                connection.Open();
                command.CommandText = $"DELETE FROM alumnos WHERE dni = {dni}";
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                ret = false;
            }
            finally
            {
                connection.Close();
            }

            return ret;
        }

        public bool Modificar()
        {
            bool ret = true;
            int dni = this.Dni;
            try
            {
                command.Parameters.Clear();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                command.CommandText = $"UPDATE alumnos SET nombre = @nombre , apellido = @apellido , nota = @nota  WHERE dni = {dni} ";


                command.Parameters.AddWithValue("@nombre", this.Nombre);
                command.Parameters.AddWithValue("@apellido", this.Apellido);
                command.Parameters.AddWithValue("@nota", this.Nota);
                
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                ret = false;
            }

            finally
            {
                connection.Close();
            }

            return ret;
        }

        #endregion
    }
}
/*///AlumnoADO (hereda de Alumno)
        ///Atributos (estáticos)
        ///conexion : string
        ///connection : SqlConnection
        ///command : SqlCommand
        ///Constructor de clase que inicialice todos sus atributos
        ///Constructor que recibe un objeto de tipo Alumno cómo parámetro
        ////// static ObtenerTodos() : List<Alumno> 
        ///Métodos de instancia (públicos):
        ///Agregar() : bool
        ///Modificar() : bool -> se modifica a partir del dni
        ///Eliminar() : bool -> se elimina a partir del dni
        ///*/