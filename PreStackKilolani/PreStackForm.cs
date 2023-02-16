using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TheSky64Lib;

namespace PreStack
{
    public partial class PreStackForm : Form
    {
        /*PreStack assembles light and calibration images for PixInsight processing
          normally after image capture via CCDAP or Humason, but can be used with others - just harder
        
  
        The file structure is created in TargetFolder.  It's structure is
        
                "PreStack" -> 
                    TargetName -> 
                        "Data Files" ->  
                            Image FITS -> 
                        "Calibration Files" ->  
                            Calibration FITS (Flats, Master Bias, Master Dark
        
        The Image FITS are relocated from CCDAP -> Images -> TargetName -> Data Files
                                      or  NightHawk->Images->TargetName->Data files
        
        The Calibration Files are relocated from CCDAP -> Images -> Targetname -> Calibration Files
                                      or NightHawk.
        
        Bias master files are copied from PreStack -> Bias and Prestack -> Darks.  The application determines the
          most recent file with the proper binning and exposure.  (as determined b

        Registration Master directory opened as courtesy
        
        */

        //Naming constants

        String CCDAPIMAGEDIRECTORYPATH = @"\Documents\CCDWare\CCDAutoPilot5";
        String CCDAPIMAGESUBDIRECTORYNAME = @"\Images";
        String CCDAPCALIBRATIONSUBDIRECTORYNAME = @"\Calibration Files";
        String CCDAPDATASUBDIRECTORYNAME = @"\Data Files";

        String NSUSERIMAGEDIRECTORYPATH = @"\Documents\Humason";
        String NSIMAGESUBDIRECTORYNAME = @"\Images";
        String NSCALIBRATIONSUBDIRECTORYNAME = @"\Calibration Files";
        String NSDATASUBDIRECTORYNAME = @"\Data Files";

        String PSUSERDIRECTORYPATH = @"\Documents\PreStack";
        String PSBIASSUBDIRECTORYNAME = @"\Bias";
        String PSDARKSSUBDIRECTORYNAME = @"\Darks";
        String PSDATASUBDIRECTORYNAME = @"\Data Files";
        String PSREGMASTERDIRECTORYNAME = @"\Registration Master";
        String PSCALIBRATIONSUBDIRECTORYNAME = @"\Calibration Files";
        String PSEASTSUBDIRECTORYNAME = @"\East";
        String PSWESTSUBDIRECTORYNAME = @"\West";

        //Naming variables
        private string PreStackRootDirectoryFullPath;     //Path ; PreStack Image Directory in User\Documents:   image file destination
        private string ImageRootDirectoryFullPath;    //Path ; CCDAP Image Directoy in User\Documents: image file source
        private string DataSubDirectoryName;
        private string CalibrationSubDirectoryName;
        //private string BiasSubDirectoryName;
        //private string DarkSubDirectoryName;

        String DestinationFolderDescription = "Select parent directory where file subdirectories will be assembled.";

        String SourceFolderDescription = "Select parent folder where image files are located.";

        String FlatsFolderDescription = "Select directory containing Calibration Files subdirectory for (this stack.";

        String DefaultTargetName = "NGC";

