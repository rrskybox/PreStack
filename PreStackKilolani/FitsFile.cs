using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace PreStack
{
    public class FitsFile
    {
        System.Drawing.Imaging.PixelFormat PixFormat = System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;

        byte[] HeaderRecord = new byte[80];
        byte[] DataUnit = new byte[2880];
        int bCount;
        List<string> KeyStr = new List<string>();
        int ImageHeaderLength = 56 + (256 * 4);

        public int xAxis;
        public int yAxis;
        public int MaxValue;
        public int MinValue;
        public int AveValue;

        public Image FITSimage;
        public byte[] FITSbytes = new byte[1];

        public FitsFile(string filepath, bool dataswitch)
        {

            //Opens file set by filepath (assumes it//s a FITS formatted file)
            //Reads in header in 80 character strings, while ("END" is found
            //if (dataswitch is true, { an array "FITSimage" is created and populated with the FITS image data as an class Image
            //  otherwise just the header info is retained
            //
            int keyindex = -1;
            FileStream FitsHandle = File.OpenRead(filepath);
            do
            {
                keyindex++;
                bCount = FitsHandle.Read(HeaderRecord, 0, 80);
                //Check for empty file (file error on creation), just opt out if (so
                if (bCount == 0)
                {
                    return;
                }
                KeyStr.Add(System.Text.Encoding.ASCII.GetString(HeaderRecord));
            } while (!KeyStr.Last().StartsWith("END "));
            //Continue through any remaining header padding
            do
            {
                keyindex++;
                bCount = FitsHandle.Read(HeaderRecord, 0, 80);
            } while (!(keyindex % 36 == 0));
            //Get the array dimensions
            int bitpix = Convert.ToInt32(ReadKey("BITPIX"));
            xAxis = Convert.ToInt32(ReadKey("NAXIS1"));
            yAxis = Convert.ToInt32(ReadKey("NAXIS2"));
            int totalpixels = xAxis * yAxis;
            int headerbytes = ImageHeaderLength;
            int totaldata = totalpixels + ImageHeaderLength;

            //Fits data section -- work in progress
            if (dataswitch)
            {
                //Create structure for FITS image byte array
                Array.Resize(ref FITSbytes, totaldata);

                byte[] dbytes;
                //Fill in the image header info
                FITSbytes[0] = 66; // B
                FITSbytes[1] = 77; // M
                dbytes = BitConverter.GetBytes(Convert.ToInt32(totaldata));
                //Total data bytes
                FITSbytes[2] = dbytes[0];
                FITSbytes[3] = dbytes[1];
                FITSbytes[4] = dbytes[2];
                FITSbytes[5] = dbytes[3];
                //Unused
                FITSbytes[6] = 0;
                FITSbytes[7] = 0;
                FITSbytes[8] = 0;
                FITSbytes[9] = 0;
                //Start of data record [1 based]
                dbytes = BitConverter.GetBytes(headerbytes);
                FITSbytes[10] = dbytes[0];
                FITSbytes[11] = dbytes[1];
                FITSbytes[12] = dbytes[2];
                FITSbytes[13] = dbytes[3];
                //DIB Header Size
                FITSbytes[14] = 40;
                FITSbytes[15] = 0;
                FITSbytes[16] = 0;
                FITSbytes[17] = 0;
                //Xaxis pixels
                dbytes = BitConverter.GetBytes(xAxis);
                FITSbytes[18] = dbytes[0];
                FITSbytes[19] = dbytes[1];
                FITSbytes[20] = dbytes[2];
                FITSbytes[21] = dbytes[3];
                //Yaxis pixels
                dbytes = BitConverter.GetBytes(yAxis);
                FITSbytes[22] = dbytes[0];
                FITSbytes[23] = dbytes[1];
                FITSbytes[24] = dbytes[2];
                FITSbytes[25] = dbytes[3];
                //Number of planes
                FITSbytes[26] = 1;
                FITSbytes[27] = 0;
                //Bits per pixel
                FITSbytes[28] = 8;
                FITSbytes[29] = 0;
                //Filler
                for (int i = 0; i <= 15; i++)
                {
                    FITSbytes[30 + i] = 0;
                }
                //Number of colors
                FITSbytes[46] = 0;
                FITSbytes[47] = 1;
                FITSbytes[48] = 0;
                FITSbytes[49] = 0;
                //Number of important colors
                FITSbytes[50] = 0;
                FITSbytes[51] = 1;
                FITSbytes[52] = 0;
                FITSbytes[53] = 0;
                //Filler
                for (int i = 54; i <= 55; i++)
                {
                    FITSbytes[i] = 0;
                }

                int indx = 57;
                for (int i = 0; i <= 255; i++)
                {
                    FITSbytes[indx] = 255;
                    FITSbytes[indx + 1] = (byte)i;
                    FITSbytes[indx + 2] = (byte)i;
                    FITSbytes[indx + 3] = (byte)i;
                    indx += 4;
                }

                int dataindex = 0;
                Int16 dataval1;
                Int32 dataval;
                Int16 highbyte;
                Int16 lowbyte;

                do
                {
                    bCount = FitsHandle.Read(DataUnit, 0, 2880);
                    for (int k = 0; k <= (bCount - 1); k = k + 2)
                    {
                        if (dataindex < totalpixels)
                        {
                            //Data is stored in high byte followed by low byte
                            highbyte = DataUnit[k];
                            lowbyte = DataUnit[k + 1];
                            dataval1 = (Int16)((highbyte << 8) + lowbyte);
                            dataval = dataval1;
                            dataval = dataval1 + (2 ^ 15);

                            FITSbytes[dataindex + headerbytes] = (byte)(dataval / 256);
                        }
                        dataindex += 1;
                    }
                } while (bCount == 0);  //No more bytes read in = done
            }

            //Close file
            FitsHandle.Close();
            FitsHandle = null;
            return;
        }

        public string ReadKey(string keyword)
        {
            //return;s contents of key word entry, scrubbed of extraneous characters
            foreach (string keyline in KeyStr)
            {
                if (keyline.Contains(keyword))
                {
                    int startindex = keyline.IndexOf("=");

                    int endindex = keyline.IndexOf("/");
                    if (endindex == 0)
                    {
                        endindex = keyline.Length - 1;
                    }

                    string keylineN = keyline.Substring(startindex + 1, endindex - (startindex + 1));
                    // keyline = Replace(keyline, "//", " ");
                    keylineN = keylineN.Replace('/', ' ');
                    // keyline = Trim(keyline);
                    keylineN = keylineN.Trim(' ');
                    return (keylineN);
                }
            }
            return (null);
        }

        public void GetByteValues()
        {
            //Determines the minimum, maximum and average values for the FITS data image
            double Low = 255;
            double High = 0;
            double Ave = 0;
            double Pix;

            for (int i = ImageHeaderLength; i < FITSbytes.Length; i++)
            {
                Pix = FITSbytes[i];
                Ave = Ave + Pix;
                if (Pix < Low)
                {
                    Low = Pix;
                }
                if (Pix > High)
                {
                    High = Pix;
                }
            }
            Ave = Ave / FITSbytes.Length;
            MaxValue = (int)High;
            MinValue = (int)Low;
            AveValue = (int)Ave;
            return;
        }

        public void SimpleStretchBytes()
        {
            //Stretches image values based on Max, Min of byte image
            //  This alogrithm subtracts the Min from each element, { multiplies by the
            //     total range divided by the max-min range -- making sure there is not
            //     a divide by zero situation by adding 1 to the average

            double ddata;
            double ndata;
            //Compute the average of the "middle" 90% of pixels

            GetByteValues();

            for (int i = ImageHeaderLength; i < FITSbytes.Length; i++)
            {
                ddata = Convert.ToDouble(FITSbytes[i]);
                //stretch range from min to max value
                ndata = (255 / MaxValue) * (ddata - MinValue);
                //Clip top and bottom of range to 0 and 255 respectively
                if (ndata > 255)
                {
                    ndata = 255;
                }
                if (ndata < 0)
                {
                    ndata = 0;
                }
                //Convert back into the image array
                FITSbytes[i] = Convert.ToByte(ndata);
            }
            return;
        }

        public void ConstrastStretchBytes()
        {
            //Stretches image values based on Max, Min of byte image
            //  This alogrithm subtracts the Min from each element, { multiplies by the
            //     total range divided by the max-min range -- making sure there is not
            //     a divide by zero situation by adding 1 to the average

            int ddata;
            double ndata;
            //Compute the average of the "middle" 90% of pixels

            int datasize = FITSbytes.Length - ImageHeaderLength;

            //Make a probablity mass functiom (PMF) for the histogram, start with it zeroed out
            //  and get the number of datapoints
            //
            int[] pmfdata = new int[256];
            for (int i = 0; i < pmfdata.Length; i++)
            {
                pmfdata[i] = 0;
                datasize += 1;
            }
            for (int j = ImageHeaderLength; j < FITSbytes.Length; j++)
            {
                ddata = Convert.ToInt16(FITSbytes[j]);
                pmfdata[ddata] += 1;
            }

            //Make a cumulative distributive functiom (CDF) for the , start with it zeroed out
            //  Normalize to the total number of points
            double[] cdfdata = new double[256];
            cdfdata[0] = pmfdata[0] / datasize;
            for (int k = 1; k < pmfdata.Length; k++)
            {
                cdfdata[k] = cdfdata[k - 1] + (pmfdata[k] / datasize);
            }
            //Adjust each value of the image based on it//s

            for (int i = ImageHeaderLength; i < FITSbytes.Length; i++)
            {
                ddata = Convert.ToInt16(FITSbytes[i]);
                //stretch range from min to max value
                // ndata = (255 / MaxValue) * (ddata - MinValue)
                ndata = ddata * (cdfdata[ddata]);
                //Clip top and bottom of range to 0 and 255 respectively
                if (ndata > 255)
                {
                    ndata = 255;
                }
                if (ndata < 0)
                {
                    ndata = 0;
                }
                //Convert back into the image array
                FITSbytes[i] = Convert.ToByte(ndata);
            }
            return;
        }

        public bool RotateFit180(string filepathIn)
        {
            //Invert both the rows and columns of FITS image such that the image rotates 180 degrees
            //
            //Read in the FITS header, then the data.
            //Write back out the FITS hearder, then the data, in reverse in bytes of two
            //Opens file set by filepath (assumes it//s a FITS formatted file)
            //Reads in header in 80 character strings, while ("END" is found
            //if (dataswitch is true, { an array "FITSimage" is created and populated with the FITS image data as an class Image
            //  otherwise just the header info is retained
            //
            int keyindex = -1;
            FileStream FitsHandleIn = File.OpenRead(filepathIn);
            do
            {
                keyindex++;
                bCount = FitsHandleIn.Read(HeaderRecord, 0, 80);
                //Check for empty file (file error on creation), just opt out if (so
                if (bCount == 0)
                {
                    FitsHandleIn.Close();
                    return false;
                }
                KeyStr.Add(System.Text.Encoding.ASCII.GetString(HeaderRecord));
            } while (!KeyStr.Last().StartsWith("END "));
            //Continue through any remaining header padding
            do
            {
                keyindex++;
                bCount = FitsHandleIn.Read(HeaderRecord, 0, 80);
            } while (!(keyindex % 36 == 0));
            //Get the array dimensions
            int bitpix = Convert.ToInt32(ReadKey("BITPIX"));
            int xAxis = Convert.ToInt32(ReadKey("NAXIS1"));
            int yAxis = Convert.ToInt32(ReadKey("NAXIS2"));
            int totalpixels = xAxis * yAxis;
            int headerbytes = ImageHeaderLength;
            int totaldata = totalpixels + ImageHeaderLength;
 
            //Read in the data array in 2880 byte chunks
            byte[] iData = new byte[totalpixels * 2];
            int dataIndex = 0;
            do
            {
                bCount = FitsHandleIn.Read(DataUnit, 0, 2880);
                for (int k = 0; k < bCount; k++)
                {
                    if (dataIndex < iData.Length)
                    {
                        iData[dataIndex] = DataUnit[k];
                        dataIndex++;
                    }
                }
            } while (dataIndex < iData.Length);

          //Close the input file and reopen
            FitsHandleIn.Close();
            FitsHandleIn = File.OpenRead(filepathIn);

             //Create a new file and stream writer
            string filepathOut = filepathIn.Remove(filepathIn.Length - 4);
            filepathOut = filepathOut + ".R.fit";
            FileStream FitsHandleOut = File.OpenWrite(filepathOut);
            //Copy the header record from one fits to the other
            for (int i = 0; i < keyindex; i++)
            {
                bCount = FitsHandleIn.Read(HeaderRecord, 0, 80);
                FitsHandleOut.Write(HeaderRecord, 0, 80);
            }
            //Write the data words in reverse order from the last data word to the first
            int idx = iData.Length-1;
            byte[] dataUnitOut = new byte[2880];
            do
            {
                for (int dc = 0; dc < 2880; dc+=2)
                {
                    if (idx >= 0)
                    {
                        dataUnitOut[dc] = iData[idx - 1];
                        dataUnitOut[dc + 1] = iData[idx];
                        idx = idx - 2;
                    }
                    else
                    {
                        dataUnitOut[dc] = 0;
                        dataUnitOut[dc + 1] = 0;
                        idx = idx - 2;
                    }
                }
                FitsHandleOut.Write(dataUnitOut, 0, 2880);
                } while (idx >= 0);
            FitsHandleOut.Close();
            FitsHandleIn.Close();
            return true;
        }
    }
}

