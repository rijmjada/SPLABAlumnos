using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesRSP
{
    #region Delegado

    public delegate void AlumnoNota(object alumni, EventArgs e);

    #endregion
    public class Alumno : Persona
    {
        #region Eventos

        public event AlumnoNota Aprobar;
        public event AlumnoNota NoAprobar;
        public event AlumnoNota Promocionar;

        #endregion

        #region Atributos
        private double nota;
        #endregion

        #region Propiedades
        public double Nota { get => nota; set => nota = value; }
        #endregion

        #region Constructores

        public Alumno(int dni, string apellido, string nombre, double nota) 
                     : base(dni, apellido, nombre)
        {
            this.Nota = nota;
        }

        public Alumno()
        { }

        #endregion

        #region Metodos
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();

            ret.AppendLine(base.ToString());
            ret.AppendLine(" NOTA: " + this.Nota);

            return ret.ToString();
        }

        public void Clasificar()
        {
            if(this.Nota < 4)
            {
                this.NoAprobar(this,EventArgs.Empty);
            }
            if(nota < 6 && nota >= 4)
            {
                this.Aprobar(this, EventArgs.Empty);
            }
            if(nota >= 6)
            {
                this.Promocionar(this, EventArgs.Empty);
            }
        }

        #endregion

    }
}
/*///Alumno (deriva de Persona)
        ///Atributo
        ///nota : double
        ///Propiedad pública de lectura y escritura para su atributo.
        ///Constructor que reciba parámetro para cada atributo
        ///Polimorfismo sobre ToString
        ///Eventos (diseñados según convenciones vistas)
        ///Aprobar
        ///NoAprobar
        ///Promocionar
        ///Método de instancia (público)
        ///Clasificar() : void
        ///Si el atributo nota es menor a 4, lanzará el evento NoAprobar.
        ///Si el atributo nota es menor a 6 (y mayor o igual a 4), lanzará el evento Aprobar.
        ///Si el atributo nota es mayor o igual a 6, lanzará el evento Promocionar.
        ///*/