        public PreStackForm()
        {
            InitializeComponent();
            //Write the version and build in the title
            //Subscribe this form to the log event 
            try
            { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch
            {
                //probably in debug mode
                this.Text = " in Debug";
            }
            this.Text = "PreStack V" + this.Text;
            //Generate paths for (current user for (PreStack and CCDAP root directories
            if (CCDAPRadioButton.Checked)
            {
                ImageRootDirectoryFullPath =
                    @"C:\Users\" + System.Environment.UserName + CCDAPIMAGEDIRECTORYPATH + CCDAPIMAGESUBDIRECTORYNAME;
                DataSubDirectoryName = CCDAPDATASUBDIRECTORYNAME;
                CalibrationSubDirectoryName = CCDAPCALIBRATIONSUBDIRECTORYNAME;
            }
            else
            {
                ImageRootDirectoryFullPath =
                    @"C:\Users\" + System.Environment.UserName + NSUSERIMAGEDIRECTORYPATH + NSIMAGESUBDIRECTORYNAME;
                DataSubDirectoryName = NSDATASUBDIRECTORYNAME;
                CalibrationSubDirectoryName = NSCALIBRATIONSUBDIRECTORYNAME;
            }
            PreStackRootDirectoryFullPath =
                @"C:\Users\" + System.Environment.UserName + PSUSERDIRECTORYPATH;
            //Set up the target name field:  initial text, its length and where ; put the curser ; start
            TargetNameText.Text = DefaultTargetName;
            TargetNameText.SelectionLength = 4;
            TargetNameText.SelectionStart = 3;
            //Set up the candidate source target name field
            string[] datednamelist = System.IO.Directory.GetDirectories(ImageRootDirectoryFullPath);
            string shortname;
            string targetname;
            string[] datednamesubs;
            //Build checklist for image data files
            foreach (string dname in datednamelist)
            {
                shortname = (dname.Split('\\')).Last();
                targetname = (shortname.Split('_')).Last();
                if (targetname.Length > 0)
                {
                    datednamesubs = System.IO.Directory.GetDirectories(dname);
                    foreach (string imagesub in datednamesubs)
                    {
                        if (imagesub.Contains("Data Files"))
                        {
                            ImageDirectoryListBox.Items.Add(shortname);
                        }
                    }
                }
            }

            //Build checklist for (flats calibration files
            foreach (string dname in datednamelist)
            {
                shortname = (dname.Split('\\')).Last();
                if (shortname.Length == 8)
                {
                    datednamesubs = System.IO.Directory.GetDirectories(dname);
                    foreach (string flatssub in datednamesubs)
                    {
                        if (flatssub.Contains("Calibration Files"))
                        {
                            FlatsDirectoryListBox.Items.Add(shortname);
                        }
                    }
                }
            }
            //Build checklist for (bias calibration files
            //If Bias directory does not exist, then create it
            string biasDir = PreStackRootDirectoryFullPath + PSBIASSUBDIRECTORYNAME;
            if (!System.IO.Directory.Exists(biasDir)) { System.IO.Directory.CreateDirectory(biasDir); }
            foreach (string dname in System.IO.Directory.GetFiles(biasDir, "*.xisf", System.IO.SearchOption.AllDirectories))
            {
                BiasDirectoryListBox.Items.Add(dname.Substring((PreStackRootDirectoryFullPath + PSBIASSUBDIRECTORYNAME).Length + 1));
            }

            //Build checklist for (darks calibration files
            //Darks directory does not exist, then create it
            string darksDir = PreStackRootDirectoryFullPath + PSDARKSSUBDIRECTORYNAME;
            if (!System.IO.Directory.Exists(darksDir)) { System.IO.Directory.CreateDirectory(darksDir); }

            foreach (string dname in System.IO.Directory.GetFiles(PreStackRootDirectoryFullPath + PSDARKSSUBDIRECTORYNAME,
                                                   "*.xisf",
                                                   System.IO.SearchOption.AllDirectories))
            {
                DarksDirectoryListBox.Items.Add(dname.Substring((PreStackRootDirectoryFullPath + PSDARKSSUBDIRECTORYNAME).Length + 1));
            }
            return;
        }

        private void ImageDirectoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Note that multiple targets can be checked -- in case of multiple nights on one target and want to use one set of flats, bias, darks
            //So, do nothing except change the target name accordingly
            int choice = ImageDirectoryListBox.SelectedIndex;
            bool t = ImageDirectoryListBox.GetItemChecked(choice);
            ImageDirectoryListBox.SetItemChecked(choice, true);
            string ImageSourceDirectoryName = ImageDirectoryListBox.SelectedItem.ToString();
            string ImageName = ImageSourceDirectoryName.Split('_')[1];  //Gets second element of image name
            TargetNameText.Text = ImageName;

            return;
        }

        private void FlatsDirectoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int choice = FlatsDirectoryListBox.SelectedIndex;
            if (choice < 0)
            {
                return;
            }
            else
            {
                int listitems = FlatsDirectoryListBox.Items.Count;
                for (int i = 0; i < listitems - 1; i++)
                {
                    FlatsDirectoryListBox.SetItemChecked(i, false);
                }
                FlatsDirectoryListBox.ClearSelected();
                FlatsDirectoryListBox.SetItemChecked(choice, true);
            }
            return;
        }

