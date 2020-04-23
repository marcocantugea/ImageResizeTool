using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging ;
using System.IO;

namespace mcgImageTools.com.image
{
    public sealed class ImageTool
    {
        private FileInfo _SourceImageFileName;
        private String _NewImageFileName;
        private float _DefaultPercentReduction;
        
        public ImageTool() {
            _DefaultPercentReduction = .50f;
            _NewImageFileName = null;
        }

        public ImageTool(String SourceImageFileName, String NewImageFileName) {

            if (!System.IO.File.Exists(SourceImageFileName))
                throw new FileNotFoundException("ImageTool:ImageTool: Source Image File not found.");

            _SourceImageFileName = new FileInfo(SourceImageFileName);
            _NewImageFileName = NewImageFileName;
        }

        public ImageTool(FileInfo ImageFileInfo, String NewImageFileName) {
            _SourceImageFileName = ImageFileInfo;
            _NewImageFileName = NewImageFileName;
        }

        public ImageTool(FileInfo ImageFileInfo) {
            _SourceImageFileName = ImageFileInfo;
        }

        public FileInfo ResizeImage(float percentReduction) {
            return ResizeImage(_SourceImageFileName, percentReduction, _NewImageFileName);
        }
        public FileInfo ResizeImage()
        {
            return ResizeImage(_SourceImageFileName, _DefaultPercentReduction, _NewImageFileName);
        }

        public FileInfo ResizeImage(FileInfo SourceImageFile, float percentReduction, String newFileName = null)
        {

            String tmpImageFileName = SourceImageFile.DirectoryName + "\\tmp_" + SourceImageFile.Name;
            Bitmap pictureFile = new Bitmap(SourceImageFile.FullName);
            int width = (int)(pictureFile.Width - (pictureFile.Width * percentReduction));
            int height = (int)(pictureFile.Height - (pictureFile.Height * percentReduction));

            Bitmap resizedPicture = new Bitmap(width, height);
            Graphics graphic = Graphics.FromImage(resizedPicture);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(pictureFile, new Rectangle(0, 0, width, height), new Rectangle(0, 0, pictureFile.Width, pictureFile.Height), GraphicsUnit.Pixel);
            graphic.Dispose();
            pictureFile.Dispose();

            resizedPicture.Save(tmpImageFileName, ImageFormat.Jpeg);
            resizedPicture.Dispose();

            FileInfo ResultFile = new FileInfo(tmpImageFileName);

            if (newFileName != null) {
                String tmpNewfileName=  ResultFile.Directory.FullName + "\\" + newFileName;
                if (System.IO.File.Exists(tmpNewfileName))
                    System.IO.File.Delete(tmpNewfileName);

                ResultFile.MoveTo(tmpNewfileName);
                ResultFile = new FileInfo(tmpNewfileName);
            }


            return ResultFile;
        
        }
    }
}
