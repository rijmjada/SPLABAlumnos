using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntidadesRSP;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace WinFormsAppRSP
{
    ///Agregar manejo de excepciones en TODOS los lugares críticos!!!
    public delegate void DelegadoTaskSinParam();
    public partial class FrmPrincipal : Form
    {
        ///Crear en un proyecto de tipo class library (EntidadesRSP), las clases:
        ///Persona
        ///Atributos (todos privados)
        ///dni : int
        ///apellido : string
        ///nombre : string
        ///Propiedades públicas de lectura y escritura para todos sus atributos.
        ///Constructor que reciba parámetros para cada atributo
        ///Polimorfismo sobre ToString
        ///
        ///Alumno (deriva de Persona)
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
        ///
        ///AlumnoADO (hereda de Alumno)
        ///Atributos (estáticos)
        ///conexion : string
        ///connection : SqlConnection
        ///command : SqlCommand
        ///Constructor de clase que inicialice todos sus atributos
        ///Constructor que recibe un objeto de tipo Alumno cómo parámetro
        ///Métodos de instancia (públicos):
        ///ObtenerTodos() : List<Alumno> 
        ///Agregar() : bool
        ///Modificar() : bool -> se modifica a partir del dni
        ///Eliminar() : bool -> se elimina a partir del dni

        ///BASE DE DATOS
        ///Crear la BASE de DATOS utn_fra_alumnos y ejecutar el siguiente script:
        ///USE [utn_fra_alumnos]
        ///GO
        ///CREATE TABLE [dbo].[alumnos]
        ///(
        ///[dni] [int] NOT NULL,
        ///[apellido] [varchar](50) NOT NULL,
        ///[nombre] [varchar](50) NOT NULL,
        ///[nota] [float] NOT NULL,
        ///) ON[PRIMARY]
        ///GO
        ///
        static string file = CrearDirectorioYArchivo();

        private static string CrearDirectorioYArchivo()
        {
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Test Xml");

            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            return ruta = Path.Combine(ruta, "datos.xml");
        }

        private List<EntidadesRSP.Alumno> alumnos;
        public FrmPrincipal()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ormeño Diego";
            MessageBox.Show(this.Text);


            this.CargarListados();

            ///Agregar los manejadores de eventos para: 
            ///btnAgregar, btnEliminar, btnModificar, btnSerializar, btnDeserializar y btnHilos
            ///

            #region Resuelto

            this.btnAgregar.Click += new EventHandler(this.ManejadorAgregar);
            this.btnModificar.Click += new EventHandler(this.ManejadorModificar);
            this.btnEliminar.Click += new EventHandler(this.ManejadorEliminar);
            this.btnSerializar.Click += new EventHandler(this.ManejadorSerializar);
            this.btnDeserializar.Click += new EventHandler(this.ManejadorDeserializar);
            this.btnHilos.Click += new EventHandler(this.ManejadorHilos); 

            #endregion

        }

        private void CargarListados()
        {
            this.lstAprobados.Items.Clear();
            this.lstDesaprobados.Items.Clear();
            this.lstPromocionados.Items.Clear();

            ///Utilizando la clase AlumnoADO, obtener y mostrar todos los productos
            ///
            this.alumnos = AlumnoADO.ObtenerTodos();
            this.lstTodos.DataSource = this.alumnos;


            foreach (Alumno item in this.alumnos)
            {
                ///Agregar, para los eventos
                ///Aprobar, NoAprobar y Promocionar, los manejadores
                ///AlumnoAprobado, AlumnoNoAprobado y AlumnoPromocionado, respectivamente
                ///

                #region Resuelto
                item.Aprobar += this.AlumnoAprobado;
                item.NoAprobar += this.AlumnoNoAprobado;
                item.Promocionar += this.AlumnoPromocionado; 
                #endregion

                item.Clasificar();

                ///Quitar los manejadores de eventos para:
                ///Aprobar, NoAprobar y Promocionar
                ///

                #region Resuelto
                item.Aprobar -= this.AlumnoAprobado;
                item.NoAprobar -= this.AlumnoNoAprobado;
                item.Promocionar -= this.AlumnoPromocionado;
                #endregion
            }
        }

        private void ManejadorAgregar(object emisor, EventArgs argumentos)
        {
            ///Agregar un nuevo alumno a la base de datos
            ///Utilizar FrmAlumno.
            ///Si se pudo agregar, invocar al método CargarListados, caso contrario mostrar mensaje
            ///
            FrmAlumno frmAlumnito = new FrmAlumno();

            if(frmAlumnito.ShowDialog() == DialogResult.OK)
            {
                AlumnoADO alumAdo = new AlumnoADO(frmAlumnito.Alumno);

                if (alumAdo.Agregar())
                {
                    this.CargarListados();
                }
                else
                {
                    MessageBox.Show("Hubo un error");
                }
            }
        }

        private void ManejadorModificar(object emisor, EventArgs argumentos)
        {
            ///Modificar el alumno seleccionado (el dni no se debe modificar, adecuar FrmAlumno)
            ///Se deben mostrar todos los datos en el formulario (adaptarlo)
            ///reutilizar FrmAlumno.
            ///Si se pudo modificar, invocar al método CargarListados, caso contrario mostrar mensaje
            ///

            FrmAlumno frmAlumnito = new FrmAlumno((Alumno)this.lstTodos.SelectedItem,EFormAlta.Modificar);

            if (frmAlumnito.ShowDialog() == DialogResult.OK)
            {
                AlumnoADO alumAdo = new AlumnoADO(frmAlumnito.Alumno);

                if (alumAdo.Modificar())
                {
                    this.CargarListados();
                }
                else
                {
                    MessageBox.Show("Hubo un error");
                }
            }

        }

        private void ManejadorEliminar(object emisor, EventArgs argumentos)
        {
            ///Eliminar el alumno seleccionado (el dni no se debe modificar, adecuar FrmAlumno)
            ///Se deben mostrar todos los datos en el formulario (adaptarlo)
            ///reutilizar FrmAlumno.
            ///Si se pudo eliminar, invocar al método CargarListados, caso contrario mostrar mensaje
            ///
            FrmAlumno frmAlumnito = new FrmAlumno((Alumno)this.lstTodos.SelectedItem, EFormAlta.Eliminar);

            if (frmAlumnito.ShowDialog() == DialogResult.OK)
            {
                AlumnoADO alumAdo = new AlumnoADO(frmAlumnito.Alumno);

                if (alumAdo.Eliminar())
                {
                    this.CargarListados();
                }
                else
                {
                    MessageBox.Show("Hubo un error");
                }
            }
        }

        private void ManejadorSerializar(object emisor, EventArgs argumentos)
        {
            ///Serializar a XML la lista de alumnos del formulario (this.alumnos)
            ///El archivo .xml guardarlo en el escritorio del cliente, 
            ///con el nombre formado con su apellido.nombre.alumnos.xml
            ///Ejemplo: Alumno Juan Pérez -> perez.juan.alumnos.xml
            ///Indicar si se pudo o no serializar la lista de alumnos
            ///
            try
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    XmlSerializer xs = new XmlSerializer(this.alumnos.GetType());
                    xs.Serialize(sw, this.alumnos);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ManejadorDeserializar(object emisor, EventArgs argumentos)
        {
            ///Deserializar de XML a una lista de alumnos
            ///El archivo .xml tomarlo del escritorio del cliente, 
            ///con el nombre formado con su apellido.nombre.alumnos.xml
            ///Ejemplo: Alumno Juan Pérez -> perez.juan.alumnos.xml
            ///Si se pudo serializar, mostrar el listado completo en un MessageBox.
            ///Si no se pudo deserializar, mostrar los motivos
            ///
            try
            {
                List<Alumno> aux = new List<Alumno>();

                using (StreamReader sw = new StreamReader(file))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<Alumno>));
                    aux = xs.Deserialize(sw) as List<Alumno>;
                }

                foreach(Alumno item in aux)
                {
                    MessageBox.Show(item.ToString());
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        public void AlumnoNoAprobado(object alumni, EventArgs e)
        {
            ///Agregar el alumno al listado lstDesaprobados
            ///
            this.lstDesaprobados.Items.Add(alumni);
        }

        public void AlumnoAprobado(object alumni, EventArgs e)
        {
            ///Agregar el alumno al listado lstAprobados
            ///
            this.lstAprobados.Items.Add(alumni);
        }

        public void AlumnoPromocionado(object alumni, EventArgs e)
        {
            ///Agregar el alumno al listado lstPromocionados
            ///
            this.lstPromocionados.Items.Add(alumni);
        }

        private void ManejadorHilos(object emisor, EventArgs argumentos)
        {
            ///Iniciar una nueva tarea en segundo plano que, para lstDesaprobados:
            ///cambie el color de fondo (a rojo) y el color de la fuente (a blanco)
            ///y lo intercambie (fondo a blanco y fuente a rojo) 10 veces,
            ///agregando un retardo de 1 segundo por cada intercambio.
            ///
            ///NOTA: propiedades BackColor (fondo) y ForeColor (fuente)
            ///colores: 
            ///System.Drawing.Color.Red (rojo)
            ///System.Drawing.Color.White (blanco)

            Task tareaSegundoPlano = Task.Run(() => this.ModificarDesaprobados());
        }

        public void ModificarDesaprobados()
        {
            if (this.lstDesaprobados.InvokeRequired)
            {
                this.lstDesaprobados.BeginInvoke((MethodInvoker)async delegate ()
                {
                    for (int i = 0; i < 10; i++)
                    {
                        this.lstDesaprobados.BackColor = System.Drawing.Color.Red;
                        this.lstDesaprobados.ForeColor = System.Drawing.Color.White;

                        await Task.Delay(500);


                        this.lstDesaprobados.BackColor = System.Drawing.Color.White;
                        this.lstDesaprobados.ForeColor = System.Drawing.Color.Red;

                        await Task.Delay(500);
                    }
                });

            }
        }
    }
}
