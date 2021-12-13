using System;
using System.Windows.Forms;
using EntidadesRSP;


namespace WinFormsAppRSP
{
    public partial class FrmAlumno : Form
    {
        #region Atributo
        private Alumno alumno;
        #endregion

        #region Propiedad
        public Alumno Alumno
        {
            get { return alumno; }
        }
        #endregion

        #region Constructor
        public FrmAlumno()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public FrmAlumno(Alumno alum, EFormAlta param) : this()
        {
            this.alumno = alum;

            this.txtNombre.Text = this.alumno.Nombre;
            this.txtApellido.Text = this.alumno.Apellido;
            this.txtDNI.Text = this.alumno.Dni.ToString();
            this.txtNota.Text = this.alumno.Nota.ToString();
            this.txtDNI.Enabled = false;

            if (param == EFormAlta.Eliminar)
            {
                this.txtNombre.Enabled = false;
                this.txtApellido.Enabled = false;
                this.txtNota.Enabled = false;
            }
        }

        #endregion

        #region Manejadores
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(         !String.IsNullOrEmpty(this.txtApellido.Text) &&
                            !String.IsNullOrEmpty(this.txtDNI.Text) &&
                                !String.IsNullOrEmpty(this.txtNombre.Text) &&
                                    !String.IsNullOrEmpty(this.txtNota.Text))
            {
                 
                double nota = Convert.ToDouble(this.txtNota.Text);
                int dni = Convert.ToInt32(this.txtDNI.Text);
                string nombre = this.txtNombre.Text;
                string apellido = this.txtApellido.Text;

                this.alumno = new Alumno(dni, apellido, nombre, nota);

                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Debe ingresar todos los campos!");
            }
            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        } 
        #endregion
    }
}
