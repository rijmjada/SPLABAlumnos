using System;
using System.Text;

namespace EntidadesRSP
{
    public class Persona
    {
        #region Atributos
        private int dni;
        private string apellido;
        private string nombre;
        #endregion

        #region Propiedades
        public int Dni { get => dni; set => dni = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        #endregion

        #region Constructores
        public Persona(int dni, string apellido, string nombre)
        {
            this.Dni = dni;
            this.Apellido = apellido;
            this.Nombre = nombre;
        }

        public Persona()
        { }

        #endregion

        #region Metodos
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.AppendLine(" DNI: " + this.Dni);
            ret.AppendLine(" APELLIDO: " + this.Apellido);
            ret.AppendLine(" NOMBRE: " + this.Nombre);
            

            return ret.ToString();
        } 

        #endregion
    }
}
/*  
 ///Persona
        ///Atributos (todos privados)
        ///dni : int
        ///apellido : string
        ///nombre : string
        ///Propiedades públicas de lectura y escritura para todos sus atributos.
        ///Constructor que reciba parámetros para cada atributo
        ///Polimorfismo sobre ToString
 */