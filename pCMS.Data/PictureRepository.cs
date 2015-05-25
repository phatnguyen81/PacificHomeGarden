using pCMS.Core;

namespace pCMS.Data
{
    public interface IPictureRepository : IRepository<Picture>
    {
    }
    public class PictureRepository : EfRepository<Picture>, IPictureRepository
    {
        public PictureRepository(pCMSEntities context) : base(context) { }
    }
    //public class PictureRepository
    //{

    //    private static readonly object SLock = new object();
    //    private readonly pCMSEntities _entities;


    //    public long ImageQuality
    //    {
    //        get
    //        {
    //            return 100L;
    //        }
    //    }
    //    public string LocalThumbImagePath
    //    {
    //        get
    //        {
    //            var path = AppSettings.PicturePath + "content\\images\\thumbs";
    //            return path;
    //        }
    //    }
    //    public string LocalImagePath
    //    {
    //        get
    //        {
    //            var path = AppSettings.PicturePath + "content\\images";
    //            return path;
    //        }
    //    }
    //    public string ImageUrl
    //    {
    //        get
    //        {
    //            var path = AppSettings.RelatePicturePath + "content/images/";
    //            return path;
    //        }
    //    }
    //    public string ImageThumbUrl
    //    {
    //        get
    //        {
    //            var path = AppSettings.RelatePicturePath + "content/images/thumbs/";
    //            return path;
    //        }
    //    }
    //    public PictureRepository()
    //    {
    //        _entities = new pCMSEntities();
    //    }
    //    public PictureRepository(pCMSEntities entities)
    //    {
    //        _entities = entities;
    //    }
    //    public Picture GetById(Guid id)
    //    {
    //        return _entities.Pictures.FirstOrDefault(q => q.Id == id);
    //    }
    //    public ProductAttribute GetProductAttributeById(Guid id)
    //    {
    //        return _entities.ProductAttributes.FirstOrDefault(q => q.Id == id);
    //    }
    //    public IEnumerable<Picture> GetAll()
    //    {
    //        return _entities.Pictures;
    //    }
    //    public void Add(Picture picture)
    //    {
    //        //if (!AppSettings.StoreInDb)
    //        //{
    //        //    SavePictureInFile(picture.Id, picture.PictureBinary, picture.MimeType);
    //        //    picture.PictureBinary = null;
    //        //}
    //        //_entities.AddToPictures(picture);
    //        ////_entities.SaveChanges();
    //        _entities.Pictures.AddObject(picture);
    //    }
    //    public void Delete(Picture picture)
    //    {
    //        _entities.Pictures.DeleteObject(picture);
    //    }

    //    public void Delete(Guid id)
    //    {
    //        var picture = GetById(id);

    //        var parts = picture.MimeType.Split('/');
    //        var lastPart = parts[parts.Length - 1];
    //        switch (lastPart)
    //        {
    //            case "pjpeg":
    //                lastPart = "jpg";
    //                break;
    //            case "x-png":
    //                lastPart = "png";
    //                break;
    //            case "x-icon":
    //                lastPart = "ico";
    //                break;
    //        }

    //        var filter = string.Format("{0}*.*", picture.Id.ToString("N"));
    //        var currentFiles = Directory.GetFiles(LocalThumbImagePath, filter);
    //        foreach (var currentFileName in currentFiles)
    //        {
    //            var localThumbPathFile = Path.Combine(LocalThumbImagePath, currentFileName);
    //            if (File.Exists(localThumbPathFile))
    //                File.Delete(localThumbPathFile);
    //        }
                
    //        _entities.Pictures.DeleteObject(GetById(id));
           
    //    }

    //    public void Commit()
    //    {
    //        _entities.SaveChanges();
    //    }

    //    public void Update(Picture picture)
    //    {
            
    //    }
 
