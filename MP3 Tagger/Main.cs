using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MP3_Tagger
{

    public partial class MP3Tagger : Form
    {
        public void GetTags() {
            //Read Tags
            string Selected = Convert.ToString(lstFiles.SelectedItem);
            TagLib.File file = TagLib.File.Create(Selected);
            string title = file.Tag.Title;
            string album = file.Tag.Album;
            string artist = file.Tag.FirstPerformer;
            string duration = file.Properties.Duration.ToString();
            uint number = file.Tag.Track;
            var art = file.Tag.Pictures.FirstOrDefault();

            //Display Tags 
            txtTitle.Text = title;
            txtArtist.Text = artist;
            txtAlbum.Text = album;
            txtLength.Text = duration.Substring(0, 9);
            txtNumber.Text = Convert.ToString(number);

            //Album Art
            if (art != null)
            {
                var ms = new MemoryStream();
                byte[] PictureData = art.Data.Data;
                ms.Write(PictureData, 0, Convert.ToInt32(PictureData.Length));
                var bm = new Bitmap(ms, false);
                ms.Dispose();
                pctArt.Image = bm;
            }
            else
            {
                pctArt.Image = null;
            }
        }








        public MP3Tagger()
        {
            InitializeComponent();





        }



        private void button1_Click(object sender, EventArgs e)
        {
           
                string FolderPath = "";
                FolderBrowserDialog SelectFile = new FolderBrowserDialog(); //Create Class
                if (SelectFile.ShowDialog() == DialogResult.OK) //OK Dialog
                {
                    FolderPath = SelectFile.SelectedPath;
                    txtPath.Text = FolderPath; //Shows Path In TextBox
                    string[] MP3Files = Directory.GetFiles(@FolderPath, "*.MP3");
                    lstFiles.Items.Clear();
                    foreach (string file in MP3Files)
                    {
                        lstFiles.Items.Add(file);
                    }
                }

            try
            {
                File.WriteAllText("FolderPath.txt", "");
                File.WriteAllText("FolderPath.txt", FolderPath);
            }
            catch(Exception ex)
            {
                Console.WriteLine("File may be in use...");
            }
        

        
    

           
         
            
               
            

            if (lstFiles.Items.Count == 0) {
                lstFiles.Items.Add("No MP3 Files Found");
                txtTitle.Text = "";
                txtAlbum.Text = "";
                txtArtist.Text = "";
                txtLength.Text = "";
                txtNumber.Text = "";
                pctArt.Image = null;
                lstFiles.Enabled = false;
            }
            else
            {
                lstFiles.Enabled = true;
            }

            txtTitle.Enabled = true;
            txtAlbum.Enabled = true;
            txtArtist.Enabled = true;
            txtNumber.Enabled = true;
            txtLength.Enabled = true;
            pctArt.Enabled = true;
            btnSave.Enabled = true;


        }

        private void Form1_Load(object sender, EventArgs e)
        {

            txtTitle.Enabled = false;
            txtAlbum.Enabled = false;
            txtArtist.Enabled = false;
            txtNumber.Enabled = false;
            txtLength.Enabled = false;
            pctArt.Enabled = false;
            btnSave.Enabled = false;

            if (File.Exists("FolderPath.txt"))
            {

                txtPath.Text = File.ReadAllText("FolderPath.txt");

                if (txtPath.Text != "")
                {
                    string[] MP3Files = Directory.GetFiles(txtPath.Text, "*.MP3");
                    foreach (string file in MP3Files)
                    {
                        lstFiles.Items.Add(file);
                    }

                    txtTitle.Enabled = true;
                    txtAlbum.Enabled = true;
                    txtArtist.Enabled = true;
                    txtNumber.Enabled = true;
                    txtLength.Enabled = true;
                    pctArt.Enabled = true;
                    btnSave.Enabled = true;
                }
                else
                {
                    File.Create("FolderPath.txt");
                }






            }
        }

        private void ShowFiles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTags();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex >= 0)
            {
                string Selected = Convert.ToString(lstFiles.SelectedItem);
                TagLib.File file = TagLib.File.Create(Selected);
                try
                {
                    file.Tag.Title = txtTitle.Text; //Update Tags With New Data.
                    file.Tag.Album = txtAlbum.Text;
                    file.Tag.Performers = new String[1] { txtArtist.Text };
                    file.Tag.Track = Convert.ToUInt16(txtNumber.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Data may be in an invalid format" + ex);
                }

                try
                {
                    TagLib.Picture Art = new TagLib.Picture();
                    MemoryStream ms = new MemoryStream();
                    pctArt.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Position = 0;
                    Art.Data = TagLib.ByteVector.FromStream(ms);
                    file.Tag.Pictures = new TagLib.IPicture[1] { Art };
                    file.Save();
                    ms.Close();
                }
                catch
                {
                }
                file.Save();





            }
            else
            {
                MessageBox.Show("Please Select An MP3 File");
            }
        }

        public void pctArt_Click(object sender, EventArgs e)
        {
            try { string Selected = Convert.ToString(lstFiles.SelectedItem);
                TagLib.File file = TagLib.File.Create(Selected);
                OpenFileDialog SelectCover = new OpenFileDialog();
                SelectCover.Filter = "Image Files(*.JPG; *.JPEG; *.GIF; *.BMP *.PNG)|*.JPG; *.JPEG; *.GIF; *.BMP,*.PNG";
                string FolderPath = SelectCover.FileName;
                if (SelectCover.ShowDialog() == DialogResult.OK) //OK Dialog 
                {
                    pctArt.Image = new Bitmap(SelectCover.FileName);
                }
            }
            catch { }

        }
    }
    
    }




