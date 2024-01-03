using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.Common;

namespace Login
{
    public partial class Graphics : Form
    {
        private readonly string connectionString = $"Provider = Microsoft.ACE.OLEDB.12.0;Data Source={Application.StartupPath}\\HMS.accdb";
        

        public Graphics()
        {
            InitializeComponent();
           
        }

        //Fetch Information

        private void ShowInfo()
        {
            dataGridView1.Rows.Clear();
            using (var con = new OleDbConnection(connectionString))
            {
                con.Open();
                using (var cmd = new OleDbCommand("SELECT MajorDisease FROM patient", con))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (dr.Read())
                            {
                                dataGridView1.Rows.Add(dr["MajorDisease"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Database cannot connect. Error:" + ex.Message);
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                chart1.Series.Clear();

                OleDbConnection con = new OleDbConnection(connectionString);
                con.Open();
                OleDbCommand AccessCommand = new OleDbCommand();
                AccessCommand.Connection = con;
                AccessCommand.CommandText = ("SELECT MajorDisease, COUNT(*) AS DiseaseCount FROM patient GROUP BY MajorDisease");
                OleDbDataReader read = AccessCommand.ExecuteReader();

                
                chart1.Palette = ChartColorPalette.BrightPastel;

                while (read.Read())
                {
                    string majordisease = read["MajorDisease"].ToString();
                    int diseaseCount = Convert.ToInt32(read["DiseaseCount"]);

                    // Creating Column series
                    Series series = new Series(majordisease);
                    series.ChartType = SeriesChartType.Column;

                    // Setting a unique color for each series
                    series.Points.AddXY(majordisease, diseaseCount);
                   
                   

                    // Inserting chart series
                    chart1.Series.Add(series);
                    
                    chart1.ChartAreas[0].AxisX.Title = "Major Disease";
                    
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message);
            }
        }







        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Graphics_Load(object sender, EventArgs e)
        {
            ShowInfo();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Home h = new Home();
            h.Show();
            this.Hide();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