    //    //private void SavePictureInFile(Guid pictureId, byte[] pictureBinary, string mimeType)
    //    //{
    //    //    string[] parts = mimeType.Split('/');
    //    //    string lastPart = parts[parts.Length - 1];
    //    //    switch (lastPart)
    //    //    {
    //    //        case "pjpeg":
    //    //            lastPart = "jpg";
    //    //            break;
    //    //        case "x-png":
    //    //            lastPart = "png";
    //    //            break;
    //    //        case "x-icon":
    //    //            lastPart = "ico";
    //    //            break;
    //    //    }
    //    //    if(!Directory.Exists(LocalThumbImagePath))
    //    //    {
    //    //        Directory.CreateDirectory(LocalThumbImagePath);
    //    //    }
    //    //    var localFilename = string.Format("{0}_0.{1}", pictureId.ToString("N"), lastPart);
    //    //    File.WriteAllBytes(Path.Combine(LocalThumbImagePath, localFilename), pictureBinary);
    //    //}
    //    //public string GetPictureUrl(Guid pictureId, int targetSize = 0, bool showDefaultPicture = true)
    //    //{
    //    //    return GetPictureUrl(GetById(pictureId), targetSize, showDefaultPicture);
    //    //}
    //    //public string GetPictureUrl(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
    //    //{
    //    //    var url = string.Empty;
    //    //    if (picture == null || LoadPictureBinary(picture).Length == 0)
    //    //    {
    //    //        if (showDefaultPicture)
    //    //        {
    //    //            url = VirtualPathUtility.ToAbsolute(GetDefaultPictureUrl(targetSize));
    //    //        }
    //    //        return url;
    //    //    }

    //    //    var parts = picture.MimeType.Split('/');
    //    //    var lastPart = parts[parts.Length - 1];
    //    //    switch (lastPart)
    //    //    {
    //    //        case "pjpeg":
    //    //            lastPart = "jpg";
    //    //            break;
    //    //        case "x-png":
    //    //            lastPart = "png";
    //    //            break;
    //    //        case "x-icon":
    //    //            lastPart = "ico";
    //    //            break;
    //    //    }

    //    //    string localFilename;

    //    //    lock (SLock)
    //    //    {
    //    //        if (targetSize == 0)
    //    //        {
    //    //            localFilename = string.Format("{0}_0.{1}", picture.Id.ToString("N"), lastPart);
    //    //            if (!File.Exists(Path.Combine(LocalThumbImagePath, localFilename)))
    //    //            {
    //    //                if (!Directory.Exists(LocalThumbImagePath))
    //    //                {
    //    //                    Directory.CreateDirectory(LocalThumbImagePath);
    //    //                }
    //    //                File.WriteAllBytes(Path.Combine(LocalThumbImagePath, localFilename), LoadPictureBinary(picture));
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            localFilename = string.Format("{0}_{1}.{2}", picture.Id.ToString("N"), targetSize, lastPart);
    //    //            if (!File.Exists(Path.Combine(LocalThumbImagePath, localFilename)))
    //    //            {
    //    //                if (!Directory.Exists(LocalThumbImagePath))
    //    //                {
    //    //                    Directory.CreateDirectory(LocalThumbImagePath);
    //    //                }
    //    //                using (var stream = new MemoryStream(LoadPictureBinary(picture)))
    //    //                {
    //    //                    var b = new Bitmap(stream);

    //    //                    var newSize = CalculateDimensions(b.Size, targetSize);

    //    //                    if (newSize.Width < 1)
    //    //                        newSize.Width = 1;
    //    //                    if (newSize.Height < 1)
    //    //                        newSize.Height = 1;

    //    //                    var newBitMap = new Bitmap(newSize.Width, newSize.Height);
    //    //                    var g = Graphics.FromImage(newBitMap);
    //    //                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
    //    //                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    //    //                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
    //    //                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
    //    //                    g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
    //    //                    var ep = new EncoderParameters();
    //    //                    ep.Param[0] = new EncoderParameter(Encoder.Quality, this.ImageQuality);
    //    //                    var ici = GetImageCodecInfoFromExtension(lastPart) ??
    //    //                              GetImageCodecInfoFromMimeType("image/jpeg");
    //    //                    newBitMap.Save(Path.Combine(LocalThumbImagePath, localFilename), ici, ep);
    //    //                    newBitMap.Dispose();
    //    //                    b.Dispose();
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //    url = VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(ImageThumbUrl), localFilename);
    //    //    return url;
    //    //}

