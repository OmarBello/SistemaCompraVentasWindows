﻿using Sistema.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema.Presentacion
{
    public partial class FrmIngreso : Form
    {
        private DataTable DtDetalle = new DataTable();
        public FrmIngreso()
        {
            InitializeComponent();
        }
        private void Listar()
        {
            try
            {
                DgvListado.DataSource = NIngreso.Listar();
                this.Formato();
                this.Limpiar();
                LblTotal.Text = "Total registros: " + Convert.ToString(DgvListado.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Buscar()
        {
            try
            {
                DgvListado.DataSource = NIngreso.Buscar(TxtBuscar.Text);
                this.Formato();
                LblTotal.Text = "Total registros: " + Convert.ToString(DgvListado.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void Formato()
        {
            DgvListado.Columns[0].Visible = false;
            DgvListado.Columns[1].Visible = false;
            DgvListado.Columns[2].Visible = false;
            DgvListado.Columns[0].Width = 100;
            DgvListado.Columns[3].Width = 150;
            DgvListado.Columns[4].Width = 150;
            DgvListado.Columns[5].Width = 100;
            DgvListado.Columns[5].HeaderText = "Documento";
            DgvListado.Columns[6].Width = 70;
            DgvListado.Columns[6].HeaderText = "Serie";
            DgvListado.Columns[7].Width = 70;
            DgvListado.Columns[7].HeaderText = "Numero";
            DgvListado.Columns[8].Width = 60;
            DgvListado.Columns[9].Width = 100;
            DgvListado.Columns[10].Width = 100;
            DgvListado.Columns[11].Width = 100;
        }

        private void Limpiar()
        {
            TxtBuscar.Clear();
            TxtId.Clear();
            BtnInsertar.Visible = true;
            ErrorIcono.Clear();

            DgvListado.Columns[0].Visible = false;
            BtnAnular.Visible = false;
            ChkSeleccionar.Checked = false;

        }

        private void MensajeError(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Sistema de ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MensajeOK(string Mensaje)
        {
            MessageBox.Show(Mensaje, "Sistema de ventas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CrearTabla()
        {
            this.DtDetalle.Columns.Add("idarticulo", System.Type.GetType("System.Int32"));
            this.DtDetalle.Columns.Add("codigo", System.Type.GetType("System.String"));
            this.DtDetalle.Columns.Add("articulo", System.Type.GetType("System.String"));
            this.DtDetalle.Columns.Add("cantidad", System.Type.GetType("System.Int32"));
            this.DtDetalle.Columns.Add("precio", System.Type.GetType("System.Decimal"));
            this.DtDetalle.Columns.Add("importe", System.Type.GetType("System.Decimal"));

            DgvDetalle.DataSource = this.DtDetalle;

            DgvDetalle.Columns[0].Visible = false;
            DgvDetalle.Columns[1].HeaderText = "CODIGO";
            DgvDetalle.Columns[1].Width = 100;
            DgvDetalle.Columns[2].HeaderText = "ARTICULO";
            DgvDetalle.Columns[2].Width = 200;
            DgvDetalle.Columns[3].HeaderText = "CANTIDAD";
            DgvDetalle.Columns[3].Width = 70;
            DgvDetalle.Columns[4].HeaderText = "PRECIO";
            DgvDetalle.Columns[4].Width = 70;
            DgvDetalle.Columns[5].HeaderText = "IMPORTE";
            DgvDetalle.Columns[5].Width = 80;

            DgvDetalle.Columns[1].ReadOnly = true;
            DgvDetalle.Columns[2].ReadOnly = true;
            DgvDetalle.Columns[5].ReadOnly = true;

        }
        private void FrmIngreso_Load(object sender, EventArgs e)
        {
            this.Listar();
            this.CrearTabla();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        private void BtnBuscarProveedor_Click(object sender, EventArgs e)
        {
            FrmVista_ProveedorIngreso vista = new FrmVista_ProveedorIngreso();
            vista.ShowDialog();
            TxtIdProveedor.Text = Convert.ToString(Variables.IdProveedor);
            TxtNombreProveedor.Text = Variables.NombreProveedor;
        }

        private void TxtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if(e.KeyCode == Keys.Enter)
                {
                    DataTable Tabla = new DataTable();
                    Tabla = NArticulo.BuscarCodigo(TxtCodigo.Text.Trim());

                    if (Tabla.Rows.Count <= 0)
                    {
                        this.MensajeError("No existe articulo con ese codigo de barras");
                    }
                    else
                    {
                        //Agregar al detalle
                        this.AgregarDetalle(Convert.ToInt32(Tabla.Rows[0][0]), Convert.ToString(Tabla.Rows[0][1]), Convert.ToString(Tabla.Rows[0][2]),Convert.ToDecimal(Tabla.Rows[0][3]));
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AgregarDetalle(int IdArticulo, string Codigo, string Nombre, decimal Precio)
        {
            DataRow Fila = DtDetalle.NewRow();
            Fila["idarticulo"] = IdArticulo;
            Fila["codigo"] = Codigo;
            Fila["articulo"] = Nombre;
            Fila["cantidad"] = 1;
            Fila["precio"] = Precio;
            Fila["importe"] = Precio;
            this.DtDetalle.Rows.Add(Fila);
        }
    }
}