        private void BiasDirectoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Add bias file ; calibration directory
            int choice = BiasDirectoryListBox.SelectedIndex;
            if (choice < 0)
            {
                return;
            }
            else
            {
                int listitems = BiasDirectoryListBox.Items.Count;
                for (int i = 0; i < listitems - 1; i++)
                {
                    BiasDirectoryListBox.SetItemChecked(i, false);
                }
                BiasDirectoryListBox.ClearSelected();
                BiasDirectoryListBox.SetItemChecked(choice, true);
            }
            return;
        }

        private void DarksDirectoryListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Add Darks file ; calibration directory
            int choice = DarksDirectoryListBox.SelectedIndex;
            if (choice < 0)
            {
                return;
            }
            else
            {
                int listitems = DarksDirectoryListBox.Items.Count;
                for (int i = 0; i < listitems - 1; i++)
                {
                    DarksDirectoryListBox.SetItemChecked(i, false);
                }
                DarksDirectoryListBox.ClearSelected();
                DarksDirectoryListBox.SetItemChecked(choice, true);
            }
            return;
        }

        private void AssembleFilesButton_Click(object sender, EventArgs e)
        //Finds and copies images, flats, master bias and master darks ; PreStack directory
        //if a directory exists for (the target name, { generate a new directory with a
        //Find and create the first instance of an empty directory with the target name
        //
        //Change 12/25/17:  create date suffix for (destination directory name based on creation date of source directory.
        //  Continue ; add a number suffix, just in case, but probably will never happen.
        //
        {
            Color oldColor = AssembleFilesButton.BackColor;
            AssembleFilesButton.BackColor = Color.LightPink;
            Show();
            System.Windows.Forms.Application.DoEvents();
            int dirNum = 1;
            string ImageDestinationDirectoryFullPath = PreStackRootDirectoryFullPath + @"\" + TargetNameText.Text;
            while (System.IO.Directory.Exists(ImageDestinationDirectoryFullPath))
            {
                dirNum += 1;
                ImageDestinationDirectoryFullPath = PreStackRootDirectoryFullPath + @"\" + TargetNameText.Text + "_" + dirNum.ToString();
            }
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath);
            //Check for (the existance of the image source directory and flats source directory
            if (ImageDirectoryListBox.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No image source selected.");
                AssembleFilesButton.BackColor = oldColor;
                return;
            }
            if (FlatsDirectoryListBox.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No flats source selected.");
                //return;
            }
            //Data Files *****************
            //Move the data files directory from data folder ; PreStack data files folder
            // Move directory seems ; be creating two copies of the directory
            foreach (string tgtFileName in ImageDirectoryListBox.CheckedItems)
            {
                string sourcedata = ImageRootDirectoryFullPath + @"\" + tgtFileName + DataSubDirectoryName;
                string destinationdata = ImageDestinationDirectoryFullPath + PSDATASUBDIRECTORYNAME;
                System.IO.Directory.CreateDirectory(destinationdata);
                bool cStat = CopyDirectory(sourcedata, destinationdata);
                if (!cStat)
                {
                    System.Windows.Forms.MessageBox.Show("Data file copy problem: ");
                }
                //Break out east and west meridian shots into separate folders
                string datadir = ImageDestinationDirectoryFullPath + @"\Data Files";
                SeparatePierside(destinationdata);
            }

            //Copy the calibration files directory from calibration folder ; PreStack calibration files folder
            // ; if there are calibration files to copy
            if (FlatsDirectoryListBox.CheckedItems.Count > 0)
            {
                string sourcecal = ImageRootDirectoryFullPath + @"\" + FlatsDirectoryListBox.CheckedItems[0] + CalibrationSubDirectoryName;
                string destinationcal = ImageDestinationDirectoryFullPath + PSCALIBRATIONSUBDIRECTORYNAME;
                System.IO.Directory.CreateDirectory(destinationcal);
                //string destinationcal = ImageDestinationDirectoryFullPath ;
                bool cStat = CopyDirectory(sourcecal, destinationcal);
                if (!cStat)
                {
                    System.Windows.Forms.MessageBox.Show("Calibration file copy problem: ");
                }
                //Separate the calibration (flats) files by pierside
                string caldir = ImageDestinationDirectoryFullPath + @"\Calibration Files";
                SeparatePierside(destinationcal);
            }
            //
            //Copy selected master bias file ; selected directory ; prestack calibration directory
            string sourcebias = PreStackRootDirectoryFullPath + PSBIASSUBDIRECTORYNAME + @"\" + BiasDirectoryListBox.CheckedItems[0];
            string destinationbias = BiasDirectoryListBox.CheckedItems[0].ToString();
            destinationbias = ImageDestinationDirectoryFullPath + PSCALIBRATIONSUBDIRECTORYNAME +
                                @"\" + destinationbias.Split('\\').Last();
            System.IO.File.Copy(sourcebias, destinationbias);

            //Copy selected master dark file ; selected directory ; prestack calibration d@"\"irectory
            string sourcedark = PreStackRootDirectoryFullPath + PSDARKSSUBDIRECTORYNAME + @"\" + DarksDirectoryListBox.CheckedItems[0];
            string destinationdark = DarksDirectoryListBox.CheckedItems[0].ToString();
            destinationdark = ImageDestinationDirectoryFullPath + PSCALIBRATIONSUBDIRECTORYNAME +
                                @"\" + destinationdark.Split('\\').Last();
            System.IO.File.Copy(sourcedark, destinationdark);

            //Create the Master directories ; be used by PixInsight
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath + @"\Master East");
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath + @"\Master East" + @"\Selected");
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath + @"\Master West");
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath + @"\Master West" + @"\Selected");
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath + @"\Master All");
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath + @"\PI Project");
            System.IO.Directory.CreateDirectory(ImageDestinationDirectoryFullPath + @"\Registration Master");

            System.Windows.Forms.MessageBox.Show("Transfer Complete");
            AssembleFilesButton.BackColor = oldColor;
            return;

        }

        #region Simple Buttons

        private void FilterFlatsButton_Click(object sender, EventArgs e)
        {
            FlatCheck();
            return;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
            return;
        }

        #endregion

        private void SeparatePierside(string parentdir)
        //Separate out either data or calibration images into east and west sides
        //  placing each side//s files in ; subdirectories labelled "East" or "West"
        //  
        //The directory in which ; separate is "parentdir" which must be a full system file path
        //
        //Make two new subdirectories, if they don//t already exist
        //
        {
            string eastdir = parentdir + PSEASTSUBDIRECTORYNAME;
            string westdir = parentdir + PSWESTSUBDIRECTORYNAME;
            if (!System.IO.Directory.Exists(eastdir))

            {
                System.IO.Directory.CreateDirectory(eastdir);
            }
            if (!System.IO.Directory.Exists(westdir))
            {
                System.IO.Directory.CreateDirectory(westdir);
            }
            //Get list of files with "East" in filename, move ; eastdir
            //{ get list of files with "West" in filename, move ; westdir
            string[] allfiles = System.IO.Directory.GetFiles(parentdir);
            foreach (string fn in allfiles)
            {
                if (fn.Contains("E."))
                {
                    int dindx = fn.IndexOf(PSDATASUBDIRECTORYNAME) + PSDATASUBDIRECTORYNAME.Length;
                    string nfn = fn.Insert(dindx, @"\East");
                    System.IO.File.Move(fn, nfn);
                }
                if (fn.Contains("W."))
                {
                    int dindx = fn.IndexOf(PSDATASUBDIRECTORYNAME) + PSDATASUBDIRECTORYNAME.Length;
                    string nfn = fn.Insert(dindx, @"\West");
                    System.IO.File.Move(fn, nfn);
                }
                if (fn.Contains("PAEast."))
                {
                    int dindx = fn.IndexOf(PSCALIBRATIONSUBDIRECTORYNAME) + PSCALIBRATIONSUBDIRECTORYNAME.Length;
                    string nfn = fn.Insert(dindx, @"\East");
                    System.IO.File.Move(fn, nfn);
                }
                if (fn.Contains("PAWest."))
                {
                    int dindx = fn.IndexOf(PSCALIBRATIONSUBDIRECTORYNAME) + PSCALIBRATIONSUBDIRECTORYNAME.Length;
                    string nfn = fn.Insert(dindx, @"\West");
                    System.IO.File.Move(fn, nfn);
                }
            }
        }

        private void FlatCheck()
        //Open folder and check each flat file for (conformance ; ADU between 20000 and 40000
        {
            int minABU = 20000;
            int maxABU = 40000;
            string ffImageType = "Flat Field";

            DialogResult fitsfilelist = OpenFitsFileDialog.ShowDialog();
            string[] fitsfiles = OpenFitsFileDialog.FileNames;
            //string fitsfileprogressincrement = (100 / fitsfiles.Length)

            FileCopyBar.Value = 0;
            FileCopyBar.Maximum = fitsfiles.Length;
            //Create image object
            ccdsoftImage tsx_im = new ccdsoftImage();

            foreach (string file in fitsfiles)
            {
                FitsFile fpo = new FitsFile(file, false);
                string filtertype = fpo.ReadKey("FILTER");
                string imagetype = fpo.ReadKey("IMAGETYP");
                string target = TargetNameText.Text;
                if (imagetype.Contains(ffImageType))
                {
                    tsx_im.Path = file;
                    tsx_im.Open();
                    if (tsx_im.averagePixelValue() < minABU)
                    {
                        if (System.Windows.Forms.MessageBox.Show(file + '\n' + "ADU Too Low: " + (tsx_im.averagePixelValue()).ToString() + '\n' + "Delete File?",
                            "Delete File Dialog",
                            MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            tsx_im.Close();
                            System.IO.File.Delete(file);
                        }
                    }
                    else
                    {
                        if (tsx_im.averagePixelValue() > maxABU)
                        {
                            if (MessageBox.Show(file + '\n' + "ADU Too High: " + (tsx_im.averagePixelValue()).ToString() + '\n' + "Delete File?",
                                "Delete File Dialog",
                                MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                tsx_im.Close();
                                System.IO.File.Delete(file);
                            }
                        }
                    }
                    tsx_im.Close();
                }
                FileCopyBar.Value += 1;
            }
            FileCopyBar.Value = 0;
            tsx_im = null;
            return;
        }

        private bool CopyDirectory(string srcDir, string destDir)
        {
            //Method copies all the files from the srcDir directory to the destDir directory
            //if any filename is duplicated in the destDir, then an "A" is appended
            //  to the end of the copied file.
            //Both directories must exist or a false is returned
            //Returns true if successful
            if ((!System.IO.Directory.Exists(srcDir)) || (!System.IO.Directory.Exists(destDir)))
            { return false; }
            IEnumerable<string> srcList = System.IO.Directory.GetFiles(srcDir);
            foreach (string sFile in srcList)
            {
                string sShortName = sFile.Split('\\').Last();
                string dFile = destDir + @"\" + sShortName;
                while (System.IO.File.Exists(dFile))
                { //add an "A" until the file doesn't exist
                    dFile = dFile + "A";
                }

                try { System.IO.File.Copy(sFile, dFile); }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        #region buttons

        private void OpenFitsFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void DestinationFolderDialog_HelpRequest(object sender, EventArgs e)
        {

        }

        private void FlatsFolderDialog_HelpRequest(object sender, EventArgs e)
        {

        }

        private void OpenCalibrationFileDialog_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TurnOverButton_Click(object sender, EventArgs e)
        {
            //Rotates FITS image file by 180 degrees
            DialogResult fitsfilelist = OpenFitsFileDialog.ShowDialog();
            string[] fitsfiles = OpenFitsFileDialog.FileNames;
            if (fitsfiles.Length > 0)
            {
                foreach (string ff in fitsfiles)
                {
                    FitsFile fitRot = new FitsFile(ff, false);
                    bool rotateStat = fitRot.RotateFit180(ff);
                }
            }
            return;
        }

        #endregion

    }
}