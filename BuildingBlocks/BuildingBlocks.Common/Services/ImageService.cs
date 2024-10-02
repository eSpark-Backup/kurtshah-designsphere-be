namespace BuildingBlocks.Common.Services;

public class ImageService : IImageService
{
    public string UploadImage(Stream data, string destination, string module)
    {
        data.Position = 0;
        if (!Directory.Exists(destination + "/" + module))
            Directory.CreateDirectory(destination + "/" + module);

        var fnname = module + "/" + DateTime.Now.Ticks.ToString() + ".jpg";
        var up = Path.Combine(destination, fnname);
        try
        {
            using (var filestream = new FileStream(up, FileMode.Create))
            {
                data.CopyTo(filestream);
                return fnname;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