    //    //public byte[] LoadPictureBinary(Picture picture)
    //    //{
    //    //    return LoadPictureBinary(picture, AppSettings.StoreInDb);
    //    //}

    //    //public byte[] LoadPictureBinary(Picture picture, bool fromDb)
    //    //{
    //    //    if (picture == null)
    //    //        throw new ArgumentNullException("picture");

    //    //    byte[] result = null;
    //    //    result = fromDb ? picture.PictureBinary : LoadPictureFromFile(picture.Id, picture.MimeType);
    //    //    return result;
    //    //}
    //    //public byte[] LoadPictureFromFile(Guid pictureId, string mimeType)
    //    //{
    //    //    var parts = mimeType.Split('/');
    //    //    var lastPart = parts[parts.Length - 1];
    //    //    switch (lastPart)
    //    //    {
    //    //        case "pjpeg":
    //    //            lastPart = "jpg";
    //    //            break;
    //    //        case "x-png":
    //    //            lastPart = "png";
    //    //            break;
    //    //        case "x-icon":
    //    //            lastPart = "ico";
    //    //            break;
    //    //    }
    //    //    string localFilename = string.Format("{0}_0.{1}", pictureId.ToString("N"), lastPart);
    //    //    if (!File.Exists(Path.Combine(LocalThumbImagePath, localFilename)))
    //    //        return new byte[0];
    //    //    return File.ReadAllBytes(Path.Combine(LocalThumbImagePath, localFilename));
    //    //}
    //    //public string GetDefaultPictureUrl(int targetSize = 0, PictureType defaultPictureType = PictureType.Entity)
    //    //{
    //    //    string defaultImageName;
    //    //    switch (defaultPictureType)
    //    //    {
    //    //        case PictureType.Entity:
    //    //            defaultImageName = AppSettings.DefaultImageName;
    //    //            break;
    //    //        case PictureType.Avatar:
    //    //            defaultImageName = AppSettings.DefaultAvatarImageName;
    //    //            break;
    //    //        default:
    //    //            defaultImageName = AppSettings.DefaultImageName;
    //    //            break;
    //    //    }


    //    //    var relPath = AppSettings.RelatePicturePath + defaultImageName;
    //    //    if (targetSize == 0)
    //    //        return relPath;
    //    //    var filePath = Path.Combine(LocalThumbImagePath, defaultImageName);
    //    //    if (File.Exists(filePath))
    //    //    {
    //    //        var fileExtension = Path.GetExtension(filePath);
    //    //        var fname = string.Format("{0}_{1}{2}",
    //    //                                  Path.GetFileNameWithoutExtension(filePath),
    //    //                                  targetSize,
    //    //                                  fileExtension);
    //    //        if (!File.Exists(Path.Combine(LocalThumbImagePath, fname)))
    //    //        {
    //    //            var b = new Bitmap(filePath);

    //    //            var newSize = CalculateDimensions(b.Size, targetSize);

    //    //            if (newSize.Width < 1)
    //    //                newSize.Width = 1;
    //    //            if (newSize.Height < 1)
    //    //                newSize.Height = 1;

    //    //            var newBitMap = new Bitmap(newSize.Width, newSize.Height);
    //    //            var g = Graphics.FromImage(newBitMap);
    //    //            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
    //    //            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    //    //            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
    //    //            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
    //    //            g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
    //    //            var ep = new EncoderParameters();
    //    //            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, this.ImageQuality);
    //    //            ImageCodecInfo ici = GetImageCodecInfoFromExtension(fileExtension);
    //    //            if (ici == null)
    //    //                ici = GetImageCodecInfoFromMimeType("image/jpeg");
    //    //            newBitMap.Save(Path.Combine(LocalThumbImagePath, fname), ici, ep);
    //    //            newBitMap.Dispose();
    //    //            b.Dispose();
    //    //        }
    //    //        //return _webHelper.GetStoreLocation() + "content/images/thumbs/" + fname;
    //    //    }
    //    //    return relPath;
    //    //}

    //}
}