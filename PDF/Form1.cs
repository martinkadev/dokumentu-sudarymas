using Spire.Doc;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PDF
{
    public partial class Form1 : Form
    {
        OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+Application.StartupPath+@"\DB.accdb");
        int studID = 0;
        int tmp;

        string docPath = null;

        Random random = new Random();

        //string[] lines = System.IO.File.ReadAllLines(Application.StartupPath + @"\Duomenys\Kreditai.txt", Encoding.GetEncoding(1257));

        public Form1()
        {
            InitializeComponent();
        }

        void viewDB()
        {
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from DB";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            OleDbDataAdapter dp = new OleDbDataAdapter(cmd);
            dp.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            if (dataGridView1.Rows.Count == 0)
            {
                istrinti_irasaButton.Enabled = false;
                atnaujinti_irasaButton.Enabled = false;
            }
            else
            {
                studID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                istrinti_irasaButton.Enabled = true;
                atnaujinti_irasaButton.Enabled = true;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            if (studijuProgramosComboBox.SelectedIndex < 0){
                MessageBox.Show("Nepasirinkta studijų programa");
                return;
            }
            Document document = new Document();

            progressBar1.Value = 5;

            sudarytiDokumenta(document);

            progressBar1.Value = 40;

            document.SaveToFile(Application.StartupPath+@"\Praktikos Dokumentai\"+this.vardasTextBox.Text+" "+this.pavardeTextBox.Text+ " - "+this.dataTimePicker.Text+".docx", FileFormat.Docx);

            progressBar1.Value = 100;

            MessageBox.Show("Word dokumentas išsaugotas!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;

            if (studijuProgramosComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Nepasirinkta studijų programa");
                return;
            }
            Document document = new Document();

            progressBar1.Value = 5;
            sudarytiDokumenta(document);
            progressBar1.Value = 40;

            var randomFilePath = $"Temp\\{random.Next(1000000, 10000000)}.docx";
            document.SaveToFile(Application.StartupPath + @"\" + randomFilePath, FileFormat.Docx);
            progressBar1.Value = 50;

            var newFilePath = @"Praktikos Dokumentai\" + this.vardasTextBox.Text + " " + this.pavardeTextBox.Text + " - " + this.dataTimePicker.Text + ".pdf";

            ExecuteCommand($"OfficeToPDF.exe \"{randomFilePath}\" \"{newFilePath}\" && exit");
            progressBar1.Value = 80;
            ExecuteCommand($"del {randomFilePath} && exit");
            progressBar1.Value = 100;

            MessageBox.Show("PDF dokumentas išsaugotas!");
        }

        private void sudarytiDokumenta(Document document)
        {
            if (docPath == null)
            {
                docPath = @"\Sablonai\Praktika.docx";
            }
            document.LoadFromFile(Application.StartupPath + docPath);
            progressBar1.Value += 2;
            document.Replace("FF0", this.dataTimePicker.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF1", this.vardasTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF2", this.pavardeTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF3", this.stud_gyv_v_adresasTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF4", this.studijuProgramosComboBox.SelectedItem.ToString(), true, true);
            progressBar1.Value += 2;
            document.Replace("FF5", this.kursasComboBox.SelectedItem.ToString(), true, true);
            progressBar1.Value += 2;
            document.Replace("FF6", this.praktikosPradziaTimePicker.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF7", this.praktikosPabaigaTimePicker.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF8", this.praktikosImoneTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF9", this.imonesAtstovoKontaktaiTextBox.Text, true, true); // seniau buvo this.praktikosImonesAdresasTextBox.Text
            progressBar1.Value += 2;
            document.Replace("FF10", this.imonesAtstovoVardasPavardeTextBox.Text, true, true);
            progressBar1.Value += 2;
            //document.Replace("FF11", this.imonesATSTextBox.Text, true, true); // dabar FF22
            // progressBar1.Value += 2;
            document.Replace("FF12", this.gimimoMetaiTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF13", this.kreditaiComboBox.SelectedItem.ToString(), true, true);
            progressBar1.Value += 2;
            document.Replace("FF14", this.sutartiesNrTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF15", this.studentoTelTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF16", this.studentoPastasTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF17", this.kolegijosPraktikosVadovasTextBox.Text, true, true);
            progressBar1.Value += 2;
            if (docPath.Equals(@"\Sablonai\Praktika.docx"))
            {
                document.Replace("FF18", this.praktikosRezultataiTextBox.Text, true, true);
            }
            progressBar1.Value += 2;
            document.Replace("FF19", this.direktoriusTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF20", this.valLabel.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF21", this.ATStextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF22", this.imonesATSTextBox.Text, true, true);
            progressBar1.Value += 2;
            document.Replace("FF23", this.praktikosTikslaiTextBox.Text, true, true);
            progressBar1.Value += 2;
        }

        public void ExecuteCommand(string Command)
        {
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + Command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            progressBar1.Value += 10;

            Process = Process.Start(ProcessInfo);

            progressBar1.Value += 5;

            Process.WaitForExit();
        }

        private void Form1_Load(object sender, EventArgs e){
            viewDB();
            kursasComboBox.SelectedIndex = 3;
            kreditaiComboBox.SelectedIndex = 0;
            valLabel.Text = "135";

            specializacijaComboBox.Enabled = false;
            praktikosTipaiComboBox.Enabled = false;
            progressBar1.Value = 0;

           //ListSortDirection sortDir = new ListSortDirection();
           // sortDir
           // dataGridView1.Sort(dataGridView1.Columns["ColumnName"], );
        }

        private void prideti_irasaButton_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "insert into DB(Data,Vardas,Pavarde,StudentoNuolatinesGyvenamosiosVietosAdresas,StudijuPrograma,PraktikosTipas,Specializacija,Kursas," +
                "PraktikosPradzia,PraktikosPabaiga,PraktikosImone,ImonesAtstovoVardasPavarde,ImonesAtstovoKontaktai,ImonesATS," +
                "GimimoMetai,Kreditai,SutartiesNr,StudentoTelefonas,StudentoPastas,KolegijosPraktikosVadovas," +
                "Direktorius,ATS,PraktikosTikslai,PraktikosRezultatai)values('" +
                dataTimePicker.Text + "','" + vardasTextBox.Text + "','" + pavardeTextBox.Text + "','" +
                stud_gyv_v_adresasTextBox.Text + "','" + studijuProgramosComboBox.SelectedItem + "','" + praktikosTipaiComboBox.SelectedItem + 
                "','" + specializacijaComboBox.SelectedItem + "','" + kursasComboBox.SelectedItem + "','" +
                praktikosPradziaTimePicker.Text + "','" + praktikosPabaigaTimePicker.Text
                + "','" + praktikosImoneTextBox.Text + "','" + imonesAtstovoVardasPavardeTextBox.Text +
                "','" + imonesAtstovoKontaktaiTextBox.Text + "','" + imonesATSTextBox.Text + "','" + gimimoMetaiTextBox.Text + 
                "','" + kreditaiComboBox.SelectedItem + "','" + sutartiesNrTextBox.Text + "','" +
                studentoTelTextBox.Text + "','" + studentoPastasTextBox.Text + "','" + kolegijosPraktikosVadovasTextBox.Text + "','" +
                direktoriusTextBox.Text + "','" + ATStextBox.Text + "','" + praktikosTikslaiTextBox.Text + "','" + praktikosRezultataiTextBox.Text + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
            viewDB();

            int lastIndex = dataGridView1.AllowUserToAddRows
                ? dataGridView1.Rows.Count - 2
                : dataGridView1.Rows.Count - 1;

            if (lastIndex >= 0)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[lastIndex].Selected = true;
                dataGridView1.CurrentCell = dataGridView1.Rows[lastIndex].Cells[0];
            }
        }

        private void atnaujinti_irasaButton_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;

            DialogResult result = MessageBox.Show(
                "Jei atnaujinsite įrašą, dings buvę įrašo duomenys.\nAr tikrai norite atnaujinti įrašą?",
                "Įrašo atnaujinimas",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            tmp = dataGridView1.CurrentCell.RowIndex;
            studID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update DB set Data='" + dataTimePicker.Text + "' , Vardas='" + vardasTextBox.Text + "' , Pavarde='" + pavardeTextBox.Text+
                "' , StudentoNuolatinesGyvenamosiosVietosAdresas='" + stud_gyv_v_adresasTextBox.Text + "' , StudijuPrograma='" + studijuProgramosComboBox.SelectedItem +
                "' , PraktikosTipas='" + praktikosTipaiComboBox.SelectedItem + "' , Specializacija='" + specializacijaComboBox.SelectedItem + 
                "' , Kursas='" + kursasComboBox.SelectedItem + "' , PraktikosPradzia='" + praktikosPradziaTimePicker.Text +
                "' , PraktikosPabaiga='" + praktikosPabaigaTimePicker.Text + "' , PraktikosImone='" + praktikosImoneTextBox.Text + "' , ImonesAtstovoVardasPavarde='" +
                imonesAtstovoVardasPavardeTextBox.Text + "' , ImonesAtstovoKontaktai='" + imonesAtstovoKontaktaiTextBox.Text + "' , ImonesATS='" + imonesATSTextBox.Text +
                "' , GimimoMetai='" + gimimoMetaiTextBox.Text + "' , Kreditai='" + kreditaiComboBox.SelectedItem +
                "' , SutartiesNr='" + sutartiesNrTextBox.Text + "' , StudentoTelefonas='" + studentoTelTextBox.Text +
                "' , StudentoPastas='" + studentoPastasTextBox.Text + "' , KolegijosPraktikosVadovas='" + kolegijosPraktikosVadovasTextBox.Text + 
                "' , Direktorius='" + direktoriusTextBox.Text + "', ATS='" + ATStextBox.Text + 
                "' , PraktikosTikslai='" + praktikosTikslaiTextBox.Text + "' , PraktikosRezultatai='" + praktikosRezultataiTextBox.Text + "' where studID=" + studID + "";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "select * from DB";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            OleDbDataAdapter dp = new OleDbDataAdapter(cmd);
            dp.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            
            dataGridView1.Rows[tmp].Selected = true;
            dataGridView1.CurrentCell = dataGridView1[0, tmp];
        }

        private void istrinti_irasaButton_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;

            DialogResult result = MessageBox.Show(
                "Jei ištrinsite įrašą, dings duomenys.\nAr tikrai norite ištrinti įrašą?",
                "Įrašo ištrynimas",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "delete * from DB where studID=" + studID + "";
            cmd.ExecuteNonQuery();
            conn.Close();
            viewDB();
            kreditaiComboBox.SelectedIndex = 0;
            valLabel.Text = "135";
            ATStextBox.Text = "";
        }

        private void isvalyti_laukusButton_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            dataTimePicker.Text = "";
            vardasTextBox.Text = "";
            pavardeTextBox.Text = "";
            stud_gyv_v_adresasTextBox.Text = "";
            studijuProgramosComboBox.SelectedIndex = -1;
            praktikosTipaiComboBox.SelectedIndex = -1;
            specializacijaComboBox.SelectedIndex = -1;
            kursasComboBox.SelectedIndex = 3;
            praktikosPradziaTimePicker.Text = "";
            praktikosPabaigaTimePicker.Text = "";
            praktikosImoneTextBox.Text = "";
            //praktikosImonesAdresasTextBox.Text = "";
            imonesAtstovoVardasPavardeTextBox.Text = "";
            imonesAtstovoKontaktaiTextBox.Text = "";
            imonesATSTextBox.Text = "";
            gimimoMetaiTextBox.Text = "";
            kreditaiComboBox.SelectedIndex = 0;
            sutartiesNrTextBox.Text = "";
            studentoTelTextBox.Text = "";
            studentoPastasTextBox.Text = "";
            kolegijosPraktikosVadovasTextBox.Text = "";
            praktikosRezultataiTextBox.Text = "";
            direktoriusTextBox.Text = "";
            ATStextBox.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            progressBar1.Value = 0;
            int i = 0;

            var tempInt = dataGridView1.SelectedRows[0].Cells[i++].Value;
            if (tempInt != null && Int32.TryParse(tempInt.ToString(), out int parsedInt))
            {
                studID = parsedInt;
            }
            else
            {
                studID = 0;
            }

            //studID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[i++].Value.ToString());

            //dataTimePicker.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            var tempDateTime = dataGridView1.SelectedRows[0].Cells[i++].Value;
            if (tempDateTime != null && DateTime.TryParse(tempDateTime.ToString(), out DateTime parsedDate))
            {
                dataTimePicker.Value = parsedDate;
            }
            else
            {
                dataTimePicker.Value = DateTime.Today;
            }

            vardasTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            pavardeTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            stud_gyv_v_adresasTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();

            studijuProgramosComboBox.SelectedItem = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            praktikosTipaiComboBox.SelectedItem = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            specializacijaComboBox.SelectedItem = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();

            switch (dataGridView1.SelectedRows[0].Cells[i++].Value.ToString()){
                case "I":
                    kursasComboBox.SelectedIndex = 0;
                    break;
                case "II":
                    kursasComboBox.SelectedIndex = 1;
                    break;
                case "III":
                    kursasComboBox.SelectedIndex = 2;
                    break;
                case "IV":
                    kursasComboBox.SelectedIndex = 3;
                    break;
                default:
                    kursasComboBox.SelectedIndex = 3;
                    break;
            }

            //praktikosPradziaTimePicker.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            tempDateTime = dataGridView1.SelectedRows[0].Cells[i++].Value;
            if (tempDateTime != null && DateTime.TryParse(tempDateTime.ToString(), out DateTime parsedDate2))
            {
                praktikosPradziaTimePicker.Value = parsedDate2;
            }
            else
            {
                praktikosPradziaTimePicker.Value = DateTime.Today;
            }

            //praktikosPabaigaTimePicker.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            tempDateTime = dataGridView1.SelectedRows[0].Cells[i++].Value;
            if (tempDateTime != null && DateTime.TryParse(tempDateTime.ToString(), out DateTime parsedDate3))
            {
                praktikosPabaigaTimePicker.Value = parsedDate3;
            }
            else
            {
                praktikosPabaigaTimePicker.Value = DateTime.Today;
            }

            praktikosImoneTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            imonesAtstovoVardasPavardeTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            imonesAtstovoKontaktaiTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            imonesATSTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            gimimoMetaiTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            var tempKreditai = dataGridView1.SelectedRows[0].Cells[i++].Value;
            switch (Convert.ToInt32(tempKreditai.ToString()))
            {
                case 5:
                    kreditaiComboBox.SelectedIndex = 0;
                    valLabel.Text = "135";
                    break;
                case 6:
                    kreditaiComboBox.SelectedIndex = 1;
                    valLabel.Text = "160";
                    break;
                case 9:
                    kreditaiComboBox.SelectedIndex = 2;
                    valLabel.Text = "240";
                    break;
                case 15:
                    kreditaiComboBox.SelectedIndex = 3;
                    valLabel.Text = "400";
                    break;
                default:
                    kreditaiComboBox.SelectedIndex = 0;
                    valLabel.Text = "135";
                    break;
            }
            sutartiesNrTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            studentoTelTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            studentoPastasTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            kolegijosPraktikosVadovasTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            direktoriusTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            ATStextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            praktikosTikslaiTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
            praktikosRezultataiTextBox.Text = dataGridView1.SelectedRows[0].Cells[i++].Value.ToString();
        }

        private void kreditaiComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            switch (kreditaiComboBox.SelectedIndex)
            {
                case 0:
                    valLabel.Text = "135";
                    break;
                case 1:
                    valLabel.Text = "160";
                    break;
                case 2:
                    valLabel.Text = "240";
                    break;
                case 3:
                    valLabel.Text = "400";
                    break;
                default:
                    valLabel.Text = "135";
                    break;
            }
        }

        private void studijuProgramosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            specializacijaComboBox.Enabled = false;
            praktikosTipaiComboBox.Enabled = true;

            string studijuPrograma;
            if (studijuProgramosComboBox.SelectedItem != null)
            {
                studijuPrograma = studijuProgramosComboBox.SelectedItem.ToString();
            }
            else
            {
                studijuPrograma = "";
            }


                praktikosTikslaiTextBox.Text = "";
            praktikosRezultataiTextBox.Text = "";
            kreditaiComboBox.SelectedIndex = 0;

            praktikosTipaiComboBox.BeginUpdate();
            praktikosTipaiComboBox.Items.Clear();
            if (studijuPrograma == "Įstaigų ir įmonių administravimas")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Pažintinė",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Išmanioji vadyba")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Pažintinė",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Pardavimų ir logistikos vadyba")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Pažintinė",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Statybos verslo vadyba")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Pažintinė",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Buhalterinė apskaita")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Baigiamoji profesinės veiklos",
                    "Profesinės veiklos"});
            }
            else if (studijuPrograma == "Teisė")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Mokomoji praktika įmonėje",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Teisė ir teisėsaugos institucijos")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Mokomoji praktika įmonėje",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Verslo įmonių ekonomika")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Pažintinė",
                    "Baigiamoji profesinės veiklos",
                    "Profesinės veiklos"});
            }
            else if (studijuPrograma == "Skaitmeninė ekonomika")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Pažintinė",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Turizmo ir pramogų verslo industrija")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Baigiamoji profesinės veiklos",
                    "Turizmo pažintinė",
                    "Apgyvendinimo ir maitinimo pažintinė",
                    "Pramoginės veiklos"});
            }
            else if (studijuPrograma == "Skaitmeninio dizaino technologijos")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Mokomoji praktika įmonėje",
                    "Multimedijos produkto kūrimo",
                    "Baigiamoji profesinės veiklos"});
            }
            else if (studijuPrograma == "Taikomoji informatika ir programavimas")
            {
                praktikosTipaiComboBox.Items.AddRange(new object[] {
                    "Mokomoji praktika įmonėje",
                    "Baigiamoji profesinės veiklos"});
            }
            praktikosTipaiComboBox.SelectedIndexChanged -= praktikosTipaiComboBox_SelectedIndexChanged;
            praktikosTipaiComboBox.SelectedIndex = -1;
            praktikosTipaiComboBox.SelectedIndexChanged += praktikosTipaiComboBox_SelectedIndexChanged;
            // Kad specializacijaComboBox'e nepasiliktų senas užrašas
            specializacijaComboBox.SelectedIndexChanged -= specializacijaComboBox_SelectedIndexChanged;
            specializacijaComboBox.SelectedIndex = -1;
            specializacijaComboBox.SelectedIndexChanged += specializacijaComboBox_SelectedIndexChanged;
            praktikosTipaiComboBox.EndUpdate();
        }

        private void praktikosTipaiComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            docPath = @"\Sablonai\Praktika.docx";
            specializacijaComboBox.Enabled = false;
            praktikosRezultataiTextBox.ReadOnly = false;

            string studijuPrograma = studijuProgramosComboBox.SelectedItem.ToString();
            string praktikosTipas = praktikosTipaiComboBox.SelectedItem.ToString();

            praktikosTikslaiTextBox.Text = "";
            praktikosRezultataiTextBox.Text = "";
            kreditaiComboBox.SelectedIndex = 0;

            specializacijaComboBox.BeginUpdate();
            specializacijaComboBox.Items.Clear();
            if (studijuPrograma == "Įstaigų ir įmonių administravimas")
            {
                if (praktikosTipas == "Pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Įstaigų-ir-įmonių-administravimas_Pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Įstaigų-ir-įmonių-administravimas_Pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 2; // 9 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "Verslo įmonių administravimas",
                        "Valstybinių ir viešųjų įstaigų administravimas"});
                }
            }
            else if (studijuPrograma == "Išmanioji vadyba")
            {
                if (praktikosTipas == "Pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Išmanioji-vadyba_Pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Išmanioji-vadyba_Pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 1; // 6 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "Skaitmeninis marketingas",
                        "Tvari projektų vadyba"});
                }
            }
            else if (studijuPrograma == "Pardavimų ir logistikos vadyba")
            {
                if (praktikosTipas == "Pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 1; // 6 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "Prekių paskirstymo logistika",
                        "Pardavimų vadyba",
                        "Jūrų transporto logistika"});
                }
            }
            else if (studijuPrograma == "Statybos verslo vadyba")
            {
                if (praktikosTipas == "Pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Statybos-verslo-vadyba_Pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Statybos-verslo-vadyba_Pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 1; // 6 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "Statybinių medžiagų vadyba",
                        "Nekilnojamojo turto vadyba"});
                }
            }
            else if (studijuPrograma == "Buhalterinė apskaita")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "Vidaus auditas",
                        "Verslo vertinimas"});
                }
                else if (praktikosTipas == "Profesinės veiklos")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Buhalterinė-apskaita_Pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Buhalterinė-apskaita_Pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 2; // 9 kreditai
                }
            }
            else if (studijuPrograma == "Teisė")
            {
                if (praktikosTipas == "Mokomoji praktika įmonėje")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė_Mokomoji-praktika-įmonėje_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė_Mokomoji-praktika-įmonėje_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 2; // 9 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė_Baigiamoji-profesinės-veiklos_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė_Baigiamoji-profesinės-veiklos_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                }
            }
            else if (studijuPrograma == "Teisė ir teisėsaugos institucijos")
            {
                if (praktikosTipas == "Mokomoji praktika įmonėje")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė-ir-teisėsaugos-institucijos_Mokomoji-praktika-įmonėje_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė-ir-teisėsaugos-institucijos_Mokomoji-praktika-įmonėje_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 2; // 9 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė-ir-teisėsaugos-institucijos_Baigiamoji-profesinės-veiklos_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Teisė-ir-teisėsaugos-institucijos_Baigiamoji-profesinės-veiklos_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                }
            }
            else if (studijuPrograma == "Verslo įmonių ekonomika")
            {
                if (praktikosTipas == "Pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Verslo-įmonių-ekonomika_Pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Verslo-įmonių-ekonomika_Pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    docPath = @"\Sablonai\Praktika_verslo_imoniu_pazintine.docx";
                    kreditaiComboBox.SelectedIndex = 1; // 6 kreditai
                    praktikosRezultataiTextBox.ReadOnly = true; // Dėl praktikos uždavinių ir skirtingų tipų sąrašų
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "Verslo įmonių finansai"});
                }
                else if (praktikosTipas == "Profesinės veiklos")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Verslo-įmonių-ekonomika_Profesinės-veiklos_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Verslo-įmonių-ekonomika_Profesinės-veiklos_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 2; // 9 kreditai
                }
            }
            else if (studijuPrograma == "Skaitmeninė ekonomika")
            {

                if (praktikosTipas == "Pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninė-ekonomika_Pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninė-ekonomika_Pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 1; // 6 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    // TODO: pridėti
                    praktikosTikslaiTextBox.Text = "Dar nėra...";
                    praktikosRezultataiTextBox.Text = "Dar nėra...";
                    kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                }
            }
            else if (studijuPrograma == "Turizmo ir pramogų verslo industrija")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "Kelionių ir ekskursijų vadyba",
                        "SPA ir sveikatingumo turizmo vadyba",
                        "Renginių vadyba"});
                }
                else if (praktikosTipas == "Turizmo pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Turizmo-pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Turizmo-pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    docPath = @"\Sablonai\Praktika_turizmo_pazintine.docx";
                    praktikosRezultataiTextBox.ReadOnly = true; // Dėl sublist'ų
                    kreditaiComboBox.SelectedIndex = 0; // 5 kreditai
                    valLabel.Text = "134";
                }
                else if (praktikosTipas == "Apgyvendinimo ir maitinimo pažintinė")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Apgyvendinimo-ir-maitinimo-pažintinė_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Apgyvendinimo-ir-maitinimo-pažintinė_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 0; // 5 kreditai
                    valLabel.Text = "135";
                }
                else if (praktikosTipas == "Pramoginės veiklos")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Pramoginės-veiklos_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Pramoginės-veiklos_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    docPath = @"\Sablonai\Praktika_turizmo_pramogine.docx";
                    praktikosRezultataiTextBox.ReadOnly = true; // Dėl skirtingų sąrašų tipų
                    kreditaiComboBox.SelectedIndex = 0; // 5 kreditai
                    valLabel.Text = "135";
                }
            }
            else if (studijuPrograma == "Skaitmeninio dizaino technologijos")
            {
                if (praktikosTipas == "Mokomoji praktika įmonėje")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninio-dizaino-technologijos_Mokomoji-praktika-įmonėje_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninio-dizaino-technologijos_Mokomoji-praktika-įmonėje_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 2; // 9 kreditai
                }
                else if (praktikosTipas == "Multimedijos produkto kūrimo")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninio-dizaino-technologijos_Multimedijos-produkto-kūrimo_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninio-dizaino-technologijos_Multimedijos-produkto-kūrimo_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 1; // 6 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninio-dizaino-technologijos_Baigiamoji-profesinės-veiklos_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Skaitmeninio-dizaino-technologijos_Baigiamoji-profesinės-veiklos_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                }
            }
            else if (studijuPrograma == "Taikomoji informatika ir programavimas")
            {
                if (praktikosTipas == "Mokomoji praktika įmonėje")
                {
                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Taikomoji-informatika-ir-programavimas_Mokomoji-praktika-įmonėje_Tikslas.txt");
                        praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosTikslaiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }

                    try
                    {
                        StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Taikomoji-informatika-ir-programavimas_Mokomoji-praktika-įmonėje_Rezultatai.txt");
                        praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                    }
                    catch (Exception esr)
                    {
                        praktikosRezultataiTextBox.Text = "";
                        Console.WriteLine("Exception: " + esr.Message);
                    }
                    kreditaiComboBox.SelectedIndex = 2; // 9 kreditai
                }
                else if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    specializacijaComboBox.Enabled = true;
                    specializacijaComboBox.Items.AddRange(new object[] {
                        "WEB projektų kūrimas",
                        "Kompiuterinių tinklų administravimas"});
                }
            }
            specializacijaComboBox.SelectedIndexChanged -= specializacijaComboBox_SelectedIndexChanged;
            specializacijaComboBox.SelectedIndex = -1;
            specializacijaComboBox.SelectedIndexChanged += specializacijaComboBox_SelectedIndexChanged;
            specializacijaComboBox.EndUpdate();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void specializacijaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            string studijuPrograma = studijuProgramosComboBox.SelectedItem.ToString();
            string praktikosTipas = praktikosTipaiComboBox.SelectedItem.ToString();
            string specializacijosTipas = specializacijaComboBox.SelectedItem.ToString();

            if (studijuPrograma == "Įstaigų ir įmonių administravimas")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "Verslo įmonių administravimas")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Įstaigų-ir-įmonių-administravimas_Baigiamoji-profesinės-veiklos_Verslo-įmonių-administravimas_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Įstaigų-ir-įmonių-administravimas_Baigiamoji-profesinės-veiklos_Verslo-įmonių-administravimas_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Valstybinių ir viešųjų įstaigų administravimas")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Įstaigų-ir-įmonių-administravimas_Baigiamoji-profesinės-veiklos_Valstybinių-ir-viešųjų-įstaigų-administravimas_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Įstaigų-ir-įmonių-administravimas_Baigiamoji-profesinės-veiklos_Valstybinių-ir-viešųjų-įstaigų-administravimas_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                }
            }
            else if (studijuPrograma == "Išmanioji vadyba")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "Skaitmeninis marketingas")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Išmanioji-vadyba_Baigiamoji-profesinės-veiklos_Skaitmeninis-marketingas_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Išmanioji-vadyba_Baigiamoji-profesinės-veiklos_Skaitmeninis-marketingas_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Tvari projektų vadyba")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Išmanioji-vadyba_Baigiamoji-profesinės-veiklos_Tvari-projektų-vadyba_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Išmanioji-vadyba_Baigiamoji-profesinės-veiklos_Tvari-projektų-vadyba_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                }
            }
            else if (studijuPrograma == "Pardavimų ir logistikos vadyba")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "Prekių paskirstymo logistika")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Baigiamoji-profesinės-veiklos_Prekių-paskirstymo-logistika_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Baigiamoji-profesinės-veiklos_Prekių-paskirstymo-logistika_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Pardavimų vadyba")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Baigiamoji-profesinės-veiklos_Pardavimų-vadyba_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Baigiamoji-profesinės-veiklos_Pardavimų-vadyba_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Jūrų transporto logistika")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Baigiamoji-profesinės-veiklos_Jūrų-transporto-logistika_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Pardavimų-ir-logistikos-vadyba_Baigiamoji-profesinės-veiklos_Jūrų-transporto-logistika_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                }
            }
            else if (studijuPrograma == "Statybos verslo vadyba")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "Statybinių medžiagų vadyba")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Statybos-verslo-vadyba_Baigiamoji-profesinės-veiklos_Statybinių-medžiagų-vadyba_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Statybos-verslo-vadyba_Baigiamoji-profesinės-veiklos_Statybinių-medžiagų-vadyba_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Nekilnojamojo turto vadyba")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Statybos-verslo-vadyba_Baigiamoji-profesinės-veiklos_Nekilnojamojo-turto-vadyba_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Statybos-verslo-vadyba_Baigiamoji-profesinės-veiklos_Nekilnojamojo-turto-vadyba_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                }
            }
            else if (studijuPrograma == "Buhalterinė apskaita")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "Vidaus auditas")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Buhalterinė-apskaita_Baigiamoji-profesinės-veiklos_Vidaus-auditas_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Buhalterinė-apskaita_Baigiamoji-profesinės-veiklos_Vidaus-auditas_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Verslo vertinimas")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Buhalterinė-apskaita_Baigiamoji-profesinės-veiklos_Verslo-vertinimas_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Buhalterinė-apskaita_Baigiamoji-profesinės-veiklos_Verslo-vertinimas_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                }
            }
            else if (studijuPrograma == "Verslo įmonių ekonomika")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "Verslo įmonių finansai")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Verslo-įmonių-ekonomika_Baigiamoji-profesinės-veiklos_Verslo-įmonių-finansai_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Verslo-įmonių-ekonomika_Baigiamoji-profesinės-veiklos_Verslo-įmonių-finansai_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        docPath = @"\Sablonai\Praktika_verslo_imoniu_baigiamoji.docx";
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                        praktikosRezultataiTextBox.ReadOnly = true; // Dėl praktikos uždavinių
                    }
                }
            }
            else if (studijuPrograma == "Turizmo ir pramogų verslo industrija")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "Kelionių ir ekskursijų vadyba")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Baigiamoji-profesinės-veiklos_Kelionių-ir-ekskursijų-vadyba_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Baigiamoji-profesinės-veiklos_Kelionių-ir-ekskursijų-vadyba_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "SPA ir sveikatingumo turizmo vadyba")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Baigiamoji-profesinės-veiklos_SPA-ir-sveikatingumo-turizmo-vadyba_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Baigiamoji-profesinės-veiklos_SPA-ir-sveikatingumo-turizmo-vadyba_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Renginių vadyba")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Baigiamoji-profesinės-veiklos_Renginių-vadyba_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Turizmo-ir-pramogų-verslo-industrija_Baigiamoji-profesinės-veiklos_Renginių-vadyba_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                }
            }
            else if (studijuPrograma == "Taikomoji informatika ir programavimas")
            {
                if (praktikosTipas == "Baigiamoji profesinės veiklos")
                {
                    if (specializacijosTipas == "WEB projektų kūrimas")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Taikomoji-informatika-ir-programavimas_Baigiamoji-profesinės-veiklos_WEB-projektų-kūrimas_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Taikomoji-informatika-ir-programavimas_Baigiamoji-profesinės-veiklos_WEB-projektų-kūrimas_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                    else if (specializacijosTipas == "Kompiuterinių tinklų administravimas")
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Taikomoji-informatika-ir-programavimas_Baigiamoji-profesinės-veiklos_Kompiuterinių-tinklų-administravimas_Tikslas.txt");
                            praktikosTikslaiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosTikslaiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }

                        try
                        {
                            StreamReader sr = new StreamReader(Application.StartupPath + @"\Sablonai\txt\Taikomoji-informatika-ir-programavimas_Baigiamoji-profesinės-veiklos_Kompiuterinių-tinklų-administravimas_Rezultatai.txt");
                            praktikosRezultataiTextBox.Text = sr.ReadToEnd().TrimEnd('\r', '\n');
                        }
                        catch (Exception esr)
                        {
                            praktikosRezultataiTextBox.Text = "";
                            Console.WriteLine("Exception: " + esr.Message);
                        }
                        kreditaiComboBox.SelectedIndex = 3; // 15 kreditų
                    }
                }
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void dataTimePicker_ValueChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void vardasTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void pavardeTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void stud_gyv_v_adresasTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void kursasComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void praktikosPradziaTimePicker_ValueChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void praktikosPabaigaTimePicker_ValueChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void praktikosImoneTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void imonesAtstovoVardasPavardeTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void imonesAtstovoKontaktaiTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void imonesATSTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void gimimoMetaiTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void sutartiesNrTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void studentoTelTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void studentoPastasTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void kolegijosPraktikosVadovasTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void direktoriusTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void ATStextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void praktikosTikslaiTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }

        private void praktikosTikslasTextBox_TextChanged(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
        }
    }
}
