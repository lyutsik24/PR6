using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PR6
{
    public partial class Main : Form
    {
        string title = "Vet";
        int IdPet;
        int IdMedicine;
        int IdVaccination;
        int IdTreatment;
        bool PetSelected;
        bool MedicineSelected;
        bool VaccionationSelected;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.vaccinationTableAdapter.Fill(this.veterinaryDataSet.Vaccination);
            this.medicineTableAdapter.Fill(this.veterinaryDataSet.Medicine);
            this.petTableAdapter.Fill(this.veterinaryDataSet.Pet);
            FillDgv();
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string queary = "INSERT INTO `Veterinary`.`Pet` (`PetName`, `Gender`, `Breed`, `Date_of_birth`, `Date_of_reg`) VALUES (@pname, @pgender, @pbreed, @pdatebirth, @pdatereg)";
                    MySqlCommand cm = new MySqlCommand(queary, cn);
                    cm.Parameters.AddWithValue("@pname", txtName.Text);
                    cm.Parameters.AddWithValue("@pgender", cmbGender.SelectedItem);
                    cm.Parameters.AddWithValue("@pbreed", cmbBreed.SelectedItem);
                    cm.Parameters.AddWithValue("@pdatebirth", dtpBirth.Value.Date.ToString("yyyy-MM-dd"));
                    cm.Parameters.AddWithValue("@pdatereg", dtpReg.Value.Date.ToString("yyyy-MM-dd"));
                    cn.Open();

                    if (cm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Питомец добавлен!", title);
                        Clear();
                        RefrPet();
                    }


                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                if (MessageBox.Show("Вы уверены что хотите изменить?", "Pet Edited", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    try
                    {
                        string queary = "UPDATE `Veterinary`.`Pet` SET `PetName` = @pname, `Gender` = @pgender, `Breed` = @pbreed, `Date_of_birth` = @pdatebirth, `Date_of_reg` = @pdatereg WHERE ID_Pet=@pcode";
                        MySqlCommand cm = new MySqlCommand(queary, cn);
                        cm.Parameters.AddWithValue("pcode", lblPcode.Text);
                        cm.Parameters.AddWithValue("@pname", txtName.Text);
                        cm.Parameters.AddWithValue("@pgender", cmbGender.SelectedItem);
                        cm.Parameters.AddWithValue("@pbreed", cmbBreed.SelectedItem);
                        cm.Parameters.AddWithValue("@pdatebirth", dtpBirth.Value.Date.ToString("yyyy-MM-dd"));
                        cm.Parameters.AddWithValue("@pdatereg", dtpReg.Value.Date.ToString("yyyy-MM-dd"));
                        cn.Open();

                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Пет изменён!", title);
                            RefrPet();
                            Btn_Add.Enabled = true;
                            lblPcode.Text = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        cn.Close();
                        MessageBox.Show(ex.Message, title);
                    }
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    DialogResult res = MessageBox.Show("Вы уверены что хотите удалить данный материал?", "Product Deleted", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        string queary = "DELETE FROM `Veterinary`.`Pet` WHERE (`ID_Pet` = @id)";
                        MySqlCommand cm = new MySqlCommand(queary, cn);
                        cm.Parameters.AddWithValue("@id", IdPet);
                        cn.Open();

                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Пет удалён!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Clear();
                            RefrPet();
                        }
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btn2_add_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string queary = "INSERT INTO `Veterinary`.`Medicine` (`Name`, `Cost`) VALUES (@pname, @pcost)";
                    MySqlCommand cm = new MySqlCommand(queary, cn);
                    cm.Parameters.AddWithValue("@pname", txtName2.Text);
                    cm.Parameters.AddWithValue("@pcost", txtCost2.Text);
                    cn.Open();

                    if (cm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Лекарство добавлено!", title);
                        RefrMed();
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btn3_add_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string queary = "INSERT INTO `Veterinary`.`Vaccination` (`Name`, `CostVac`) VALUES (@pname, @pcost)";
                    MySqlCommand cm = new MySqlCommand(queary, cn);
                    cm.Parameters.AddWithValue("@pname", txtName3.Text);
                    cm.Parameters.AddWithValue("@pcost", txtCost3.Text);
                    cn.Open();

                    if (cm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Прививка добавлена!", title);
                        RefrVac();
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btn2_delete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    DialogResult res = MessageBox.Show("Вы уверены что хотите удалить?", "Medicine Deleted", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        string queary = "DELETE FROM `Veterinary`.`Medicine` WHERE (`ID_Medicine` = @id)";
                        MySqlCommand cm = new MySqlCommand(queary, cn);
                        cm.Parameters.AddWithValue("@id", IdMedicine);
                        cn.Open();

                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Лекарство удалено!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            RefrMed();
                        }
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btn3_delete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    DialogResult res = MessageBox.Show("Вы уверены что хотите удалить?", "Vaccionation Deleted", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        string queary = "DELETE FROM `Veterinary`.`Vaccination` WHERE (`ID_Vaccination` = @id)";
                        MySqlCommand cm = new MySqlCommand(queary, cn);
                        cm.Parameters.AddWithValue("@id", IdVaccination);
                        cn.Open();

                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Прививка удалена!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            RefrVac();
                        }
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btn4_add_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string queary = "INSERT INTO `Veterinary`.`Treatment` (`Pet_ID`, `Medicine_ID`, `Vaccination_ID`) VALUES (@pname, @pmed, @pvac)";
                    MySqlCommand cm = new MySqlCommand(queary, cn);
                    cm.Parameters.AddWithValue("@pname", cmbPet.SelectedValue);
                    cm.Parameters.AddWithValue("@pmed", cmbMed.SelectedValue);
                    cm.Parameters.AddWithValue("@pvac", cmbVac.SelectedValue);
                    cn.Open();

                    if (cm.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Добавлено!", title);
                        FillDgv();
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void btn4_delete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    DialogResult res = MessageBox.Show("Вы уверены что хотите удалить?", "Treatment Deleted", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        string queary = "DELETE FROM `Veterinary`.`Treatment` WHERE (`ID_Treatment` = @id)";
                        MySqlCommand cm = new MySqlCommand(queary, cn);
                        cm.Parameters.AddWithValue("@id", IdTreatment);
                        cn.Open();

                        if (cm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Удалено!", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            FillDgv();
                        }
                    }
                }
                catch (Exception ex)
                {
                    cn.Close();
                    MessageBox.Show(ex.Message, title);
                }
            }
        }

        private void dgvPet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvPet.Rows[e.RowIndex];
                IdPet = Convert.ToInt32(selectedRow.Cells[0].Value);
                PetSelected = true;
            }
        }

        private void dgvPet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvPet.Columns[e.ColumnIndex].Name;
            lblPcode.Text = dgvPet.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvPet.Rows[e.RowIndex].Cells[1].Value.ToString();
            Btn_Add.Enabled = false;
            btn_Edit.Enabled = true;
        }

        private void dgvMedicine_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvMedicine.Rows[e.RowIndex];
                IdMedicine = Convert.ToInt32(selectedRow.Cells[0].Value);
                MedicineSelected = true;
            }
        }

        private void dgvVaccination_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dgvVaccination.Rows[e.RowIndex];
                IdVaccination = Convert.ToInt32(selectedRow.Cells[0].Value);
                VaccionationSelected = true;
            }
        }

        public void RefrMed()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string queary = "SELECT * FROM `Veterinary`.`Medicine`";
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    DataTable dt = new DataTable();
                    MySqlCommand cm = new MySqlCommand(queary, cn);

                    da.SelectCommand = cm;
                    da.Fill(dt);
                    dgvMedicine.DataSource = dt;
                }
                catch (Exception)
                {
                    cn.Close();
                }
            }
        }

        public void RefrVac()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string queary = "SELECT * FROM `Veterinary`.`Vaccination`";
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    DataTable dt = new DataTable();
                    MySqlCommand cm = new MySqlCommand(queary, cn);

                    da.SelectCommand = cm;
                    da.Fill(dt);
                    dgvVaccination.DataSource = dt;
                }
                catch (Exception)
                {
                    cn.Close();
                }
            }
        }

        private void FillDgv()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string query = @"SELECT `ID_Treatment` AS `№`, `PetName` AS `Питомец`, `Cost` AS `Лекарство`, `CostVac` AS `Вакцина`
                                    FROM `Veterinary`.`Treatment`
                                    JOIN `Veterinary`.`Pet` ON `Veterinary`.`Treatment`.`Pet_ID` = `Veterinary`.`Pet`.`ID_Pet`
                                    JOIN `Veterinary`.`Medicine` ON `Veterinary`.`Treatment`.`Medicine_ID` = `Veterinary`.`Medicine`.`ID_Medicine`
                                    JOIN `Veterinary`.`Vaccination` ON `Veterinary`.`Treatment`.`Vaccination_ID` = `Veterinary`.`Vaccination`.`ID_Vaccination`
                                    ORDER BY `ID_Treatment`";
                    cn.Open();
                    MySqlCommand cm = new MySqlCommand(query, cn);
                    MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                    DataTable dt = new DataTable();
                    da.SelectCommand = cm;
                    da.Fill(dt);
                    dgvTreatment.DataSource = dt;

                    dgvTreatment.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    dgvTreatment.Columns[0].DisplayIndex = 0;
                    dgvTreatment.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvTreatment.Columns[1].DisplayIndex = 1;
                    dgvTreatment.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvTreatment.Columns[2].DisplayIndex = 2;
                    dgvTreatment.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvTreatment.Columns[3].DisplayIndex = 3;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void Clear()
        {
            txtName.Clear();
            cmbBreed.SelectedIndex = 0;
            cmbGender.SelectedIndex = 0;
        }

        public void RefrPet()
        {
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.VeterinaryConnectionString))
            {
                try
                {
                    string queary = "SELECT * FROM `Veterinary`.`Pet`";
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    DataTable dt = new DataTable();
                    MySqlCommand cm = new MySqlCommand(queary, cn);

                    da.SelectCommand = cm;
                    da.Fill(dt);
                    dgvPet.DataSource = dt;
                }
                catch (Exception)
                {
                    cn.Close();
                }
            }
        }
    }
}
